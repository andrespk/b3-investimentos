using Serilog;

namespace B3.Investimentos.Application.Contextos;

public interface IContexto
{
    ILogger Logger { get; }
}