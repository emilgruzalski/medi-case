using Microsoft.EntityFrameworkCore;
using Premedical.Authentication;
using Premedical.Models;
using Premedical.Services.Interfaces;

namespace Premedical.Seeders;

public class DataSeeder
{
    private const string AdminEmailKey = "Seed:AdminEmail";
    private const string AdminPasswordKey = "Seed:AdminPassword";

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
        new() { Id = 1, LangValue = "pl", Name = "Polski" },
        new() { Id = 2, LangValue = "en", Name = "English" }
    };

    private readonly MedicalContext _context;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IConfiguration _configuration;
    private readonly ILogger<DataSeeder> _logger;

    public DataSeeder(
        MedicalContext context,
        IPasswordHasher passwordHasher,
        IConfiguration configuration,
        ILogger<DataSeeder> logger)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        await _context.Database.MigrateAsync();
        await SeedEntityTypesAsync();
        await SeedLanguagesAsync();
        await SeedAdminAsync();
    }

    private async Task SeedEntityTypesAsync()
    {
        var existingIds = await _context.EntityTypes.Select(t => t.Id).ToListAsync();
        var missing = EntityTypes.Where(t => !existingIds.Contains(t.Id)).ToList();
        if (missing.Count == 0)
            return;

        _context.EntityTypes.AddRange(missing.Select(t => new EntityType { Id = t.Id, Name = t.Name }));
        await _context.SaveChangesAsync();
    }

    private async Task SeedLanguagesAsync()
    {
        if (await _context.EntityLanguages.AnyAsync())
            return;

        _context.EntityLanguages.AddRange(
            DefaultLanguages.Select(l => new EntityLanguage { Id = l.Id, LangValue = l.LangValue, Name = l.Name }));
        await _context.SaveChangesAsync();
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
