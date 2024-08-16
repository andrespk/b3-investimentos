namespace B3.Investimentos.Infrastructure.Caching.Abstractions;

public interface ICacheService
{
    Task<T?> ObterAsync<T>(string cacheKey, CancellationToken cancellationToken);
    Task RegistrarAsync<T>(string cacheKey, T valor, TimeSpan ttl, CancellationToken cancellationToken);
    string GerarChave<T>(T chave);
}