using B3.Investimentos.Domain.Cdb.Abstractions;

namespace B3.Investimentos.Domain.Cdb;

public class CdbService(IResgateCdb resgate) : ICdbService
{
    public async Task<IResgateCdb> ResgatarAsync(ICdb cdb, int prazoEmMeses, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        resgate.Resgatar(cdb, prazoEmMeses);
        return await Task.FromResult(resgate);
    }
}