using System.Diagnostics;
using System.Reflection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using ReleasePipeline.Api.Readiness;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<ReadinessOptions>(builder.Configuration.GetSection(ReadinessOptions.SectionName));
builder.Services.AddHttpClient(nameof(ReadinessChecker));
builder.Services.AddSingleton<ReadinessChecker>();

var serviceName = builder.Configuration["OpenTelemetry:ServiceName"] ?? "ReleasePipeline.Api";
var otelExporter = builder.Configuration["OpenTelemetry:Exporter"] ?? "Console";

builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource => resource.AddService(serviceName))
    .WithTracing(tracing =>
    {
        tracing
            .AddAspNetCoreInstrumentation(options =>
            {
                options.RecordException = true;
                options.Filter = context => !context.Request.Path.StartsWithSegments("/health");
            })
            .AddHttpClientInstrumentation();

        if (string.Equals(otelExporter, "Otlp", StringComparison.OrdinalIgnoreCase))
        {
            tracing.AddOtlpExporter(options =>
            {
                options.Endpoint = new Uri(
                    builder.Configuration["OpenTelemetry:OtlpEndpoint"] ?? "http://localhost:4317");
            });
        }
        else
        {
            tracing.AddConsoleExporter();
        }
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/health", () => Results.Ok(new { status = "healthy", timestamp = DateTime.UtcNow }))
    .WithName("HealthCheck")
    .WithTags("Operations");

app.MapGet("/ready", async (ReadinessChecker readiness, CancellationToken cancellationToken) =>
{
    var report = await readiness.CheckAsync(cancellationToken);
    var statusCode = report.Status == "ready" ? StatusCodes.Status200OK : StatusCodes.Status503ServiceUnavailable;

    return Results.Json(report, statusCode: statusCode);
})
.WithName("ReadinessCheck")
.WithTags("Operations");

app.MapGet("/api/release-info", () =>
{
    var assembly = Assembly.GetExecutingAssembly();
    var version = assembly.GetName().Version?.ToString() ?? "unknown";
    var informationalVersion = assembly
        .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
        ?.InformationalVersion ?? version;
    var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

    return Results.Ok(new
    {
        service = "ReleasePipeline.Api",
        version,
        informationalVersion,
        environment,
        buildTime = GetAssemblyMetadata(assembly, "BuildTime"),
        gitSha = Environment.GetEnvironmentVariable("GIT_SHA")
            ?? GetAssemblyMetadata(assembly, "GitSha")
            ?? "local",
        deployedAt = Environment.GetEnvironmentVariable("DEPLOYED_AT")
    });
})
.WithName("ReleaseInfo")
.WithTags("Operations");

app.Run();

static string? GetAssemblyMetadata(Assembly assembly, string key)
{
    return assembly
        .GetCustomAttributes<AssemblyMetadataAttribute>()
        .FirstOrDefault(attribute => attribute.Key == key)
        ?.Value;
}

// Exposed for integration tests
public partial class Program { }
