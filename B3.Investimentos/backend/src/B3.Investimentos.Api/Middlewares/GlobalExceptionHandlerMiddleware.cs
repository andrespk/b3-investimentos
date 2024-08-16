using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using B3.Investimentos.Application;
using B3.Investimentos.Application.DTO;
using B3.Investimentos.Infrastructure.Logging;
using FluentValidation;
using Serilog;

namespace B3.Investimentos.Api.Middlewares;

public class GlobalExceptionHandlerMiddleware(RequestDelegate next)
{
    private string? _mensagemLog = string.Empty;
    private bool _possuiFalhaDeValidacao;
    private bool _possuiErro;
    private Dictionary<string, string> _falhasDeValidacao = new();

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException e)
        {
            _possuiFalhaDeValidacao = true;
            _falhasDeValidacao = e.Errors
                .GroupBy(x => x.PropertyName)
                .ToDictionary(k => ResolverChaveDaValidacao(k.Key), values => values.First().ErrorMessage)!;
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            _mensagemLog = $"[ValidationException] {e.Message}";
            await TratarErrosEFalhasDeValidacaoAsync(context);
        }
        catch (Exception e)
        {
            if (e.GetType() == typeof(ValidationException))
            {
                return;
            }

            _possuiErro = true;
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            _mensagemLog = $"[{e.GetType().Name}] {FormatarExcecaoEmJson(e)}";
            await TratarErrosEFalhasDeValidacaoAsync(context);
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

    private string FormatarExcecaoEmJson(Exception exception)
    {
        var excecao = new
        {
            Origem = exception.Source,
            Tipo = exception.GetType().Name,
            Erro = exception.Message,
            TipoExcecaoInterna = exception.InnerException?.GetType().Name,
            ErroExcecaoInterna = exception.InnerException?.Message,
            Pilha = exception.StackTrace,
            Dados = exception.Data
        };

        return JsonSerializer.Serialize(excecao);
    }

    private string FormatarRequisicaoSumarizada(HttpContext contexto)
    {
        var metadados = new
        {
            Request = new
            {
                Host = contexto.Request.Host.Value,
                Path = contexto.Request.Path.Value,
                contexto.Request.Method,
                contexto.Request.Query,
                contexto.Request.Protocol,
                contexto.Request.IsHttps,
                contexto.Request.Headers,
                contexto.Request.Cookies
            },
            contexto.Response.StatusCode
        };

        return JsonSerializer.Serialize(metadados);
    }


    private async Task TratarErrosEFalhasDeValidacaoAsync(HttpContext contexto)
    {
        var opcoesSerializador = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter() },
            DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        if (_possuiFalhaDeValidacao)
        {
            Log.Warning("{0}", _mensagemLog);
            await contexto.Response.WriteAsJsonAsync(
                Resultado.Falha(MensagensApplication.FalhasDeValidacaoEncontradas, _falhasDeValidacao,
                    contexto.Response.StatusCode), opcoesSerializador);
        }

        if (_possuiErro)
        {
            var mensagemLogEnriquecida = $"{_mensagemLog}\n[Request] {FormatarRequisicaoSumarizada(contexto)}";
            Log.Error("{0}", mensagemLogEnriquecida);
            await contexto.Response.WriteAsJsonAsync(
                Resultado.Falha(
                    MensagensApplication.FalhaNoProcessamentoDaRequisicao,
                    default,
                    contexto.Response.StatusCode),
                opcoesSerializador);
        }

        _mensagemLog = string.Empty;
        _possuiErro = false;
        _possuiFalhaDeValidacao = false;
        _falhasDeValidacao = new();
    }
}