using Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.Infrastructure.Data.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            // Tłumaczymy EF Core, że właściwość 'Products' czyta z prywatnego pola '_products'
            builder.Metadata.FindNavigation(nameof(Category.Products))!
                .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}