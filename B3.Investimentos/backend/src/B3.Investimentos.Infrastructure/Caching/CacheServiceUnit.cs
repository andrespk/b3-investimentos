using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json;
using B3.Investimentos.Infrastructure.Caching.Abstractions;
using Microsoft.Extensions.Configuration;

namespace B3.Investimentos.Infrastructure.Caching;

[ExcludeFromCodeCoverage]
public class CacheServiceUnit(ICacheProvider provider, IConfiguration configuration) : ICacheService
{
    public Task<T?> ObterAsync<T>(string cacheKey, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return provider.ObterAsync<T>(cacheKey, cancellationToken);
    }

    public Task CriarOuAtualizarAsync<T>(string cacheKey, T valor, TimeSpan ttl, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        return provider.CriarOuAtualizarAsync(cacheKey, valor, ttl, cancellationToken);
    }

    public string GerarCacheKey<T>(T chave)
    {
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(chave)));
    }
}