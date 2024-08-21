namespace B3.Investimentos.Domain.Cdb.Abstractions;

public interface ICdbService
{
    Task<IResgateCdb> ResgatarAsync(ICdb cdb, CancellationToken cancellationToken);

    ICdb Investir(decimal valorInvestido, int prazoEmMeses, decimal? percentualCdi = default,
        decimal? percentualCdiPagoPeloBanco = default);
}