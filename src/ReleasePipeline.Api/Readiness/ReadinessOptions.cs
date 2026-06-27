namespace ReleasePipeline.Api.Readiness;

public sealed class ReadinessOptions
{
    public const string SectionName = "Readiness";

    public List<DependencyCheck> Dependencies { get; init; } = [];
}

public sealed class DependencyCheck
{
    public required string Name { get; init; }

    public required string Url { get; init; }

    public int TimeoutSeconds { get; init; } = 3;
}
