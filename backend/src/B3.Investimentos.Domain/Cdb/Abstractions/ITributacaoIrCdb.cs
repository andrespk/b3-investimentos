namespace B3.Investimentos.Domain.Cdb.Abstractions;

public interface ITributacaoIrCdb
{
    decimal Aliquota { get; }
    decimal ValorAPagar { get; }
    decimal ValorBaseDeCalculo { get; }

    void Calcular(ICdb investimento);
}