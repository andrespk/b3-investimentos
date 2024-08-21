using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using B3.Investimentos.Infrastructure.Caching.Abstractions;

namespace B3.Investimentos.Infrastructure.Caching;

[ExcludeFromCodeCoverage]
public class CacheService(ICacheProvider provider) : ICacheService
{
    public Task<T?> ObterAsync<T>(string cacheKey, CancellationToken cancellationToken) =>
        provider.ObterAsync<T>(cacheKey, cancellationToken);

    public Task RegistrarAsync<T>(string cacheKey, T valor, TimeSpan ttl, CancellationToken cancellationToken) =>
        provider.RegistrarAsync(cacheKey, valor, ttl, cancellationToken);

    public string GerarChave<T>(T chave) => JsonSerializer.Serialize(chave).Replace(" ", string.Empty);
}