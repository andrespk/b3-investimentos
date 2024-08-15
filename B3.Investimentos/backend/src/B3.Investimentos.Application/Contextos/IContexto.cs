using Microsoft.Extensions.Configuration;
using Serilog;

namespace B3.Investimentos.Application.Contextos;

public interface IContexto
{
    IConfiguration Configuration { get; }
    ILogger Logger { get; }
}