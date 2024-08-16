namespace B3.Investimentos.Application.DTO;

public interface IResultado<T>
{
    bool IsBemSucedido { get; }
    string? Mensagem { get; }
    int StatusCode { get; }

    T Dados { get; }
    IDictionary<string, string> Erros { get; }
}