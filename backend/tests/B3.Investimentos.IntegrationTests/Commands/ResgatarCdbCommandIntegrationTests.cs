using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Nodes;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;

namespace B3.Investimentos.IntegrationTests.Commands;

public class ResgatarCdbCommandIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    internal record ResgatarCdbRequest(
        decimal ValorInvestido,
        decimal PrazoEmMeses,
        decimal? PercentualCdi,
        decimal? PercentualCdiPagoPeloBanco);

    private readonly HttpClient _client;

    public ResgatarCdbCommandIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact(DisplayName = "Deve retornar regatar o CDB corretamente")]
    public async Task DeveResgatarCdbCorretamenteAsync()
    {
        var valorInvestido = 1000;
        var prazoEmMeses = 25;
        var percentualCdi = 0.9M;
        var percentualCdiPagoPeloBanco = 108;
        var valorBrutoEsperado = 1273.45M;
        var valorLiquidoEsperado = 1232.44M;
        var input = JsonContent.Create(
            new ResgatarCdbRequest(valorInvestido, prazoEmMeses, percentualCdi, percentualCdiPagoPeloBanco));

        var response = await _client.PostAsync("/cdb/resgatar", input);
        var jsonObject = (await JsonNode.ParseAsync(await response.Content.ReadAsStreamAsync()))?.AsObject();
        var sucesso = ObterValor<bool>(jsonObject, "sucesso");
        var dadosJsonObject = ObterValor<JsonNode>(jsonObject, "dados")?.AsObject();
        var valorBruto = ObterValor<decimal>(dadosJsonObject, "valorBruto");
        var valorLiquido = ObterValor<decimal>(dadosJsonObject, "valorLiquido");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        sucesso.Should().BeTrue();
        valorLiquido.Should().BeLessThan(valorBruto);
        valorLiquido.Should().BeGreaterThan(valorInvestido);
        valorBruto.Should().Be(valorBrutoEsperado);
        valorLiquido.Should().Be(valorLiquidoEsperado);
    }
    
    [Fact(DisplayName = "Deve retornar regatar o CDB corretamente sem informar os percentuais de CDI")]
    public async Task DeveResgatarCdbCorretamenteSemInformarPercentuaisDeCdiAsync()
    {
        var valorInvestido = 1000;
        var prazoEmMeses = 25;
        decimal? percentualCdi = null;
        decimal? percentualCdiPagoPeloBanco = null;
        var valorBrutoEsperado = 1273.45M;
        var valorLiquidoEsperado = 1232.44M;
        var input = JsonContent.Create(
            new ResgatarCdbRequest(valorInvestido, prazoEmMeses, percentualCdi, percentualCdiPagoPeloBanco));

        var response = await _client.PostAsync("/cdb/resgatar", input);
        var jsonObject = (await JsonNode.ParseAsync(await response.Content.ReadAsStreamAsync()))?.AsObject();
        var sucesso = ObterValor<bool>(jsonObject, "sucesso");
        var dadosJsonObject = ObterValor<JsonNode>(jsonObject, "dados")?.AsObject();
        var valorBruto = ObterValor<decimal>(dadosJsonObject, "valorBruto");
        var valorLiquido = ObterValor<decimal>(dadosJsonObject, "valorLiquido");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        sucesso.Should().BeTrue();
        valorLiquido.Should().BeLessThan(valorBruto);
        valorLiquido.Should().BeGreaterThan(valorInvestido);
        valorBruto.Should().Be(valorBrutoEsperado);
        valorLiquido.Should().Be(valorLiquidoEsperado);
    }
    
    [Theory(DisplayName = "Deve falhar ao resgatar o CDB")]
    [InlineData(1000, 1)]
    [InlineData(0, 2)]
    public async Task DeveFalharAoResgatarCdbAsync(decimal valorInvestido, decimal prazoEmMeses)
    {
        decimal? percentualCdi = null;
        decimal? percentualCdiPagoPeloBanco = null;
        var input = JsonContent.Create(
            new ResgatarCdbRequest(valorInvestido, prazoEmMeses, percentualCdi, percentualCdiPagoPeloBanco));

        var response = await _client.PostAsync("/cdb/resgatar", input);
        var jsonObject = (await JsonNode.ParseAsync(await response.Content.ReadAsStreamAsync()))?.AsObject();
        var sucesso = ObterValor<bool>(jsonObject, "sucesso");
        var errosJsonObject = ObterValor<JsonNode>(jsonObject, "dados")?.AsObject();
        
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        sucesso.Should().BeFalse();
        errosJsonObject?.Count.Should().BeGreaterThan(0);
    }

    private T? ObterValor<T>(JsonObject? jsonNode, string propriedade)
    {
        if (jsonNode is not null)
        {
            var sucesso = jsonNode.TryGetPropertyValue(propriedade, out JsonNode? innerJsonNode);
            if (sucesso && innerJsonNode is not null)
            {
                var json = innerJsonNode.ToJsonString();
                return JsonSerializer.Deserialize<T>(json);
            }
        }
        return default;
    }
}