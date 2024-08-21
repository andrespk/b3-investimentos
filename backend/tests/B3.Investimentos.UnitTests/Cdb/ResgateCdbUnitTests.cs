using B3.Investimentos.Domain.Cdb;
using B3.Investimentos.Domain.Cdb.Abstractions;
using B3.Investimentos.Domain.Extensions;
using Bogus;
using FakeItEasy;
using FluentAssertions;
using ValidationException = FluentValidation.ValidationException;

namespace B3.Investimentos.UnitTests.Cdb;

public class ResgateCdbUnitTests
{
    private readonly Faker _faker = new();

    [Fact(DisplayName = "Deve a operação corretamente")]
    public void DeveEfetuarAOperacaoCorretamente()
    {
        var prazoEmMeses = _faker.Random.Number(2, 25);
        var percentualCi = _faker.Random.Decimal(0.1M, 0.9M).Truncar(2);
        var percentualCiPagoPeloBanco = _faker.Random.Number(100, 110);
        var valorInvestidoClienteA = _faker.Random.Decimal(1, 10000).Truncar(2);
        var cdbClienteA = new Domain.Cdb.Cdb(valorInvestidoClienteA, prazoEmMeses, percentualCi, percentualCiPagoPeloBanco);
        var resgateCdbClienteA = new ResgateCdb( new TributacaoIrCdb());
        resgateCdbClienteA.Resgatar(cdbClienteA);
        var valorInvestidoClienteB = _faker.Random.Decimal(1, 10000).Truncar(2);
        var cdbClienteB = new Domain.Cdb.Cdb(valorInvestidoClienteB, prazoEmMeses,percentualCi, percentualCiPagoPeloBanco);
        var resgateCdbClienteB = new ResgateCdb( new TributacaoIrCdb());
        resgateCdbClienteB.Resgatar(cdbClienteB);
        
        resgateCdbClienteA.TributacaoIr.Aliquota.Should().BeGreaterThan(0);
        resgateCdbClienteB.TributacaoIr.Aliquota.Should().BeGreaterThan(0);
        resgateCdbClienteA.TributacaoIr.ValorAPagar.Should().BeGreaterThan(0);
        resgateCdbClienteB.TributacaoIr.ValorAPagar.Should().BeGreaterThan(0);
        resgateCdbClienteA.TributacaoIr.ValorBaseDeCalculo.Should().Be(cdbClienteA.ValorRendimento);
        resgateCdbClienteB.TributacaoIr.ValorBaseDeCalculo.Should().Be(cdbClienteB.ValorRendimento);
        (cdbClienteA.ValorInvestido + cdbClienteA.ValorRendimento -
         resgateCdbClienteA.TributacaoIr.ValorAPagar).Should().Be(resgateCdbClienteA.ValorLiquido);
        (cdbClienteB.ValorInvestido + cdbClienteB.ValorRendimento -
         resgateCdbClienteB.TributacaoIr.ValorAPagar).Should().Be(resgateCdbClienteB.ValorLiquido);
    }

    [Fact(DisplayName = "Deve falhar na operação com prazo inválido.")]
    public void DeveFalharAOperacaoComPrazoInvalido()
    {
        var prazoEmMeses = 0;
        var valorInvestido = _faker.Random.Decimal(1, 10000).Truncar(2);
        var percentualCi = _faker.Random.Decimal(0.1M, 0.9M);
        var percentualCiPagoPeloBanco = _faker.Random.Number(100, 110);
        Exception exception = null;

        try
        {
            var cdb = new Domain.Cdb.Cdb(valorInvestido, prazoEmMeses, percentualCi, percentualCiPagoPeloBanco);
            var resgateCdb = new ResgateCdb(new TributacaoIrCdb());
            resgateCdb.Resgatar(cdb);
        }
        catch (ValidationException e)
        {
            exception = e;
            e.Message.Contains("o prazo em meses informado é inválido", StringComparison.InvariantCultureIgnoreCase)
                .Should().BeTrue();
        }

        exception.Should().NotBeNull();
    }
}