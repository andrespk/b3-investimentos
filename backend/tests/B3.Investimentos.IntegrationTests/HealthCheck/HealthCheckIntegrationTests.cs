
using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using HealthStatus = B3.Investimentos.Application.DTO.HealthStatus;

namespace B3.Investimentos.IntegrationTests.HealthCheck;

public class HealthCheckIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    public HealthCheckIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }
    
    [Fact(DisplayName = "Deve retornar o status corretamente")]
    public async Task DeveRetornarStatusComSucessoAsync()
    {
        var statusEsperado = new HealthStatus();

        var response = await _client.GetAsync("/health");
        var statusObtido = await response.Content.ReadFromJsonAsync<HealthStatus>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        statusObtido!.Healthy.Equals(statusEsperado.Healthy).Should().BeTrue();
        statusObtido.UtcNow.Should().BeOnOrAfter(statusEsperado.UtcNow);
    }
}