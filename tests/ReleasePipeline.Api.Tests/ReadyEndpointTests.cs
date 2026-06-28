using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace ReleasePipeline.Api.Tests;

public class ReadyEndpointTests : IClassFixture<ApiWebApplicationFactory>
{
    private readonly HttpClient _client;

    public ReadyEndpointTests(ApiWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Ready_WithNoDependencies_ReturnsOk()
    {
        var response = await _client.GetAsync("/ready");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var body = await response.Content.ReadFromJsonAsync<ReadyResponse>();
        Assert.NotNull(body);
        Assert.Equal("ready", body.Status);
        Assert.Empty(body.Dependencies);
    }

    private sealed record ReadyResponse(string Status, IReadOnlyList<object> Dependencies, DateTime Timestamp);
}
