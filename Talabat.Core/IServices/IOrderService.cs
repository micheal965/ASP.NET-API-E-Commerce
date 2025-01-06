using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Core.IServices
{
    public interface IOrderService
    {
        Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int deliverymethodId, Address shippingaddress);
        Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail);

        Task<Order> GetOrderByIdForUserAsync(int orderId, string buyerEmail);
        Task<IReadOnlyList<DeliveryMethod?>> GetDeliveryMethods();
    }
}
