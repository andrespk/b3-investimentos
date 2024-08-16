using System.Diagnostics.CodeAnalysis;
using B3.Investimentos.Infrastructure.Caching;
using B3.Investimentos.Infrastructure.Caching.Abstractions;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace B3.Investimentos.UnitTests.Cache;

public class CacheServiceTests
{
    [Fact(DisplayName = "Deve operar corretamente com o provedor de cache em memória")]
    public async Task DeveOperarCorretamenteComProvedorCacheEmMemoriaAsync()
    {
        var provider = ObterCacheProvider();
        var cacheKey = Guid.NewGuid().ToString();
        var valorEsperado = "TESTE123";
        var ttl = TimeSpan.FromSeconds(5);
        var cancellationToken = ObterCancellationToken();
        
        await provider.RegistrarAsync(cacheKey, valorEsperado, ttl, cancellationToken);
        var cache = await provider.ObterAsync<string>(cacheKey, cancellationToken);

        string.IsNullOrEmpty(cache).Should().BeFalse();
        cache.Should().Be(valorEsperado);
    }

    [Fact(DisplayName = "Deve obter o valor em cache corretamente")]
    public async Task DeveObterCacheCorretamenteAsync()
    {
        var (servico, cacheProvider) = ObterCenario();
        var cacheKey = servico.GerarChave(Guid.NewGuid());
        var valorEsperado = 0;
        var cancellationToken = ObterCancellationToken();
        var ttl = TimeSpan.FromSeconds(2);

        await servico.RegistrarAsync(cacheKey, valorEsperado, ttl, cancellationToken);
        var resultado = await servico.ObterAsync<int>(cacheKey, cancellationToken);

        resultado.Should().Be(valorEsperado);
        A.CallTo(() => cacheProvider.ObterAsync<int>(cacheKey, cancellationToken)).MustHaveHappenedOnceExactly();
        A.CallTo(() => cacheProvider.RegistrarAsync(cacheKey, valorEsperado, ttl, cancellationToken))
            .MustHaveHappenedOnceExactly();
    }

    [Fact(DisplayName = "Deve Obter o valor em cache vazio apos o periodo de rentencao")]
    public async Task DeveObterCacheVazioAsync()
    {
        var servico = ObterServico();
        var cacheKey = servico.GerarChave(Guid.NewGuid());
        var valorEsperado = Guid.NewGuid();
        var cancellationToken = ObterCancellationToken();
        var ttl = TimeSpan.FromMilliseconds(1);

        await servico.RegistrarAsync(cacheKey, valorEsperado, ttl, cancellationToken);
        await Task.Delay(2);
        var resultado = await servico.ObterAsync<int?>(cacheKey, cancellationToken);

        resultado.Should().BeNull();
    }

    private static (ICacheService, ICacheProvider) ObterCenario()
    {
        var cacheProvider = A.Fake<ICacheProvider>();
        return (new CacheService(cacheProvider), cacheProvider);
    }

    private static ICacheService ObterServico()
    {
        var memoryCache = A.Fake<IMemoryCache>();
        var cacheProvider = new MemoryCacheProvider(memoryCache);
        return new CacheService(cacheProvider);
    }

    private static CancellationToken ObterCancellationToken() => new();
    
    private static MemoryCacheProvider ObterCacheProvider()
    {
        var services = new ServiceCollection();
        services.AddMemoryCache();
        var serviceProvider = services.BuildServiceProvider();

        var memoryCache = serviceProvider.GetService<IMemoryCache>() ?? A.Fake<IMemoryCache>();
        var provider = new MemoryCacheProvider(memoryCache);
        return provider;
    }
}