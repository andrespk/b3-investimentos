using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text.Json.Serialization;

namespace B3.Investimentos.Application.DTO;

[ExcludeFromCodeCoverage]
public struct Resultado<T> : IResultado<T>
{
    private const int StatusCodeFalhaPadrao = (int)HttpStatusCode.BadRequest;

    public Resultado()
    {
        IsBemSucedido = false;
        Mensagem = null;
        StatusCode = StatusCodeFalhaPadrao;
    }

    public bool IsBemSucedido { get; private set; }
    public string? Mensagem { get; private set; }
    public T? Dados { get; private set; }
    public IDictionary<string, string> Erros { get; private set; } = new Dictionary<string, string>();


    [JsonIgnore] public int StatusCode { get; private init; }

    public static Resultado<T> Sucesso(T? dados = default, string? mensagem = default,
        int statusCode = (int)HttpStatusCode.OK)
    {
        return new Resultado<T>
        {
            IsBemSucedido = true,
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
            IsBemSucedido = false,
            Mensagem = mensagem,
            StatusCode = statusCode,
            Erros = erros ?? new Dictionary<string, string>()
        };
    }

    public static Resultado<T> Falha(string mensagem, int statusCode = StatusCodeFalhaPadrao)
    {
        return new Resultado<T>
        {
            IsBemSucedido = false,
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
        IsBemSucedido = false;
        Mensagem = null;
        StatusCode = StatusCodeFalhaPadrao;
    }

    public bool IsBemSucedido { get; private set; }
    public string? Mensagem { get; private set; }
    public IDictionary<string, string> Erros { get; private set; } = new Dictionary<string, string>();


    [JsonIgnore] public int StatusCode { get; private init; }

    public static Resultado Sucesso(string? mensagem = default, int statusCode = (int)HttpStatusCode.OK)
    {
        return new Resultado
        {
            IsBemSucedido = true,
            Mensagem = mensagem,
            StatusCode = statusCode
        };
    }

    public static Resultado Falha(string mensagem, IDictionary<string, string>? erros = default,
        int statusCode = StatusCodeFalhaPadrao)
    {
        return new Resultado
        {
            IsBemSucedido = false,
            Mensagem = mensagem,
            StatusCode = statusCode,
            Erros = erros ?? new Dictionary<string, string>()
        };
    }

    public static Resultado Falha(string mensagem, int statusCode = StatusCodeFalhaPadrao)
    {
        return new Resultado
        {
            IsBemSucedido = false,
            Mensagem = mensagem,
            StatusCode = statusCode
        };
    }
}