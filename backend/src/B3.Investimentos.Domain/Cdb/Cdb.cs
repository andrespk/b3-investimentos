using B3.Investimentos.Domain.Cdb.Abstractions;
using B3.Investimentos.Domain.Extensions;

namespace B3.Investimentos.Domain.Cdb;

public class Cdb : ICdb
{
    public int PrazoEmMeses { get; }
    public decimal ValorInvestido { get; }
    public decimal ValorRetorno { get; private set; }
    public decimal PercentualCdi { get; init; }
    public decimal PercentualCdiPagoPeloBanco { get; init; }
    public decimal PercentualRendimento { get; private set; }
    public decimal ValorRendimento { get; private set; }

    public Cdb(decimal valorInvestido, int prazoEmMeses, decimal percentualCdi, decimal percentualCdiPagoPeloBanco)
    {
        ValorInvestido = valorInvestido;
        PrazoEmMeses = prazoEmMeses;
        PercentualCdi = percentualCdi;
        PercentualCdiPagoPeloBanco = percentualCdiPagoPeloBanco;
        CalcularRendimento();
    }

    private void CalcularRendimento()
    {
        PercentualRendimento = 0;
        ValorRetorno = ValorInvestido;

        var correcao = 1 + PercentualCdi / 100 * PercentualCdiPagoPeloBanco / 100;

        for (var mes = 1; mes <= PrazoEmMeses; mes++)
        {
            PercentualRendimento += ((correcao - 1) * 100).Truncar(2);
            ValorRetorno = (ValorRetorno * correcao).Truncar(2);
            ValorRendimento = ValorRetorno - ValorInvestido;
        }
    }
}