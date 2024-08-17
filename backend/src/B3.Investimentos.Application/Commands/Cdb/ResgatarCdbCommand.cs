using AstroCqrs;
using B3.Investimentos.Application.Constants;
using B3.Investimentos.Application.Contextos.Cdb;
using B3.Investimentos.Application.DTO;
using B3.Investimentos.Domain.Cdb.Abstractions;
using B3.Investimentos.Infrastructure.Caching.Constants;
using FluentValidation;
using Microsoft.Extensions.Configuration;

namespace B3.Investimentos.Application.Commands.Cdb;

public static class ResgatarCdbCommand
{
    public sealed record Command(
        decimal ValorInicial,
        int PrazoEmMeses,
        decimal? PercentualCdi = default,
        decimal? PercentualCdiPagoPeloBanco = default) : ICommand<IHandlerResponse<Response>>;

    public sealed record Response(Resultado<IResgateCdb> Resposta);

    public sealed class CommandValidator : Validator<Command>
    {
        public CommandValidator()
        {
            RuleFor(x => x.ValorInicial)
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

        public sealed class Handler(ICdbContexto contexto) : CommandHandler<Command, Response>(contexto)
        {
            public override async Task<IHandlerResponse<Response>> ExecuteAsync(Command comando,
                CancellationToken ct = default)
            {
                var cacheKey = contexto.CacheService.GerarChave(comando);
                var resgate = await contexto.CacheService.ObterAsync<IResgateCdb>(cacheKey, ct);

                if (resgate is not null) return Success(new Response(Resultado<IResgateCdb>.Sucesso(resgate)));

                var percentualCdi =
                    comando.PercentualCdi ??
                    contexto.Configuration.GetValue<decimal>(ConfiguracaoAplicacao.PercentualPadraoCdi);
                var percentualCdiPagoPeloBanco = comando.PercentualCdiPagoPeloBanco ??
                                                 contexto.Configuration.GetValue<decimal>(ConfiguracaoAplicacao
                                                     .PercentualPadraoCdiPagoPeloBanco);
                var cdb = new Domain.Cdb.Cdb(comando.ValorInicial, comando.PrazoEmMeses, percentualCdi,
                    percentualCdiPagoPeloBanco);
                var ttl = TimeSpan.FromSeconds(contexto.Configuration.GetValue<int>(
                    ConfiguracaoInfraestrutura.TempoDeVidaCacheEmSegundos));
                resgate = await contexto.CdbService.ResgatarAsync(cdb, ct);
                await contexto.CacheService.RegistrarAsync(cacheKey, resgate, ttl, ct);

                return Success(new Response(Resultado<IResgateCdb>.Sucesso(resgate)));
            }
        }
    }
}