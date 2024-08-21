using B3.Investimentos.Application.Constants;
using B3.Investimentos.Domain.Cdb.Abstractions;
using B3.Investimentos.Infrastructure.Caching.Abstractions;
using B3.Investimentos.Infrastructure.Caching.Constants;
using Microsoft.Extensions.Configuration;

namespace B3.Investimentos.Application.Services;

public class CdbService(ICacheService cacheService, IConfiguration configuration, IResgateCdb resgateCdb) : ICdbService
{
    public async Task<IResgateCdb> ResgatarAsync(ICdb cdb, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        var cacheKey = cacheService.GerarChave(cdb);
        var resgateEmCache = await cacheService.ObterAsync<IResgateCdb>(cacheKey, cancellationToken);
        if (resgateEmCache is not null) return resgateEmCache;
        var ttl = TimeSpan.FromSeconds(configuration.GetValue<int>(
            ConfiguracaoInfraestrutura.TempoDeVidaCacheEmSegundos));
        resgateCdb.Resgatar(cdb);
        await cacheService.RegistrarAsync(cacheKey, resgateCdb, ttl, cancellationToken);
        return resgateCdb;
    }

    public ICdb Investir(decimal valorInvestido, int prazoEmMeses, decimal? percentualCdi = default,
        decimal? percentualCdiPagoPeloBanco = default)
    {
        percentualCdi ??= configuration.GetValue<decimal>(ConfiguracaoAplicacao.PercentualPadraoCdi);
        percentualCdiPagoPeloBanco ??= configuration.GetValue<decimal>(ConfiguracaoAplicacao
            .PercentualPadraoCdiPagoPeloBanco);
        return new Domain.Cdb.Cdb(valorInvestido, prazoEmMeses, percentualCdi.GetValueOrDefault(),
            percentualCdiPagoPeloBanco.GetValueOrDefault());
    }
}