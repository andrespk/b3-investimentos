using System.Diagnostics.CodeAnalysis;
using B3.Investimentos.Domain.Extensions;
using Bogus;
using FluentAssertions;
using Xunit;

namespace B3.Investimentos.UnitTests.Cdb;

[ExcludeFromCodeCoverage]
public class CdbUnitTests
{
    private readonly Faker _faker = new();

    [Fact(DisplayName = "Deve efetuar corretamente a correção monetária")]
    public void DeveEfetuarCorrecaoMonetariaCorretamente()
    {
        var percentualCi = _faker.Random.Decimal(0.1M, 0.9M);
        var percentualCiPagoPeloBanco = _faker.Random.Number(100, 110);
        var prazoEmMesesClienteA = _faker.Random.Number(2, 25);
        var prazoEmMesesClienteB = _faker.Random.Number(2, 25);
        var cdbClienteA = new Domain.Cdb.Cdb(_faker.Random.Decimal(1, 99999.99M).Truncar(2), percentualCi,
            percentualCiPagoPeloBanco);
        var cdbClienteB = new Domain.Cdb.Cdb(100, percentualCi, percentualCiPagoPeloBanco);
        var cdbClienteC = new Domain.Cdb.Cdb(_faker.Random.Decimal(1, 99999.99M).Truncar(2), percentualCi,
            percentualCiPagoPeloBanco);

        cdbClienteA.Corrigir(prazoEmMesesClienteA);
        cdbClienteB.Corrigir(_faker.Random.Decimal(1, 99999.99M).Truncar(2), prazoEmMesesClienteB);
        cdbClienteC.DefinirPercentuaisCdi(_faker.Random.Decimal(0.1M, 0.9M), _faker.Random.Number(100, 110));

        cdbClienteA.ValorDoRendimento.Should().BeGreaterThan(0);
        cdbClienteA.PercentualDoRendimento.Should().BeGreaterThan(0);
        cdbClienteB.ValorDoRendimento.Should().BeGreaterThan(0);
        cdbClienteB.PercentualDoRendimento.Should().BeGreaterThan(0);
        cdbClienteC.PercentualCdi.Should().NotBe(percentualCi);
        cdbClienteC.PercentualCdiPagoPeloBanco.Should().NotBe(percentualCiPagoPeloBanco);
    }
}