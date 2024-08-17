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
    public decimal Aliquota { get; private set; }
    public decimal ValorBaseDeCalculo { get; private set; }
    public decimal ValorAPagar { get; private set; }

    public void Calcular(ICdb investimento)
    {
        var faixaDeTributacao = _tabelaRegressiva
            .FirstOrDefault(x =>
                (x.De >= investimento.PrazoEmMeses && x.Ate <= investimento.PrazoEmMeses) ||
                (x.De <= investimento.PrazoEmMeses && x.Ate >= investimento.PrazoEmMeses));

        if (faixaDeTributacao is null)
            throw new ValidationException(
                string.Format(MensagensDomain.PrazoEmMesesInvalido, investimento.PrazoEmMeses));

        Aliquota = faixaDeTributacao.Aliquota;
        ValorBaseDeCalculo = investimento.ValorRendimento;
        ValorAPagar = (ValorBaseDeCalculo * Aliquota / 100).Truncar(2);
    }
}