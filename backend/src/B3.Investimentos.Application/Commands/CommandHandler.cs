using System.Diagnostics.CodeAnalysis;
using AstroCqrs;
using B3.Investimentos.Application.Contextos;
using Serilog;

namespace B3.Investimentos.Application.Commands;

[ExcludeFromCodeCoverage]
public abstract class CommandHandler<TCommand, TResponse> : Handler<TCommand, TResponse>
    where TCommand : IHandlerMessage<IHandlerResponse<TResponse>>
{
    protected readonly ILogger Logger;

    protected CommandHandler(IContexto contexto)
    {
        Logger = contexto.Logger;
    }
}

[ExcludeFromCodeCoverage]
public abstract class CommandHandler<TCommand> : Handler<TCommand> where TCommand : IHandlerMessage<IHandlerResponse>
{
    protected readonly ILogger Logger;

    protected CommandHandler(IContexto contexto)
    {
        Logger = contexto.Logger;
    }
}