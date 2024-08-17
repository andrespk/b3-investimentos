using B3.Investimentos.Domain.Cdb.Abstractions;

namespace B3.Investimentos.Domain.Cdb;

public class CdbService(IResgateCdb resgateCdb) : ICdbService
{
    public async Task<IResgateCdb> ResgatarAsync(ICdb cdb, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        resgateCdb.Resgatar(cdb);
        return await Task.FromResult(resgateCdb);
    }
}