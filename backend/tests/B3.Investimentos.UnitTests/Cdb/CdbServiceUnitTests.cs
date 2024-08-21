using B3.Investimentos.Application.Constants;
using B3.Investimentos.Application.Services;
using B3.Investimentos.Domain.Cdb;
using B3.Investimentos.Domain.Extensions;
using B3.Investimentos.Infrastructure.Caching;
using B3.Investimentos.Infrastructure.Caching.Abstractions;
using Bogus;
using FluentAssertions;
using Microsoft.Extensions.Configuration;

namespace B3.Investimentos.UnitTests.Cdb;

public class CdbServiceUnitTests
{
    private readonly IConfiguration _configuration = UnitTestHelper.GetIConfigurationRoot();
    private readonly ICacheService _cacheService;
    private readonly Faker _faker = new();

    public CdbServiceUnitTests()
    {
        var cacheProvider = UnitTestHelper.ObterCacheProvider();
        _cacheService = new CacheService(cacheProvider);
    }
    [Fact(DisplayName = "Deve efetuar o resgate do CDB com sucesso")]
    public async Task DeveEfetuarResgateCdbComSucessoAsync()
    {
        var valorInicial = _faker.Random.Decimal(1, 10000).Truncar(2);
        var prazoEmMeses = _faker.Random.Number(2, 25);
        var percentualCi = _faker.Random.Decimal(0.1M, 0.9M).Truncar(2);
        var percentualCiPagoPeloBanco = _faker.Random.Number(100, 110);
        var cdb = new Domain.Cdb.Cdb(valorInicial, prazoEmMeses, percentualCi, percentualCiPagoPeloBanco);
        var resgateCdb = new ResgateCdb(new TributacaoIrCdb());
        var servico = new CdbService(_cacheService, _configuration, resgateCdb);
        var resgate = await servico.ResgatarAsync(cdb,new CancellationToken());
        
        resgate.Should().NotBeNull();
        (cdb.ValorInvestido + cdb.ValorRendimento - resgate.TributacaoIr.ValorAPagar)
            .Should().Be(resgate.ValorLiquido);
    }

    [Fact(DisplayName = "Deve efetuar o resgate do CDB com sucesso com as configuracoes padrao")]
    public async Task DeveEfetuarResgateCdbComSucessoComConfiguracoesPadraoAsync()
    {
        var valorInicial = _faker.Random.Decimal(1, 10000).Truncar(2);
        var prazoEmMeses = _faker.Random.Number(2, 25);
        var percentualCi = _configuration.GetValue<decimal>(ConfiguracaoAplicacao.PercentualPadraoCdi);
        var percentualCiPagoPeloBanco =
            _configuration.GetValue<decimal>(ConfiguracaoAplicacao.PercentualPadraoCdiPagoPeloBanco);
        var cdb = new Domain.Cdb.Cdb(valorInicial, prazoEmMeses, percentualCi, percentualCiPagoPeloBanco);
        var resgateCdb = new ResgateCdb(new TributacaoIrCdb());
        var servico = new CdbService(_cacheService, _configuration, resgateCdb);
        var resgate = await servico.ResgatarAsync(cdb, new CancellationToken());

        percentualCi.Should().BeGreaterThan(0);
        percentualCiPagoPeloBanco.Should().BeGreaterThan(0);
        resgate.Should().NotBeNull();
        (cdb.ValorInvestido + cdb.ValorRendimento - resgate.TributacaoIr.ValorAPagar)
            .Should().Be(resgate.ValorLiquido);
    }
}