using Microsoft.EntityFrameworkCore;
using Premedical.Models;

namespace Premedical.Seeders;

public class SampleContentSeeder
{
    private readonly MedicalContext _context;

    public SampleContentSeeder(MedicalContext context)
    {
        _context = context;
    }

    /// <returns>True when the sample content was added.</returns>
    public async Task<bool> SeedAsync()
    {
        if (await _context.Entities.AnyAsync())
            return false;

        var now = DateTime.UtcNow;
        var roots = SampleContent.Tree.Select((node, index) => AddNode(node, null, index, now)).ToList();

        foreach (var term in SampleContent.AssessmentDictionary)
        {
            AddTerm(roots[0], DefaultLanguageIds.Polish, term.TermPl, term.DefinitionPl);
            AddTerm(roots[0], DefaultLanguageIds.English, term.TermEn, term.DefinitionEn);
        }

        await _context.SaveChangesAsync();
        return true;
    }

    private Entity AddNode(SampleNode node, Guid? parentId, int order, DateTime now)
    {
        var entity = new Entity
        {
            Id = Guid.NewGuid(),
            TypeId = node.TypeId,
            Order = order,
            IsCorrect = node.IsCorrect,
            CreatedAt = now,
            ModifiedAt = now
        };
        entity.ParentLink = new EntitiesGraphDatum { Id = Guid.NewGuid(), ParentId = parentId, ChildId = entity.Id };
        entity.EntityTranslations.Add(NewTranslation(DefaultLanguageIds.Polish, node.TitlePl, node.ContentPl, now));
        entity.EntityTranslations.Add(NewTranslation(DefaultLanguageIds.English, node.TitleEn, node.ContentEn, now));
        _context.Entities.Add(entity);

        var children = node.Children ?? Array.Empty<SampleNode>();
        for (var i = 0; i < children.Length; i++)
            AddNode(children[i], entity.Id, i, now);

        return entity;
    }

    private void AddTerm(Entity entity, int langId, string term, string definition) =>
        _context.EntityDictionaries.Add(new EntityDictionary
        {
            Id = Guid.NewGuid(),
            EntityId = entity.Id,
            LangId = langId,
            Term = term,
            Definition = definition
        });

    private static EntityTranslation NewTranslation(int langId, string title, string? content, DateTime now) => new()
    {
        Id = Guid.NewGuid(),
        LangId = langId,
        Title = title,
        Content = content,
        ModifiedAt = now
    };
}
