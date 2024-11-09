using MarriageAgency.ViewModels;
using Microsoft.Extensions.Caching.Memory;
using MarriageAgency.Services;

namespace MarriageAgency.Middleware
{
    public class DbCacheMiddleware(RequestDelegate next, IMemoryCache memoryCache, string cacheKey = "Services 10")
    {
        private readonly RequestDelegate _next = next;
        private readonly IMemoryCache _memoryCache = memoryCache;
        private readonly string _cacheKey = cacheKey;

        public Task Invoke(HttpContext httpContext, IServiceService operationService)
        {
            // пытаемся получить элемент из кэша
            if (!_memoryCache.TryGetValue(_cacheKey, out HomeViewModel homeViewModel))
            {
                // если в кэше не найден элемент, получаем его от сервиса
                homeViewModel = operationService.GetHomeViewModel();
                // и сохраняем в кэше
                _memoryCache.Set(_cacheKey, homeViewModel,
                    new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(1)));

            }

            return _next(httpContext);
        }
    }

    // Метод расширения, используемый для добавления промежуточного программного обеспечения в конвейер HTTP-запроса.
    public static class DbCacheMiddlewareExtensions
    {
        public static IApplicationBuilder UseOperatinCache(this IApplicationBuilder builder, string cacheKey)
        {
            return builder.UseMiddleware<DbCacheMiddleware>(cacheKey);
        }
    }
}
