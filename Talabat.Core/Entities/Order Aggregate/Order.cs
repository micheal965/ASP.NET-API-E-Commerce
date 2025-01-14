using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Talabat.Core.Entities.Order_Aggregate
{
    public class Order : BaseEntity
    {
        public Order()
        {

        }

        public Order(string buyerEmail, Address shippingAddress, DeliveryMethod deliveryMethod, ICollection<OrderItem> items, decimal subtotal, string paymentintent)
        {
            BuyerEmail = buyerEmail;
            ShippingAddress = shippingAddress;
            DeliveryMethod = deliveryMethod;
            Items = items;
            Subtotal = subtotal;
            PaymentIntentId = paymentintent;
        }

        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow;

        public OrderStatus OrderStatus = OrderStatus.Pending;
        //encapsulation
        public Address ShippingAddress { get; set; }
        [ForeignKey("DeliveryMethod")]
        public int DeliveryMethodId { get; set; }
        public DeliveryMethod DeliveryMethod { get; set; }
        public ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>();//Navigational Property
        //Sum(Items)=>Quantity*Price
        public decimal Subtotal { get; set; }
        // subtotal+ DeliveryCost  =>Derived  
        [NotMapped]
        public decimal Total => Subtotal + DeliveryMethod.Cost;
        public string PaymentIntentId { get; set; }
    }
}
