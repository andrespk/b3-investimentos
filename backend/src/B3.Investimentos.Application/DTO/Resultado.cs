using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text.Json.Serialization;

namespace B3.Investimentos.Application.DTO;

[ExcludeFromCodeCoverage]
public struct Resultado<T>
{
    private const int StatusCodeFalhaPadrao = (int)HttpStatusCode.BadRequest;

    public Resultado()
    {
        Sucesso = false;
        Mensagem = null;
        StatusCode = StatusCodeFalhaPadrao;
    }

    public bool Sucesso { get; private set; }
    public string? Mensagem { get; private set; }
    public T? Dados { get; private set; }
    public IDictionary<string, string> Erros { get; private set; } = new Dictionary<string, string>();


    [JsonIgnore] public int StatusCode { get; private init; }

    public static Resultado<T> BemSucedido(T? dados = default, string? mensagem = default,
        int statusCode = (int)HttpStatusCode.OK)
    {
        return new Resultado<T>
        {
            Sucesso = true,
            Mensagem = mensagem,
            StatusCode = statusCode,
            Dados = dados
        };
    }

    public static Resultado<T> Falha(string mensagem, IDictionary<string, string>? erros = default,
        int statusCode = StatusCodeFalhaPadrao)
    {
        return new Resultado<T>
        {
            Sucesso = false,
            Mensagem = mensagem,
            StatusCode = statusCode,
            Erros = erros ?? new Dictionary<string, string>()
        };
    }

    public static Resultado<T> Falha(string mensagem, int statusCode = StatusCodeFalhaPadrao)
    {
        return new Resultado<T>
        {
            Sucesso = false,
            Mensagem = mensagem,
            StatusCode = statusCode
        };
    }
}

[ExcludeFromCodeCoverage]
public struct Resultado
{
    private const int StatusCodeFalhaPadrao = (int)HttpStatusCode.BadRequest;

    public Resultado()
    {
        Sucesso = false;
        Mensagem = null;
        StatusCode = StatusCodeFalhaPadrao;
    }

    public bool Sucesso { get; private set; }
    public string? Mensagem { get; private set; }
    public IDictionary<string, string> Erros { get; private set; } = new Dictionary<string, string>();


    [JsonIgnore] public int StatusCode { get; private init; }

    public static Resultado BemSucedido(string? mensagem = default, int statusCode = (int)HttpStatusCode.OK)
    {
        return new Resultado
        {
            Sucesso = true,
            Mensagem = mensagem,
            StatusCode = statusCode
        };
    }

    public static Resultado Falha(string mensagem, IDictionary<string, string>? erros = default,
        int statusCode = StatusCodeFalhaPadrao)
    {
        return new Resultado
        {
            Sucesso = false,
            Mensagem = mensagem,
            StatusCode = statusCode,
            Erros = erros ?? new Dictionary<string, string>()
        };
    }

    public static Resultado Falha(string mensagem, int statusCode = StatusCodeFalhaPadrao)
    {
        return new Resultado
        {
            Sucesso = false,
            Mensagem = mensagem,
            StatusCode = statusCode
        };
    }
}