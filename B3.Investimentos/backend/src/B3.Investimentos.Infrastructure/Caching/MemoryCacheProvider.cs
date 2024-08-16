using B3.Investimentos.Infrastructure.Caching.Abstractions;
using Microsoft.Extensions.Caching.Memory;

namespace B3.Investimentos.Infrastructure.Caching;

public sealed class MemoryCacheProvider(IMemoryCache cache) : ICacheProvider
{
    public Task<T?> ObterAsync<T>(string cacheKey, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return Task.FromResult(cache.Get<T>(cacheKey));
    }

    public Task RegistrarAsync<T>(string cacheKey, T valor, TimeSpan ttl, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var cacheOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = ttl
        };

        return Task.FromResult(cache.Set(cacheKey, valor, cacheOptions));
    }
}