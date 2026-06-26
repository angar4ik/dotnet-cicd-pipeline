using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/health", () => Results.Ok(new { status = "healthy", timestamp = DateTime.UtcNow }))
    .WithName("HealthCheck")
    .WithTags("Operations");

app.MapGet("/api/release-info", () =>
{
    var assembly = Assembly.GetExecutingAssembly();
    var version = assembly.GetName().Version?.ToString() ?? "unknown";
    var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

    return Results.Ok(new
    {
        service = "ReleasePipeline.Api",
        version,
        environment,
        buildTime = GetBuildTime(assembly),
        gitSha = Environment.GetEnvironmentVariable("GIT_SHA") ?? "local",
        deployedAt = Environment.GetEnvironmentVariable("DEPLOYED_AT")
    });
})
.WithName("ReleaseInfo")
.WithTags("Operations");

app.Run();

static string? GetBuildTime(Assembly assembly)
{
    var attribute = assembly.GetCustomAttribute<AssemblyMetadataAttribute>();
    return attribute?.Key == "BuildTime" ? attribute.Value : null;
}

// Exposed for integration tests
public partial class Program { }
