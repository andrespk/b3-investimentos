namespace B3.Investimentos.Infrastructure.Caching.Abstractions;

public interface ICacheProvider
{
    Task<T?> ObterAsync<T>(string cacheKey, CancellationToken cancellationToken);
    Task CriarOuAtualizarAsync<T>(string cacheKey, T valor, TimeSpan ttl, CancellationToken cancellationToken);
}