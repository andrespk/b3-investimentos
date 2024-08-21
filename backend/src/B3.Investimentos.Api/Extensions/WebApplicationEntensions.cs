using System.Diagnostics.CodeAnalysis;
using AstroCqrs;
using B3.Investimentos.Api.Middlewares;
using B3.Investimentos.Application.Commands.Cdb;
using B3.Investimentos.Application.Dto;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;

namespace B3.Investimentos.Api.Extensions;

[ExcludeFromCodeCoverage]
public static class WebApplicationEntensions
{
    public static WebApplication ConfigurarRotas(this WebApplication app)
    {
        app.UseHttpsRedirection();
        app.UseCors();
        app.MapPostHandler<ResgatarCdbCommand.Command, Resultado<ResgatarCdbCommand.Response>>("/cdb/resgatar")
            .WithName("b3-investimentos-resgatar-cdb")
            .WithOpenApi();
        Log.Logger.Information("Rotas configuradas: OK");
        return app;
    }

    public static WebApplication ConfigurarTratamentoDeErros(this WebApplication app)
    {
        app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
        Log.Logger.Information("Tratamento de erros configurado: OK");
        return app;
    }

    public static WebApplication ConfigurarDocumentacao(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        Log.Logger.Information("Documentacao configurada: OK");
        return app;
    }

    public static WebApplication ConfigurarHealthCheck(this WebApplication app)
    {
        app.MapHealthChecks("/health");
        app.UseHealthChecks("/health", new HealthCheckOptions
        {
            ResponseWriter = async (context, report) =>
            {
                var response = new
                {
                    Healthy = true,
                    DateTime.UtcNow
                };
                await context.Response.WriteAsJsonAsync(response);
            }
        });
        Log.Logger.Information("Health Check configurado: OK");
        return app;
    }
}