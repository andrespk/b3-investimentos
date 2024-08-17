namespace B3.Investimentos.Domain.Cdb.Abstractions;

public interface IResgateCdb
{
    ITributacaoIrCdb TributacaoIr { get; }
    decimal ValorLiquido { get; }
    void Resgatar(ICdb investimento);
}