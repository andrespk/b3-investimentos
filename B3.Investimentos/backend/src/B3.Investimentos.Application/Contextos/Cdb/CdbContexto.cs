using System.Diagnostics.CodeAnalysis;
using B3.Investimentos.Domain.Cdb.Abstractions;
using B3.Investimentos.Infrastructure.Caching.Abstractions;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace B3.Investimentos.Application.Contextos.Cdb;

[ExcludeFromCodeCoverage]
public sealed class CdbContexto(
    ICdbService cdbService,
    ICacheService cacheService,
    ILogger logger,
    IConfiguration configuration) : ICdbContexto
{
    public ICdbService CdbService { get; } = cdbService;
    public ICacheService CacheService { get; } = cacheService;
    public IConfiguration Configuration { get; } = configuration;
    public ILogger Logger { get; } = logger;
}