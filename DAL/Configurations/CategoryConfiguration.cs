using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.Property(x => x.Id).HasDefaultValueSql("NEWID()");
        builder.HasIndex(x => x.Name);
        builder.HasMany<Article>()
            .WithOne(article => article.Category)
            .HasForeignKey(article => article.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}