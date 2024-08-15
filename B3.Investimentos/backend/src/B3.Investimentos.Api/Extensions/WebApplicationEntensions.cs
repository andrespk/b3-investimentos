using System.Diagnostics.CodeAnalysis;
using AstroCqrs;
using B3.Investimentos.Application.Commands.Cdb;

namespace B3.Investimentos.Api.Extensions;

[ExcludeFromCodeCoverage]
public static class WebApplicationEntensions
{
    public static WebApplication ConfigurarRotas(this WebApplication app)
    {
        app.UseHttpsRedirection();
        app.UseCors();

        app.MapPostHandler<ResgatarCdbCommand.Command, ResgatarCdbCommand.Response>("/cdb/resgatar")
            .WithName("ResgatarCDB")
            .WithOpenApi();

        return app;
    }

    public static WebApplication ConfigurarMiddlewares(this WebApplication app)
    {
        //TODO: ajustar depois
        //app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

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