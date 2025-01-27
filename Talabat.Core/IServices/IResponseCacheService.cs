namespace Talabat.Core.IServices
{
    public interface IResponseCacheService
    {
        Task CacheResponseAsync(string cachekey, object Response, TimeSpan timetolive);
        Task<string?> GetCachedResponseAsync(string cachekey);
    }
}
