using B3.Investimentos.Api.Middlewares;
using FakeItEasy;
using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Serilog;
using Xunit;

namespace B3.Investimentos.UnitTests.Middleware;

public class GlobalExceptionHandlerMiddlewareUnitTests
{
    [Fact(DisplayName = "Deve tratar exceções não esperadas corretamente")]
    public async Task DeveTratarExcecoesNaoEsperadasCorretamenteAsync()
    {
        var mensagemExcecao = "Testando exceção não esperada";
        var logger = ConfigurarEObterLogger();
        var middleware = new GlobalExceptionHandlerMiddleware((contexto) =>
        {
             throw new Exception(mensagemExcecao);
        });
        var context = new DefaultHttpContext();

        await middleware.InvokeAsync(context);

        context.Response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        A.CallTo(logger).MustHaveHappenedOnceExactly();
    }

    [Fact(DisplayName = "Deve tratar falhas de validação corretamente")]
    public async Task DeveTratarFalhasDeValidacaoCorretamenteAsync()
    {
        var mensagemValidacao = "Testando falha de validação";
        var logger = ConfigurarEObterLogger();

        var middleware = new GlobalExceptionHandlerMiddleware((innerHttpContext) =>
        {
            throw new ValidationException(mensagemValidacao);
        });

        var context = new DefaultHttpContext();
        var responseBodyStream = new MemoryStream();
        context.Response.Body = responseBodyStream;

        await middleware.InvokeAsync(context);

        context.Response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        A.CallTo(logger).MustHaveHappenedOnceExactly();
    }

    private static ILogger ConfigurarEObterLogger()
    {
        var logger = A.Fake<ILogger>();
        Log.Logger = logger;

        return logger;
    }
}