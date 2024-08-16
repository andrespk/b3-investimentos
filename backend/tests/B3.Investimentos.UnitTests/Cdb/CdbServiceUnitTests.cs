using System.Diagnostics.CodeAnalysis;
using B3.Investimentos.Application.Constants;
using B3.Investimentos.Domain.Cdb;
using B3.Investimentos.Domain.Cdb.Abstractions;
using B3.Investimentos.Domain.Extensions;
using Bogus;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace B3.Investimentos.UnitTests.Cdb;

[ExcludeFromCodeCoverage]
public class CdbServiceUnitTests
{
    private readonly IConfiguration _configuration = TestHelper.GetIConfigurationRoot();
    private readonly Faker _faker = new();

    [Fact(DisplayName = "Deve efetuar o resgate do CDB com sucesso")]
    public async Task DeveEfetuarResgateCdbComSucessoAsync()
    {
        var valorInicial = _faker.Random.Decimal(0, 10000).Truncar(2);
        var prazoEmMeses = _faker.Random.Number(2, 25);
        var percentualCi = _faker.Random.Decimal(0.1M, 0.9M);
        var percentualCiPagoPeloBanco = _faker.Random.Number(100, 110);
        var cdb = new Domain.Cdb.Cdb(valorInicial, percentualCi, percentualCiPagoPeloBanco);
        var resgateCdb = A.Fake<IResgateCdb>();
        var servico = new CdbService(resgateCdb);
        var resgate = await servico.ResgatarAsync(cdb, prazoEmMeses, new CancellationToken());

        A.CallTo(() => resgateCdb.Resgatar(cdb, prazoEmMeses)).MustHaveHappened();
        resgate.Should().NotBeNull();
        (resgate.Investimento.ValorInicial + resgate.Investimento.ValorDoRendimento - resgate.TributacaoIr.ValorAPagar)
            .Should().Be(resgate.ValorAResgatar);
    }

    [Fact(DisplayName = "Deve efetuar o resgate do CDB com sucesso com as configuracoes padrao")]
    public async Task DeveEfetuarResgateCdbComSucessoComConfiguracoesPadraoAsync()
    {
        var valorInicial = _faker.Random.Decimal(0, 10000).Truncar(2);
        var prazoEmMeses = _faker.Random.Number(2, 25);
        var percentualCi = _configuration.GetValue<decimal>(ConfiguracaoAplicacao.PercentualPadraoCdi);
        var percentualCiPagoPeloBanco =
            _configuration.GetValue<decimal>(ConfiguracaoAplicacao.PercentualPadraoCdiPagoPeloBanco);
        var cdb = new Domain.Cdb.Cdb(valorInicial, percentualCi, percentualCiPagoPeloBanco);
        var resgateCdb = A.Fake<IResgateCdb>();
        var servico = new CdbService(resgateCdb);
        var resgate = await servico.ResgatarAsync(cdb, prazoEmMeses, new CancellationToken());

        percentualCi.Should().BeGreaterThan(0);
        percentualCiPagoPeloBanco.Should().BeGreaterThan(0);
        A.CallTo(() => resgateCdb.Resgatar(cdb, prazoEmMeses)).MustHaveHappened();
        resgate.Should().NotBeNull();
        (resgate.Investimento.ValorInicial + resgate.Investimento.ValorDoRendimento - resgate.TributacaoIr.ValorAPagar)
            .Should().Be(resgate.ValorAResgatar);
    }
}