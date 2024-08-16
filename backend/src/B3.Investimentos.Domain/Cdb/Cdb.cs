using B3.Investimentos.Domain.Cdb.Abstractions;
using B3.Investimentos.Domain.Extensions;

namespace B3.Investimentos.Domain.Cdb;

public class Cdb : ICdb
{
    public Cdb(decimal valorInicial, decimal percentualCdi, decimal percentualCdiPagoPeloBanco)
    {
        ConfigurarValorInicial(valorInicial);
        DefinirPercentuaisCdi(percentualCdi, percentualCdiPagoPeloBanco);
    }

    public decimal PercentualCdi { get; private set; }
    public decimal PercentualCdiPagoPeloBanco { get; private set; }
    public decimal ValorInicial { get; private set; }
    public decimal ValorAtual { get; private set; }
    public decimal PercentualDoRendimento { get; private set; }

    public decimal ValorDoRendimento { get; private set; }

    public void DefinirPercentuaisCdi(decimal percentualCdi, decimal percentualCdiPagoPeloBanco)
    {
        PercentualCdi = percentualCdi;
        PercentualCdiPagoPeloBanco = percentualCdiPagoPeloBanco;
    }

    public void Corrigir(decimal valorInicial, int prazoEmMeses)
    {
        ConfigurarValorInicial(valorInicial);
        Corrigir(prazoEmMeses);
    }

    public void Corrigir(int prazoEmMeses)
    {
        PercentualDoRendimento = 0;

        var correcao = 1 + PercentualCdi / 100 * PercentualCdiPagoPeloBanco / 100;

        for (var mes = 1; mes <= prazoEmMeses; mes++)
        {
            PercentualDoRendimento += ((correcao - 1) * 100).Truncar(2);
            ValorAtual = (ValorAtual * correcao).Truncar(2);
            ValorDoRendimento = ValorAtual - ValorInicial;
        }
    }

    private void ConfigurarValorInicial(decimal valorInicial)
    {
        ValorInicial = valorInicial;
        ValorAtual = ValorInicial;
    }
}