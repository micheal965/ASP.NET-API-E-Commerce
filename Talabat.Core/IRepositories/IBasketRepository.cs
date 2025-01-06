using Talabat.Core.Entities;

namespace Talabat.Core.IRepositories
{
    public interface IBasketRepository
    {
        public Task<CustomerBasket?> GetBasketAsync(string Basketid);
        public Task<bool> DeleteBasketAsync(string Basketid);
        public Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket Basket);
    }
}
