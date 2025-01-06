using System.ComponentModel.DataAnnotations.Schema;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.APIs.DTOs.Order
{
    public class OrderToReturnDto
    {
        public int Id { get; set; }
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; }

        public OrderStatus OrderStatus { get; set; }
        //encapsulation
        public Address ShippingAddress { get; set; }
        public string DeliveryMethodName { get; set; }
        public decimal DeliverMethodCost { get; set; }
        [ForeignKey("Items")]
        public int ItemsId { get; set; }
        public ICollection<OrderItemDto> Items { get; set; } = new HashSet<OrderItemDto>();
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }
        public string PaymentIntentId { get; set; } = string.Empty;
    }
}
