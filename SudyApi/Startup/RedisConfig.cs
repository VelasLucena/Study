using Nest;
using StudandoApi.Data.Contexts;
using SudyApi.Models;
using SudyApi.Properties.Enuns;
using SudyApi.Utility;

namespace SudyApi.Startup
{
    public static class RedisConfig
    {
        public static void AddRedis(this IServiceCollection services)
        {
            services.AddStackExchangeRedisCache(x =>
            {
                x.InstanceName = nameof(SudyContext);
                x.Configuration = $"localhost:6379";
            });
        }
    }
}
