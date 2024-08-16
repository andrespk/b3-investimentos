using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Http;

namespace B3.Investimentos.Infrastructure.Logging;

[ExcludeFromCodeCoverage]
public record MetadadosRequisicao(HttpRequest Request, int ResponseStatus);