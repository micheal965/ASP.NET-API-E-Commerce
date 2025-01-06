
namespace Talabat.Core.Entities
{
    public class CustomerBasket
    {
        public string Id { get; set; }
        public List<BasketProducts> BasketProducts { get; set; }
        public int DeliveryMethodId { get; set; }
        public decimal ShippingPrice { get; set; }
        public string PaymentIntentId { get; set; }
        public string ClientSecret { get; set; }
        public CustomerBasket(string id)
        {
            Id = id;
            BasketProducts = new List<BasketProducts>();
        }
    }
}
