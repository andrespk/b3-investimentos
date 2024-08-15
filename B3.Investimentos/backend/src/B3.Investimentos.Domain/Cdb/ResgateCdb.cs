using B3.Investimentos.Domain.Cdb.Abstractions;

namespace B3.Investimentos.Domain.Cdb;

public class ResgateCdb(ITributacaoIrCdb tributacaoIrCdb) : IResgateCdb
{
    private const int _zero = 0;
    public ICdb Investimento { get; private set; } = new Cdb(_zero, _zero, _zero);
    public int PrazoEmMeses { get; private set; }
    public ITributacaoIrCdb TributacaoIr { get; } = tributacaoIrCdb;
    public decimal ValorAResgatar { get; private set; }

    public void Resgatar(ICdb investimento, int prazoEmMeses)
    {
        PrazoEmMeses = prazoEmMeses;
        Investimento = investimento;
        Investimento.Corrigir(Investimento.ValorInicial, PrazoEmMeses);
        TributacaoIr.Calcular(Investimento, PrazoEmMeses);
        ValorAResgatar = Investimento.ValorAtual - TributacaoIr.ValorAPagar;
    }
}