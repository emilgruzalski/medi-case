namespace Premedical.Models;

/// <summary>
/// Creates contexts for the database with published content that learners read from.
/// The moderator database (registered as <see cref="MedicalContext"/>) is the working copy.
/// </summary>
public interface IPublishedContextFactory
{
    MedicalContext CreateDbContext();
}
