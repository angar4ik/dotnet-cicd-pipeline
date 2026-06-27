using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace ReleasePipeline.Api.Tests;

public class ReleaseInfoEndpointTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ReleaseInfoEndpointTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task ReleaseInfo_ReturnsServiceMetadata()
    {
        var response = await _client.GetAsync("/api/release-info");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var body = await response.Content.ReadFromJsonAsync<ReleaseInfoResponse>();
        Assert.NotNull(body);
        Assert.Equal("ReleasePipeline.Api", body.Service);
        Assert.False(string.IsNullOrWhiteSpace(body.Version));
        Assert.False(string.IsNullOrWhiteSpace(body.InformationalVersion));
        Assert.Contains("+", body.InformationalVersion);
    }

    private sealed record ReleaseInfoResponse(
        string Service,
        string Version,
        string InformationalVersion,
        string Environment,
        string? BuildTime,
        string GitSha,
        string? DeployedAt);
}
