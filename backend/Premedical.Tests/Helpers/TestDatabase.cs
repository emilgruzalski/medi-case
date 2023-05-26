using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Premedical.Models;

namespace Premedical.Tests.Helpers;

public static class TestDatabase
{
    public static MedicalContext Create(string name) =>
        new(new DbContextOptionsBuilder<MedicalContext>()
            .UseInMemoryDatabase(name)
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options);

    public static string UniqueName() => Guid.NewGuid().ToString();

    public static async Task SeedStructureAsync(MedicalContext context)
    {
        context.EntityTypes.AddRange(
            new[] { EntityTypeIds.Category, EntityTypeIds.Content, EntityTypeIds.QuestionList, EntityTypeIds.Question,
                    EntityTypeIds.Test, EntityTypeIds.TestQuestion, EntityTypeIds.TestAnswer }
                .Select(id => new EntityType { Id = id, Name = $"Type{id}" }));
        context.EntityLanguages.AddRange(
            new EntityLanguage { Id = 1, LangValue = "pl", Name = "Polski" },
            new EntityLanguage { Id = 2, LangValue = "en", Name = "English" });
        await context.SaveChangesAsync();
    }
}

public class TestPublishedContextFactory : IPublishedContextFactory
{
    private readonly string _name = TestDatabase.UniqueName();

    public MedicalContext CreateDbContext() => TestDatabase.Create(_name);
}
