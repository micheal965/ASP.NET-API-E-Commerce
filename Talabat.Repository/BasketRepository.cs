using StackExchange.Redis;
using System.Text.Json;
using Talabat.Core.Entities;
using Talabat.Core.IRepositories;

namespace Talabat.Repository
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDatabase database;
        public BasketRepository(IConnectionMultiplexer connectionMultiplexer)
        {
            database = connectionMultiplexer.GetDatabase();
        }
        public async Task<bool> DeleteBasketAsync(string Basketid)
        {
            return await database.KeyDeleteAsync(Basketid);
        }

        public async Task<CustomerBasket?> GetBasketAsync(string Basketid)
        {
            RedisValue basket = await database.StringGetAsync(Basketid);
            return basket.HasValue ? JsonSerializer.Deserialize<CustomerBasket>(basket) : null;
        }

        public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket Basket)
        {
            bool FlagForCreatingOrUpdating = await database.StringSetAsync(Basket.Id, JsonSerializer.Serialize(Basket), TimeSpan.FromDays(1));
            if (FlagForCreatingOrUpdating)
                return await GetBasketAsync(Basket.Id);
            else
                return null;
        }
    }
}
