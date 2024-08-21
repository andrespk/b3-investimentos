using AstroCqrs;
using B3.Investimentos.Application.Contextos.Cdb;
using B3.Investimentos.Application.Dto;
using FluentValidation;
using Mapster;

namespace B3.Investimentos.Application.Commands.Cdb;

public static class ResgatarCdbCommand
{
    public sealed record Command(
        decimal ValorInvestido,
        int PrazoEmMeses,
        decimal? PercentualCdi = default,
        decimal? PercentualCdiPagoPeloBanco = default) : ICommand<IHandlerResponse<Resultado<Response>>>;

    public record Response(decimal ValorBruto, decimal ValorLiquido);

    public sealed class CommandValidator : Validator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.ValorInvestido)
                .NotNull()
                .GreaterThan(0)
                .WithMessage(MensagensApplication.CdbValorInicialInvalido);

            RuleFor(x => x.PrazoEmMeses)
                .NotNull()
                .GreaterThan(1)
                .WithMessage(MensagensApplication.CdbPrazoEmMesesInvalido);

            RuleFor(x => x.PercentualCdi)
                .GreaterThan(0)
                .When(x => x.PercentualCdi is not null)
                .WithMessage(MensagensApplication.CdbPercentualCdiInvalido);

            RuleFor(x => x.PercentualCdiPagoPeloBanco)
                .GreaterThan(0)
                .When(x => x.PercentualCdiPagoPeloBanco is not null)
                .WithMessage(MensagensApplication.CdbPercentualCdiPagoPeloBancoInvalido);
        }

        public sealed class Handler(ICdbContexto contexto)
            : CommandHandler<Command, Resultado<Response>>(contexto)
        {
            public override async Task<IHandlerResponse<Resultado<Response>>> ExecuteAsync(Command comando,
                CancellationToken ct = default)
            {
                var cdb = contexto.CdbService.Investir(
                    comando.ValorInvestido, 
                    comando.PrazoEmMeses,
                    comando.PercentualCdi, 
                    comando.PercentualCdiPagoPeloBanco);
                var resgate = await contexto.CdbService.ResgatarAsync(cdb, ct);
                return Success(Resultado<Response>.BemSucedido(resgate.Adapt<Response>()));
            }
        }
    }
}