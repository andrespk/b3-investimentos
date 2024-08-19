using B3.Investimentos.Api.Extensions;
using Serilog;

namespace B3.Investimentos.Api;

public static class AplicacaoProvider
{
    public static WebApplicationBuilder ObterBuilder(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("../../logs/log.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        var builder = WebApplication.CreateBuilder(args);
        builder.AdicionarDependenciasDeDominio();
        builder.AdicionarInfraestrutura();
        
        return builder;
    }
    
    public static WebApplication ObterWebApplication(string[] args)
    {
        var builder = ObterBuilder(args);
        var webApplication = builder.Build();
        webApplication.ConfigurarRotas();
        webApplication.ConfigurarTratamentoDeErros();
        webApplication.ConfigurarDocumentacao();
        webApplication.ConfigurarHealthCheck();
        
        return webApplication;
    }
    
    public static WebApplication ObterWebApplication(WebApplicationBuilder builder)
    {
        var webApplication = builder.Build();
        webApplication.ConfigurarRotas();
        webApplication.ConfigurarTratamentoDeErros();
        webApplication.ConfigurarDocumentacao();
        webApplication.ConfigurarHealthCheck();
        
        return webApplication;
    }
}