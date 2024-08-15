using System.Diagnostics.CodeAnalysis;
using B3.Investimentos.Domain.Cdb;
using B3.Investimentos.Domain.Extensions;
using Bogus;
using FluentAssertions;
using Xunit;
using ValidationException = FluentValidation.ValidationException;

namespace B3.Investimentos.UnitTests.Cdb;

[ExcludeFromCodeCoverage]
public class ResgateCdbUnitTests
{
    private readonly Faker _faker = new();

    [Fact(DisplayName = "Deve a operação corretamente")]
    public void DeveEfetuarAOperacaoCorretamente()
    {
        var prazoEmMeses = _faker.Random.Number(2, 25);
        var percentualCi = _faker.Random.Decimal(0.1M, 0.9M);
        var percentualCiPagoPeloBanco = _faker.Random.Number(100, 110);
        var aliquotaIr = _faker.Random.Decimal(1, 27.5M);

        var cdbClienteA = new Domain.Cdb.Cdb(_faker.Random.Decimal(0, 10000).Truncar(2), percentualCi,
            percentualCiPagoPeloBanco);
        cdbClienteA.Corrigir(prazoEmMeses);
        var resgateCdbClienteA =
            new ResgateCdb(new TributacaoIrCdb(cdbClienteA.ValorDoRendimento, prazoEmMeses, aliquotaIr));
        resgateCdbClienteA.Resgatar(cdbClienteA, prazoEmMeses);

        var cdbClienteB = new Domain.Cdb.Cdb(_faker.Random.Decimal(0, 10000).Truncar(2), percentualCi,
            percentualCiPagoPeloBanco);
        cdbClienteB.Corrigir(prazoEmMeses);
        var resgateCdbClienteB = new ResgateCdb(new TributacaoIrCdb());
        resgateCdbClienteB.Resgatar(cdbClienteA, prazoEmMeses);

        (resgateCdbClienteA.Investimento.ValorInicial + resgateCdbClienteA.Investimento.ValorDoRendimento -
         resgateCdbClienteA.TributacaoIr.ValorAPagar).Should().Be(resgateCdbClienteA.ValorAResgatar);

        (resgateCdbClienteB.Investimento.ValorInicial + resgateCdbClienteB.Investimento.ValorDoRendimento -
         resgateCdbClienteB.TributacaoIr.ValorAPagar).Should().Be(resgateCdbClienteB.ValorAResgatar);
    }

    [Fact(DisplayName = "Deve falhar na operação com prazo inválido.")]
    public void DeveFalharAOperacaoComPrazoInvalido()
    {
        var prazoEmMeses = 0;
        var percentualCi = _faker.Random.Decimal(0.1M, 0.9M);
        var percentualCiPagoPeloBanco = _faker.Random.Number(100, 110);
        var aliquotaIr = _faker.Random.Decimal(1, 27.5M);

        try
        {
            var cdb = new Domain.Cdb.Cdb(_faker.Random.Decimal(0, 10000).Truncar(2), percentualCi,
                percentualCiPagoPeloBanco);
            cdb.Corrigir(prazoEmMeses);
            var resgateCdb = new ResgateCdb(new TributacaoIrCdb(cdb.ValorDoRendimento, prazoEmMeses, aliquotaIr));
            resgateCdb.Resgatar(cdb, prazoEmMeses);
        }
        catch (ValidationException e)
        {
            e.Message.Contains("o prazo em meses informado é inválido", StringComparison.InvariantCultureIgnoreCase)
                .Should().BeTrue();
        }
    }
}