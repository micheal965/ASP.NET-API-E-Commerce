using Talabat.Core.Entities;

namespace Talabat.Core.IServices
{
    public interface IPaymentService
    {
        Task<CustomerBasket> CreateOrUpdatePaymentIntentAsync(string BasketId);
    }
}
