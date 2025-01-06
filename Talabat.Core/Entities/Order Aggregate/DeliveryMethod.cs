
using System.Text.Json.Serialization;

namespace Talabat.Core.Entities.Order_Aggregate
{
    public class DeliveryMethod : BaseEntity
    {
        public DeliveryMethod()
        {

        }

        public DeliveryMethod(string shortName, string description, decimal cost, string deliveryTime)
        {
            ShortName = shortName;
            Description = description;
            Cost = cost;
            DeliveryTime = deliveryTime;
        }

        public string ShortName { get; set; }
        public string Description { get; set; }
        public decimal Cost { get; set; }
        public string DeliveryTime { get; set; }
        [JsonIgnore]
        public ICollection<Order> Order { get; set; } = new List<Order>();
    }
}
