using Microsoft.EntityFrameworkCore;
using Premedical.DataTransferObjects;
using Premedical.Exceptions;
using Premedical.Models;
using Premedical.Repositories;
using Premedical.Services;
using Premedical.Tests.Helpers;

namespace Premedical.Tests.Services;

public class EntityServiceTests
{
    private readonly MedicalContext _context = TestDatabase.Create(TestDatabase.UniqueName());

    private EntityService CreateService() => new(
        new EntityRepository(_context),
        new EntityTypeRepository(_context),
        new EntityLanguageRepository(_context),
        new EntityGraphDataRepository(_context),
        TestMapper.Create());

    [Fact]
    public async Task CreateAsync_RejectsTypeNotAllowedUnderParent()
    {
        await TestDatabase.SeedStructureAsync(_context);
        var service = CreateService();
        var content = await service.CreateAsync(new EntityPostDTO { TypeId = EntityTypeIds.Category });

        await Assert.ThrowsAsync<BadRequestException>(() => service.CreateAsync(
            new EntityPostDTO { TypeId = EntityTypeIds.TestAnswer, ParentId = content.Id }));
    }

    [Fact]
    public async Task CreateAsync_PlacesNewEntityAfterExistingSiblings()
    {
        await TestDatabase.SeedStructureAsync(_context);
        var service = CreateService();
        var category = await service.CreateAsync(new EntityPostDTO { TypeId = EntityTypeIds.Category });

        await service.CreateAsync(new EntityPostDTO { TypeId = EntityTypeIds.Content, ParentId = category.Id });
        var second = await service.CreateAsync(new EntityPostDTO
        {
            TypeId = EntityTypeIds.Content,
            ParentId = category.Id,
            Translations = { new EntityTranslationContentDTO { LangId = 1, Title = "Druga karta" } }
        });

        Assert.Equal(1, second.Order);
        Assert.Equal("Druga karta", second.Translations.Single().Title);
    }

    [Fact]
    public async Task DeleteAsync_RemovesWholeSubtree()
    {
        await TestDatabase.SeedStructureAsync(_context);
        var service = CreateService();
        var category = await service.CreateAsync(new EntityPostDTO { TypeId = EntityTypeIds.Category });
        var test = await service.CreateAsync(new EntityPostDTO { TypeId = EntityTypeIds.Test, ParentId = category.Id });
        await service.CreateAsync(new EntityPostDTO { TypeId = EntityTypeIds.TestQuestion, ParentId = test.Id });

        await service.DeleteAsync(category.Id);

        Assert.Equal(0, await _context.Entities.CountAsync());
        Assert.Equal(0, await _context.EntitiesGraphData.CountAsync());
    }
}
