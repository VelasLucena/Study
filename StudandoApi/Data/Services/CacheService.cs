using Microsoft.Extensions.Caching.Distributed;

namespace SudyApi.Data.Services
{
    public class CacheService
    {
        #region Field

        private readonly IDistributedCache _cache;
        private readonly DistributedCacheEntryOptions _options;

        #endregion

        #region Constructor

        public CacheService(IDistributedCache cache)
        {
            _cache = cache;
            _options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
                SlidingExpiration = TimeSpan.FromMinutes(5),
            };
        }

        #endregion

        public async Task<string> Get(string key)
        {
            return await _cache.GetStringAsync(key);
        }

        public async Task Set(string key, string valor)
        {
            await _cache.SetStringAsync(key, valor, _options);
        }

        public async Task Remove(string key)
        {
            await _cache.RemoveAsync(key);
        }
    }
}
