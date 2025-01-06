using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Talabat.Core.Entities;

namespace Talabat.Repository.Data.Config
{
    internal class BrandConfiguration : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            builder.Property(p => p.Name)
                .HasMaxLength(50)
                .IsRequired();
        }
    }
}
