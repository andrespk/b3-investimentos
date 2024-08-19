namespace B3.Investimentos.Domain.Cdb.Abstractions;

public interface IResgateCdb
{
    ITributacaoIrCdb TributacaoIr { get; }
    decimal ValorLiquido { get; }
    decimal ValorBruto { get; }
    void Resgatar(ICdb investimento);
}