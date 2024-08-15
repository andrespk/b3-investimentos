namespace B3.Investimentos.Domain.Cdb.Abstractions;

public interface ICdb
{
    decimal ValorInicial { get; }
    decimal ValorAtual { get; }
    decimal PercentualDoRendimento { get; }
    decimal ValorDoRendimento { get; }
    void Corrigir(decimal valorInicial, int prazoEmMeses);
    void DefinirPercentuaisCdi(decimal percentualCdi, decimal percentualCdiPagoPeloBanco);
}