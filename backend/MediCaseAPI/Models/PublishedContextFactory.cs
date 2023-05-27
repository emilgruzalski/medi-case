using Microsoft.EntityFrameworkCore;

namespace MediCaseAPI.Models;

public class PublishedContextFactory : IPublishedContextFactory
{
    private readonly DbContextOptions<MedicalContext> _options;

    public PublishedContextFactory(string connectionString)
    {
        _options = new DbContextOptionsBuilder<MedicalContext>()
            .UseSqlServer(connectionString)
            .Options;
    }

    public MedicalContext CreateDbContext() => new(_options);
}
