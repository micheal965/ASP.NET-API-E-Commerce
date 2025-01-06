using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Repository.Data.Config
{
    internal class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            //encapsulating shippingaddress into order 
            builder.OwnsOne(O => O.ShippingAddress, ShippingAddress => ShippingAddress.WithOwner());

            //Saving Orderstatus in Db as a string and retrieve as a Enum
            builder.Property(O => O.OrderStatus)
                .HasConversion(
                 OStatus => OStatus.ToString(),
                 OStatus => (OrderStatus)Enum.Parse(typeof(OrderStatus), OStatus)
                );

            //Mapping Deliverymethod [1:many] 
            //Order Mandatory ,Deliverymethod optional
            builder.HasOne(O => O.DeliveryMethod)
                    .WithMany(d => d.Order)
                    .HasForeignKey(o => o.DeliveryMethodId)
                    .OnDelete(DeleteBehavior.SetNull);

            builder.Property(O => O.Subtotal).
                HasColumnType("decimal(18,2)");

            //Mapping Order and Orderitems
            //1:Many Order optional orderitems mandatory
            //builder.HasMany(o => o.Items)
            //    .WithOne(oi => oi.Order)
            //    .HasForeignKey(oi => oi.OrderId)
            //    .IsRequired();

        }
    }
}
