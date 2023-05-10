using Microsoft.EntityFrameworkCore;
using Premedical.Authentication;
using Premedical.Models;
using Premedical.Services.Interfaces;

namespace Premedical.Seeders;

public class DataSeeder
{
    private const string AdminEmailKey = "Seed:AdminEmail";
    private const string AdminPasswordKey = "Seed:AdminPassword";
    private const string SampleContentKey = "Seed:SampleContent";

    private static readonly EntityType[] EntityTypes =
    {
        new() { Id = EntityTypeIds.Category, Name = "Category" },
        new() { Id = EntityTypeIds.Content, Name = "Content" },
        new() { Id = EntityTypeIds.QuestionList, Name = "QuestionList" },
        new() { Id = EntityTypeIds.Question, Name = "Question" },
        new() { Id = EntityTypeIds.Test, Name = "Test" },
        new() { Id = EntityTypeIds.TestQuestion, Name = "TestQuestion" },
        new() { Id = EntityTypeIds.TestAnswer, Name = "TestAnswer" }
    };

    private static readonly EntityLanguage[] DefaultLanguages =
    {
        new() { Id = DefaultLanguageIds.Polish, LangValue = "pl", Name = "Polski" },
        new() { Id = DefaultLanguageIds.English, LangValue = "en", Name = "English" }
    };

    private readonly MedicalContext _context;
    private readonly IPublishedContextFactory _publishedContextFactory;
    private readonly IPasswordHasher _passwordHasher;
    private readonly SampleContentSeeder _sampleContentSeeder;
    private readonly ISynchronizationService _synchronizationService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<DataSeeder> _logger;

    public DataSeeder(
        MedicalContext context,
        IPublishedContextFactory publishedContextFactory,
        IPasswordHasher passwordHasher,
        SampleContentSeeder sampleContentSeeder,
        ISynchronizationService synchronizationService,
        IConfiguration configuration,
        ILogger<DataSeeder> logger)
    {
        _context = context;
        _publishedContextFactory = publishedContextFactory;
        _passwordHasher = passwordHasher;
        _sampleContentSeeder = sampleContentSeeder;
        _synchronizationService = synchronizationService;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        await SeedStructureAsync(_context);
        await SeedAdminAsync();

        await using var published = _publishedContextFactory.CreateDbContext();
        await SeedStructureAsync(published);

        if (_configuration.GetValue<bool>(SampleContentKey) && await _sampleContentSeeder.SeedAsync())
        {
            await _synchronizationService.SynchronizeAsync();
            _logger.LogInformation("Seeded and published sample content");
        }
    }

    private static async Task SeedStructureAsync(MedicalContext context)
    {
        await context.Database.MigrateAsync();
        await SeedEntityTypesAsync(context);
        await SeedLanguagesAsync(context);
    }

    private static async Task SeedEntityTypesAsync(MedicalContext context)
    {
        var existingIds = await context.EntityTypes.Select(t => t.Id).ToListAsync();
        var missing = EntityTypes.Where(t => !existingIds.Contains(t.Id)).ToList();
        if (missing.Count == 0)
            return;

        context.EntityTypes.AddRange(missing.Select(t => new EntityType { Id = t.Id, Name = t.Name }));
        await context.SaveChangesAsync();
    }

    private static async Task SeedLanguagesAsync(MedicalContext context)
    {
        if (await context.EntityLanguages.AnyAsync())
            return;

        context.EntityLanguages.AddRange(
            DefaultLanguages.Select(l => new EntityLanguage { Id = l.Id, LangValue = l.LangValue, Name = l.Name }));
        await context.SaveChangesAsync();
    }

    private async Task SeedAdminAsync()
    {
        var email = _configuration[AdminEmailKey];
        var password = _configuration[AdminPasswordKey];
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            return;

        email = email.Trim().ToLowerInvariant();
        if (await _context.Users.AnyAsync(u => u.Email == email))
            return;

        _context.Users.Add(new User
        {
            Email = email,
            PasswordHash = _passwordHasher.Hash(password),
            FirstName = "Admin",
            LastName = "MediCase",
            Role = Roles.Admin,
            CreatedAt = DateTime.UtcNow
        });
        await _context.SaveChangesAsync();
        _logger.LogInformation("Seeded administrator account {Email}", email);
    }
}
