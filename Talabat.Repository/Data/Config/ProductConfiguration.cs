using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Talabat.Core.Entities;

namespace Talabat.Repository.Data.Config
{
    internal class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            //Properties Configurations
            builder.Property(p => p.Name)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.Description)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.PictureUrl)
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(p => p.Price)
                .HasColumnType("decimal(18,2)")
                .IsRequired();
            //Relationships
            builder.HasOne(p => p.Category)
                .WithMany()
                .HasForeignKey(p => p.CategoryId)
                .IsRequired();

            builder.HasOne(p => p.Brand)
                .WithMany()
                .HasForeignKey(p => p.BrandId)
                .IsRequired();

        }
    }
}
