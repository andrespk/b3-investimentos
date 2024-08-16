using B3.Investimentos.Api.Extensions;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("../../logs/log.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
builder.AdicionarDependenciasDeDominio();
builder.AdicionarInfraestrutura();

var app = builder.Build();
app.ConfigurarRotas();
app.ConfigurarTratamentoDeErros();
app.ConfigurarDocumentacao();

await app.RunAsync();