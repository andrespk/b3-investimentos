namespace B3.Investimentos.Domain.Cdb.Abstractions;

public interface IResgateCdb
{
    ICdb Investimento { get; }
    int PrazoEmMeses { get; }
    ITributacaoIrCdb TributacaoIr { get; }
    decimal ValorAResgatar { get; }
    void Resgatar(ICdb investimento, int prazoEmMeses);
}