using B3.Investimentos.Domain.Cdb.Abstractions;

namespace B3.Investimentos.Domain.Cdb;

public class ResgateCdb(ITributacaoIrCdb tributacaoIrCdb) : IResgateCdb
{
    public ITributacaoIrCdb TributacaoIr { get; } = tributacaoIrCdb;
    public decimal ValorLiquido { get; private set; }
    public decimal ValorBruto { get; private set; }
    

    public void Resgatar(ICdb investimento)
    {
        TributacaoIr.Calcular(investimento);
        ValorBruto = investimento.ValorRetorno;
        ValorLiquido = ValorBruto - TributacaoIr.ValorAPagar;
    }
}