using System.Diagnostics.CodeAnalysis;
using AstroCqrs;
using B3.Investimentos.Application.Contextos.Cdb;
using B3.Investimentos.Domain.Cdb;
using B3.Investimentos.Domain.Cdb.Abstractions;
using B3.Investimentos.Infrastructure.Caching;
using B3.Investimentos.Infrastructure.Caching.Abstractions;
using Serilog;
using ILogger = Serilog.ILogger;

namespace B3.Investimentos.Api.Extensions;

[ExcludeFromCodeCoverage]
public static class WebApplicatioBuilderExtensions
{
    public static WebApplicationBuilder AdicionarInfraestrutura(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog();
        builder.Services.AddAstroCqrs();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddCors();
        builder.Services.AddMemoryCache();
        builder.Services.AddSingleton<ICacheProvider, MemoryCacheProvider>();
        builder.Services.AddScoped<ICacheService, CacheServiceUnit>();
        builder.Services.AddTransient<ILogger>(provider => Log.Logger);

        return builder;
    }

    public static WebApplicationBuilder AdicionarDependenciasDeDominio(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ITributacaoIrCdb, TributacaoIrCdb>();
        builder.Services.AddScoped<IResgateCdb, ResgateCdb>();
        builder.Services.AddScoped<ICdbService, CdbService>();
        builder.Services.AddScoped<ICdbContexto, CdbContexto>();

        return builder;
    }
}