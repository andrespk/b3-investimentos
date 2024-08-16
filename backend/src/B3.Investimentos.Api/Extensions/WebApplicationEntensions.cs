using System.Diagnostics.CodeAnalysis;
using AstroCqrs;
using B3.Investimentos.Api.Middlewares;
using B3.Investimentos.Application.Commands.Cdb;
using Serilog;

namespace B3.Investimentos.Api.Extensions;

[ExcludeFromCodeCoverage]
public static class WebApplicationEntensions
{
    public static WebApplication ConfigurarRotas(this WebApplication app)
    {
        app.UseHttpsRedirection();
        app.UseCors();
        app.MapPostHandler<ResgatarCdbCommand.Command, ResgatarCdbCommand.Response>("/cdb/resgatar")
            .WithName("b3-investimentos-resgatar-cdb")
            .WithOpenApi();
        return app;
    }

    public static WebApplication ConfigurarTratamentoDeErros(this WebApplication app)
    {
        app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
        return app;
    }

    public static WebApplication ConfigurarDocumentacao(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        return app;
    }
}