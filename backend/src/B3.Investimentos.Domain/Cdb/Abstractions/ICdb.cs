namespace B3.Investimentos.Domain.Cdb.Abstractions;

public interface ICdb
{
    decimal ValorInvestido { get; }
    decimal ValorRetorno { get; }
    int PrazoEmMeses { get; }
    decimal PercentualCdi { get; }
     decimal PercentualCdiPagoPeloBanco { get; }
    decimal PercentualRendimento { get; }
    decimal ValorRendimento { get; }
}