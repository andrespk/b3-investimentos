using B3.Investimentos.Domain.Cdb.Abstractions;

namespace B3.Investimentos.Application.Contextos.Cdb;

public interface ICdbContexto : IContexto
{
    ICdbService CdbService { get; }
}