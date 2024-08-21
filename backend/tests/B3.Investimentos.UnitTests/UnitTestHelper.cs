using System.Diagnostics.CodeAnalysis;
using B3.Investimentos.Infrastructure.Caching;
using FakeItEasy;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace B3.Investimentos.UnitTests;

[ExcludeFromCodeCoverage]
public static class UnitTestHelper
{
    public static IConfigurationRoot GetIConfigurationRoot()
    {
        return new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", true, true)
            .AddJsonFile("appsettings.Development.json", true,
                true)
            .AddEnvironmentVariables()
            .Build();
    }
    
    public static MemoryCacheProvider ObterCacheProvider()
    {
        var services = new ServiceCollection();
        services.AddMemoryCache();
        var serviceProvider = services.BuildServiceProvider();

        var memoryCache = serviceProvider.GetService<IMemoryCache>() ?? A.Fake<IMemoryCache>();
        var provider = new MemoryCacheProvider(memoryCache);
        return provider;
    }
}