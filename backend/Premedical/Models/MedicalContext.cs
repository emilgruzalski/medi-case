using Microsoft.EntityFrameworkCore;

namespace Premedical.Models;

public partial class MedicalContext : DbContext
{
    public MedicalContext(DbContextOptions<MedicalContext> options)
        : base(options)
    {
    }

    public virtual DbSet<EntitiesGraphDatum> EntitiesGraphData { get; set; }

    public virtual DbSet<Entity> Entities { get; set; }

    public virtual DbSet<EntityDictionary> EntityDictionaries { get; set; }

    public virtual DbSet<EntityLanguage> EntityLanguages { get; set; }

    public virtual DbSet<EntityTranslation> EntityTranslations { get; set; }

    public virtual DbSet<EntityType> EntityTypes { get; set; }

    public virtual DbSet<Session> Sessions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EntitiesGraphDatum>(entity =>
        {
            entity.HasIndex(e => e.ChildId).IsUnique();

            entity.HasOne(d => d.Child).WithOne(p => p.ParentLink)
                .HasForeignKey<EntitiesGraphDatum>(d => d.ChildId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.Parent).WithMany(p => p.ChildLinks)
                .HasForeignKey(d => d.ParentId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<Entity>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.ModifiedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Type).WithMany(p => p.Entities)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<EntityDictionary>(entity =>
        {
            entity.Property(e => e.Term).HasMaxLength(FieldLengths.Term);
            entity.Property(e => e.Definition).HasMaxLength(FieldLengths.Definition);

            entity.HasOne(d => d.Entity).WithMany(p => p.EntityDictionaries)
                .HasForeignKey(d => d.EntityId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.Lang).WithMany(p => p.EntityDictionaries)
                .HasForeignKey(d => d.LangId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<EntityLanguage>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.HasIndex(e => e.LangValue).IsUnique();
            entity.Property(e => e.LangValue).HasMaxLength(FieldLengths.LanguageCode);
            entity.Property(e => e.Name).HasMaxLength(FieldLengths.Name);
        });

        modelBuilder.Entity<EntityTranslation>(entity =>
        {
            entity.HasIndex(e => new { e.EntityId, e.LangId }).IsUnique();
            entity.Property(e => e.Title).HasMaxLength(FieldLengths.Title);
            entity.Property(e => e.ModifiedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Entity).WithMany(p => p.EntityTranslations)
                .HasForeignKey(d => d.EntityId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.Lang).WithMany(p => p.EntityTranslations)
                .HasForeignKey(d => d.LangId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<EntityType>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(FieldLengths.Name);
        });

        modelBuilder.Entity<Session>(entity =>
        {
            entity.HasIndex(e => e.Token).IsUnique();
            entity.Property(e => e.Token).HasMaxLength(FieldLengths.SessionToken);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.ExpiresAt).HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.Sessions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.Email).HasMaxLength(FieldLengths.Email);
            entity.Property(e => e.FirstName).HasMaxLength(FieldLengths.PersonName);
            entity.Property(e => e.LastName).HasMaxLength(FieldLengths.PersonName);
            entity.Property(e => e.Role).HasMaxLength(FieldLengths.Role);
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.LastLoginAt).HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
