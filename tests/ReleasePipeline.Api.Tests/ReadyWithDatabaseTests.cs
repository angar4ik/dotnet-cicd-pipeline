using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace ReleasePipeline.Api.Tests;

[Collection("Postgres")]
public class ReadyWithDatabaseTests(PostgresDatabaseFixture fixture)
{
    [SkippableFact]
    public async Task Ready_WithDatabaseConfigured_ReturnsOkAndPostgresHealthy()
    {
        Skip.IfNot(fixture.IsConfigured, "Set ConnectionStrings__Default to run database integration tests.");

        using var client = fixture.Factory.CreateClient();
        var response = await client.GetAsync("/ready");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var body = await response.Content.ReadFromJsonAsync<ReadyResponse>();
        Assert.NotNull(body);
        Assert.Equal("ready", body.Status);

        var postgres = Assert.Single(body.Dependencies, dependency => dependency.Name == "postgres");
        Assert.True(postgres.Healthy);
    }

    private sealed record ReadyResponse(string Status, IReadOnlyList<DependencyResponse> Dependencies, DateTime Timestamp);

    private sealed record DependencyResponse(
        string Name,
        string Url,
        bool Healthy,
        int? StatusCode,
        long LatencyMs,
        string? Error);
}
