using System.ComponentModel.DataAnnotations.Schema;

namespace Talabat.Core.Entities.Order_Aggregate
{
    public class OrderItem : BaseEntity
    {
        public OrderItem()
        {

        }
        public OrderItem(ProductItemOrdered product, int quantity, decimal price)
        {
            Product = product;
            Quantity = quantity;
            Price = price;
        }
        public ProductItemOrdered Product { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public Order Order { get; set; }

    }
}
