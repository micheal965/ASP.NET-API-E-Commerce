using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Talabat.Core.Entities;

namespace Talabat.Repository.Data.Config
{
    internal class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {

        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(p => p.Name)
                .HasMaxLength(50)
                .IsRequired();
        }
    }
}
