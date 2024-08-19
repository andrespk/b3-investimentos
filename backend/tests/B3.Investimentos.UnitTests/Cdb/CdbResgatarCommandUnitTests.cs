using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using B3.Investimentos.Application;
using B3.Investimentos.Application.Commands.Cdb;
using B3.Investimentos.Application.Constants;
using B3.Investimentos.Application.Contextos.Cdb;
using B3.Investimentos.Domain.Extensions;
using Bogus;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace B3.Investimentos.UnitTests.Cdb;

public class CdbResgatarCommandUnitTests
{
    private readonly IConfiguration _configuration;
    private readonly Faker _faker = new();

    public CdbResgatarCommandUnitTests()
    {
        _configuration = UnitTestHelper.GetIConfigurationRoot();
    }

    [Fact(DisplayName = "Deve efetuar o resgate do CDB com sucesso")]
    public async Task DeveEfetuarOResgateComSucessoAsync()
    {
        var valorInicial = _faker.Random.Decimal(0, 10000).Truncar(2);
        var prazoEmMeses = _faker.Random.Number(2, 25);
        var percentualCi = _configuration.GetValue<decimal>(ConfiguracaoAplicacao.PercentualPadraoCdi);
        var percentualCiPagoPeloBanco =
            _configuration.GetValue<decimal>(ConfiguracaoAplicacao.PercentualPadraoCdiPagoPeloBanco);
        var comando =
            new ResgatarCdbCommand.Command(valorInicial, prazoEmMeses, percentualCi, percentualCiPagoPeloBanco);

        var resultadoValidacao = await new ResgatarCdbCommand.CommandValidator().ValidateAsync(comando);
        var resposta = await ObterHandlerComando().ExecuteAsync(comando);

        resultadoValidacao.IsValid.Should().BeTrue();
        resposta.IsSuccess.Should().BeTrue();
        resposta.Payload.Sucesso.Should().BeTrue();
        resposta.Payload.Dados.Should().NotBeNull();
        resposta.Payload.Erros.Count.Should().Be(0);
    }

    [Fact(DisplayName = "Deve falhar na validação do Resgate do CDB para prazos inválidos")]
    public async Task DeveFalharNaValidacaoDoComandoParaPrazosInvalidosAsync()
    {
        var valorInicial = _faker.Random.Decimal(0, 10000).Truncar(2);
        var prazoEmMeses = _faker.Random.Number(-1, 1);
        var percentualCi = _configuration.GetValue<decimal>(ConfiguracaoAplicacao.PercentualPadraoCdi);
        var percentualCiPagoPeloBanco =
            _configuration.GetValue<decimal>(ConfiguracaoAplicacao.PercentualPadraoCdiPagoPeloBanco);
        var comando =
            new ResgatarCdbCommand.Command(valorInicial, prazoEmMeses, percentualCi, percentualCiPagoPeloBanco);


        var resultadoValidacao = await new ResgatarCdbCommand.CommandValidator().ValidateAsync(comando);

        resultadoValidacao.IsValid.Should().BeFalse();
        resultadoValidacao.Errors.Count.Should().Be(1);
        resultadoValidacao.Errors.FirstOrDefault()?.ErrorMessage.Equals(MensagensApplication.CdbPrazoEmMesesInvalido)
            .Should()
            .BeTrue();
    }

    [Fact(DisplayName = "Deve falhar na validação do Resgate do CDB para percentual inválido de CDI")]
    public async Task DeveFalharNaValidacaoDoComandoParaPercentualCdiInvalidoAsync()
    {
        var valorInicial = _faker.Random.Decimal(0, 10000).Truncar(2);
        var prazoEmMeses = _faker.Random.Number(2, 25);
        var percentualCi = 0;
        int? percentualCiPagoPeloBanco = null;
        var comando =
            new ResgatarCdbCommand.Command(valorInicial, prazoEmMeses, percentualCi, percentualCiPagoPeloBanco);


        var resultadoValidacao = await new ResgatarCdbCommand.CommandValidator().ValidateAsync(comando);

        resultadoValidacao.IsValid.Should().BeFalse();
        resultadoValidacao.Errors.Count.Should().Be(1);
        resultadoValidacao.Errors.FirstOrDefault()?.ErrorMessage.Equals(MensagensApplication.CdbPercentualCdiInvalido)
            .Should()
            .BeTrue();
    }

    [Fact(DisplayName = "Deve falhar na validação do Resgate do CDB para percentual inválido de CDI pago pelo banco")]
    public async Task DeveFalharNaValidacaoDoComandoParaPercentualCdiPagoPeloBancoInvalidoAsync()
    {
        var valorInicial = _faker.Random.Decimal(0, 10000).Truncar(2);
        var prazoEmMeses = _faker.Random.Number(2, 25);
        var percentualCi = _faker.Random.Decimal(0.1M, 0.9M);
        int? percentualCiPagoPeloBanco = 0;
        var comando =
            new ResgatarCdbCommand.Command(valorInicial, prazoEmMeses, percentualCi, percentualCiPagoPeloBanco);


        var resultadoValidacao = await new ResgatarCdbCommand.CommandValidator().ValidateAsync(comando);

        resultadoValidacao.IsValid.Should().BeFalse();
        resultadoValidacao.Errors.Count.Should().Be(1);
        resultadoValidacao.Errors.FirstOrDefault()?.ErrorMessage
            .Equals(MensagensApplication.CdbPercentualCdiPagoPeloBancoInvalido).Should()
            .BeTrue();
    }

    [Fact(DisplayName = "Deve obter o resgate pelo cache")]
    public async Task DeveObterOResgatePeloCacheAsync()
    {
        var valorInicial = _faker.Random.Decimal(0, 10000).Truncar(2);
        var prazoEmMeses = _faker.Random.Number(2, 25);
        var percentualCi = _configuration.GetValue<decimal>(ConfiguracaoAplicacao.PercentualPadraoCdi);
        var percentualCiPagoPeloBanco =
            _configuration.GetValue<decimal>(ConfiguracaoAplicacao.PercentualPadraoCdiPagoPeloBanco);
        var cancellationToken = new CancellationToken();

        var comando =
            new ResgatarCdbCommand.Command(valorInicial, prazoEmMeses, percentualCi, percentualCiPagoPeloBanco);

        var stopWatchResgateA = new Stopwatch();
        stopWatchResgateA.Start();
        var respostaA = await ObterHandlerComando().ExecuteAsync(comando, cancellationToken);
        stopWatchResgateA.Stop();

        var stopWatchResgateB = new Stopwatch();
        stopWatchResgateB.Start();
        var respostaB = await ObterHandlerComando().ExecuteAsync(comando, cancellationToken);
        stopWatchResgateB.Stop();

        respostaA.Payload.Dados.Should().NotBeNull();
        respostaB.Payload.Dados.Should().NotBeNull();
        stopWatchResgateA.ElapsedMilliseconds.Should().BeGreaterThan(stopWatchResgateB.ElapsedMilliseconds);
    }

    private static ResgatarCdbCommand.CommandValidator.Handler ObterHandlerComando()
    {
        var contextoCdb = A.Fake<ICdbContexto>();
        return new ResgatarCdbCommand.CommandValidator.Handler(contextoCdb);
    }
}