using System.Diagnostics;
using Microsoft.Extensions.Options;

namespace ReleasePipeline.Api.Readiness;

public sealed class ReadinessChecker(IHttpClientFactory httpClientFactory, IOptions<ReadinessOptions> options)
{
    public async Task<ReadinessReport> CheckAsync(CancellationToken cancellationToken = default)
    {
        var dependencies = options.Value.Dependencies;

        if (dependencies.Count == 0)
        {
            return new ReadinessReport("ready", [], DateTime.UtcNow);
        }

        var client = httpClientFactory.CreateClient(nameof(ReadinessChecker));
        var results = new List<DependencyStatus>(dependencies.Count);

        foreach (var dependency in dependencies)
        {
            results.Add(await ProbeDependencyAsync(client, dependency, cancellationToken));
        }

        var status = results.All(result => result.Healthy) ? "ready" : "not_ready";
        return new ReadinessReport(status, results, DateTime.UtcNow);
    }

    private static async Task<DependencyStatus> ProbeDependencyAsync(
        HttpClient client,
        DependencyCheck dependency,
        CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();

        try
        {
            using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(dependency.TimeoutSeconds));
            using var linked = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeout.Token);

            using var response = await client.GetAsync(dependency.Url, linked.Token);
            stopwatch.Stop();

            return new DependencyStatus(
                dependency.Name,
                dependency.Url,
                response.IsSuccessStatusCode,
                (int)response.StatusCode,
                stopwatch.ElapsedMilliseconds,
                response.IsSuccessStatusCode ? null : $"HTTP {(int)response.StatusCode}");
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            return new DependencyStatus(
                dependency.Name,
                dependency.Url,
                false,
                null,
                stopwatch.ElapsedMilliseconds,
                ex.Message);
        }
    }
}

public sealed record ReadinessReport(string Status, IReadOnlyList<DependencyStatus> Dependencies, DateTime Timestamp);

public sealed record DependencyStatus(
    string Name,
    string Url,
    bool Healthy,
    int? StatusCode,
    long LatencyMs,
    string? Error);
