using B3.Investimentos.Domain.Cdb.Abstractions;
using B3.Investimentos.Domain.Extensions;
using B3.Investimentos.Domain.ValueObjects;
using FluentValidation;

namespace B3.Investimentos.Domain.Cdb;

public class TributacaoIrCdb : ITributacaoIrCdb
{
    private readonly IList<FaixaTributariaEmMeses> _tabelaRegressiva = new List<FaixaTributariaEmMeses>
    {
        new(1, 6, (decimal)22.5),
        new(7, 12, 20),
        new(13, 24, (decimal)17.5),
        new(25, int.MaxValue, 15)
    };

    public TributacaoIrCdb()
    {
    }

    public TributacaoIrCdb(decimal valorRendimento, int prazoEmMeses, decimal aliquota)
    {
        Aliquota = aliquota;
        Calcular(valorRendimento, prazoEmMeses);
    }

    public decimal ValorATributar { get; private set; }
    public decimal Aliquota { get; private set; }
    public decimal ValorAPagar { get; private set; }

    public void Calcular(ICdb investimento, int prazoEmMeses)
    {
        var faixaDeTributacao = _tabelaRegressiva
            .FirstOrDefault(x =>
                (x.De >= prazoEmMeses && x.Ate <= prazoEmMeses) || (x.De <= prazoEmMeses && x.Ate >= prazoEmMeses));

        if (faixaDeTributacao is null)
            throw new ValidationException(string.Format(MensagensDomain.PrazoEmMesesInvalido, prazoEmMeses));

        Calcular(investimento.ValorDoRendimento, faixaDeTributacao.Aliquota);
    }

    private void Calcular(decimal valorDoRendimento, decimal aliquota)
    {
        Aliquota = aliquota;
        ValorATributar = valorDoRendimento;
        ValorAPagar = (ValorATributar * Aliquota / 100).Truncar(2);
    }
}