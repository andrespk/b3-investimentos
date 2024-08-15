using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using B3.Investimentos.Application;
using B3.Investimentos.Application.DTO;
using B3.Investimentos.Infrastructure.Logging;
using FluentValidation;

namespace B3.Investimentos.Api.Middlewares;

public class GlobalExceptionHandlerMiddleware(ILogService logger)
{
    public async Task InvokeAsync(HttpContext contexto, RequestDelegate next)
    {
        try
        {
            await next(contexto);
        }
        catch (Exception excecao)
        {
            var opcoesSerializador = new JsonSerializerOptions
            {
                Converters = { new JsonStringEnumConverter() },
                DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            if (excecao.GetType() == typeof(ValidationException))
            {
                var failures = (excecao as ValidationException)?.Errors
                    .GroupBy(x => x.PropertyName)
                    .ToDictionary(k => ResolverChaveDaValidacao(k.Key), values => values.First().ErrorMessage);
                contexto.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                logger.LogWarning(excecao.Message, JsonSerializer.Serialize(ObterMetadados(contexto)));
                await contexto.Response.WriteAsJsonAsync(
                    Resultado.Falha(MensagensApplication.FalhasDeValidacaoEncontradas, failures,
                        contexto.Response.StatusCode), opcoesSerializador);
            }
            else
            {
                var origem = excecao.GetType().Name;
                contexto.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                logger.LogError(origem, excecao.Message, ObterMetadados(contexto));
                await contexto.Response.WriteAsJsonAsync(
                    Resultado.Falha(
                        MensagensApplication.FalhaNoProcessamentoDaRequisicao,
                        default,
                        contexto.Response.StatusCode),
                    opcoesSerializador);
            }
        }
    }

    private static string ResolverChaveDaValidacao(string chave)
    {
        var regex = new Regex(@"\[(\d+)\]");
        var caminhoConvertido = regex.Replace(chave, ".$1");

        var partes = caminhoConvertido.Split('.');
        for (var i = 1; i < partes.Length; i++) partes[i] = char.ToLower(partes[i][0]) + partes[i][1..];

        return string.Join(".", partes);
    }

    private MetadadosRequisicao ObterMetadados(HttpContext httpContext)
    {
        return new MetadadosRequisicao(httpContext.Request, httpContext.Response.StatusCode);
    }
}