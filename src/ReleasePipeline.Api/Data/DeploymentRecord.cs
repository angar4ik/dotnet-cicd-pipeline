namespace ReleasePipeline.Api.Data;

public sealed class DeploymentRecord
{
    public int Id { get; set; }

    public required string Environment { get; set; }

    public required string Status { get; set; }

    public DateTime DeployedAt { get; set; }
}
