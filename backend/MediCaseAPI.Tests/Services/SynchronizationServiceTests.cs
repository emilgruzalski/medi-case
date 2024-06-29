using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using MediCaseAPI.DataTransferObjects;
using MediCaseAPI.Models;
using MediCaseAPI.Services;
using MediCaseAPI.Tests.Helpers;

namespace MediCaseAPI.Tests.Services;

public class SynchronizationServiceTests
{
    private readonly MedicalContext _moderator = TestDatabase.Create(TestDatabase.UniqueName());
    private readonly TestPublishedContextFactory _published = new();

    private SynchronizationService CreateService() =>
        new(_moderator, _published, new FakeFileStorage(), NullLogger<SynchronizationService>.Instance);

    [Fact]
    public async Task SynchronizeAsync_CopiesContentToPublishedDatabase()
    {
        await TestDatabase.SeedStructureAsync(_moderator);
        var root = EntityFactory.Create(EntityTypeIds.Category, null, "Badanie pacjenta");
        _moderator.Entities.Add(root);
        await _moderator.SaveChangesAsync();

        var result = await CreateService().SynchronizeAsync();

        await using var published = _published.CreateDbContext();
        Assert.Equal(1, result.Entities);
        Assert.Equal("Badanie pacjenta", (await published.EntityTranslations.SingleAsync()).Title);
    }

    [Fact]
    public async Task SynchronizeAsync_RemovesEntitiesDeletedByModerators()
    {
        await TestDatabase.SeedStructureAsync(_moderator);
        var root = EntityFactory.Create(EntityTypeIds.Category, null, "Do usunięcia");
        _moderator.Entities.Add(root);
        await _moderator.SaveChangesAsync();
        await CreateService().SynchronizeAsync();

        _moderator.EntitiesGraphData.Remove(root.ParentLink!);
        _moderator.Entities.Remove(root);
        await _moderator.SaveChangesAsync();
        await CreateService().SynchronizeAsync();

        await using var published = _published.CreateDbContext();
        Assert.Empty(await published.Entities.ToListAsync());
    }

    [Fact]
    public async Task GetPendingChangesAsync_ReportsAddedModifiedAndRemoved()
    {
        await TestDatabase.SeedStructureAsync(_moderator);
        var modified = EntityFactory.Create(EntityTypeIds.Category, null, "Zmieniony");
        var removed = EntityFactory.Create(EntityTypeIds.Category, null, "Usunięty");
        _moderator.Entities.AddRange(modified, removed);
        await _moderator.SaveChangesAsync();
        await CreateService().SynchronizeAsync();

        modified.ModifiedAt = modified.ModifiedAt.AddMinutes(1);
        _moderator.EntitiesGraphData.Remove(removed.ParentLink!);
        _moderator.Entities.Remove(removed);
        _moderator.Entities.Add(EntityFactory.Create(EntityTypeIds.Category, null, "Nowy"));
        await _moderator.SaveChangesAsync();

        var changes = await CreateService().GetPendingChangesAsync(langId: 1);

        Assert.False(changes.LanguagesChanged);
        Assert.Equal(PendingChangeType.Added, changes.Entities.Single(c => c.Title == "Nowy").ChangeType);
        Assert.Equal(PendingChangeType.Modified, changes.Entities.Single(c => c.Title == "Zmieniony").ChangeType);
        Assert.Equal(PendingChangeType.Removed, changes.Entities.Single(c => c.Title == "Usunięty").ChangeType);
    }
}
