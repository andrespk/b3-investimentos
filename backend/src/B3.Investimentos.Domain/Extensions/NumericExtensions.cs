namespace B3.Investimentos.Domain.Extensions;

public static class NumericExtensions
{
    public static decimal Truncar(this decimal valor, int precisao)
    {
        var fator = int.Parse($"1{new string('0', precisao)}");
        return Math.Truncate(fator * valor) / fator;
    }
}