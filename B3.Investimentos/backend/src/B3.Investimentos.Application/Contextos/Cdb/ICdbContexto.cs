using B3.Investimentos.Domain.Cdb.Abstractions;
using B3.Investimentos.Infrastructure.Caching.Abstractions;

namespace B3.Investimentos.Application.Contextos.Cdb;

public interface ICdbContexto : IContexto
{
    ICdbService CdbService { get; }
    ICacheService CacheService { get; }
}