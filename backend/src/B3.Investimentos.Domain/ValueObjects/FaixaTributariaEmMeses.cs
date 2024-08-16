using System.Diagnostics.CodeAnalysis;

namespace B3.Investimentos.Domain.ValueObjects;

[ExcludeFromCodeCoverage]
public record FaixaTributariaEmMeses(int De, int Ate, decimal Aliquota);