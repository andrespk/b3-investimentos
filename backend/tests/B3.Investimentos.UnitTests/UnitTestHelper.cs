using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;

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
}