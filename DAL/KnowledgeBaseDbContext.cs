using DAL.Configurations;
using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public sealed class KnowledgeBaseDbContext : DbContext
{
    public DbSet<Category> Categories { get; init; }
    
    public DbSet<Article> Articles { get; init; }

    public KnowledgeBaseDbContext(DbContextOptions options)
        : base(options)
    {
        Database.EnsureCreated();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new ())
    {
        foreach (var auditableEntity in ChangeTracker.Entries<Auditable>())
        {
            if (auditableEntity.State == EntityState.Added)
            {
                var currentDate = DateTime.UtcNow;
                auditableEntity.Entity.CreatedAt = currentDate;
                auditableEntity.Entity.UpdatedAt = currentDate;
            }

            if (auditableEntity.State == EntityState.Modified)
            {
                auditableEntity.Property(x => x.CreatedAt).IsModified = false;
                auditableEntity.Entity.UpdatedAt = DateTime.UtcNow;
            }
        }
        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableDetailedErrors();
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ArticleConfiguration());
        modelBuilder.ApplyConfiguration(new CategoryConfiguration());
    }
}