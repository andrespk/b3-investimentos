using System.Diagnostics.CodeAnalysis;
using B3.Investimentos.Domain.Cdb.Abstractions;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace B3.Investimentos.Application.Contextos.Cdb;

[ExcludeFromCodeCoverage]
public sealed class CdbContexto(
    ICdbService cdbService,
    ILogger logger) : ICdbContexto
{
    public ICdbService CdbService { get; } = cdbService;
    public IConfiguration Configuration { get; }
    public ILogger Logger { get; } = logger;
}