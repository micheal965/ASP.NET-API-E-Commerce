using StackExchange.Redis;
using System.Text.Json;
using Talabat.Core.IServices;

namespace Talabat.Service
{
    public class ResponseCacheService : IResponseCacheService
    {
        private readonly IDatabase _database;

        public ResponseCacheService(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }
        public async Task CacheResponseAsync(string cachekey, object Response, TimeSpan timetolive)
        {
            if (Response == null) return;

            var JsonType = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

            var SerializedResponse = JsonSerializer.Serialize(Response, JsonType);

            await _database.StringSetAsync(cachekey, SerializedResponse, timetolive);
        }

        public async Task<string?> GetCachedResponseAsync(string cachekey)
        {
            var CachedResponse = await _database.StringGetAsync(cachekey);
            if (CachedResponse.IsNullOrEmpty) return null;
            return CachedResponse;

        }
    }
}
