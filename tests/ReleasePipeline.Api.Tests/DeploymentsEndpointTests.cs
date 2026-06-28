using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace ReleasePipeline.Api.Tests;

[Collection("Postgres")]
public class DeploymentsEndpointTests(PostgresDatabaseFixture fixture)
{
    [SkippableFact]
    public async Task Deployments_ReturnsSeededRecords()
    {
        Skip.IfNot(fixture.IsConfigured, "Set ConnectionStrings__Default to run database integration tests.");

        using var client = fixture.Factory.CreateClient();
        var response = await client.GetAsync("/api/deployments");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var body = await response.Content.ReadFromJsonAsync<List<DeploymentResponse>>();
        Assert.NotNull(body);
        Assert.Equal(2, body.Count);
        Assert.Contains(body, record => record.Environment == "staging" && record.Status == "success");
        Assert.Contains(body, record => record.Environment == "production" && record.Status == "success");
    }

    private sealed record DeploymentResponse(int Id, string Environment, string Status, DateTime DeployedAt);
}
