using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ReleasePipeline.Api.Data;
using Xunit;

namespace ReleasePipeline.Api.Tests;

public sealed class PostgresWebApplicationFactory : WebApplicationFactory<Program>
{
    public static string? ConnectionString =>
        Environment.GetEnvironmentVariable("ConnectionStrings__Default");

    public bool IsConfigured => !string.IsNullOrWhiteSpace(ConnectionString);

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        if (!IsConfigured)
        {
            return;
        }

        builder.UseSetting("ConnectionStrings:Default", ConnectionString);
    }
}

public sealed class PostgresDatabaseFixture : IAsyncLifetime
{
    public PostgresWebApplicationFactory Factory { get; } = new();

    public bool IsConfigured => Factory.IsConfigured;

    public async Task InitializeAsync()
    {
        if (!IsConfigured)
        {
            return;
        }

        _ = Factory.CreateClient();

        using var scope = Factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await dbContext.Database.MigrateAsync();

        if (!await dbContext.Deployments.AnyAsync())
        {
            dbContext.Deployments.AddRange(
                new DeploymentRecord
                {
                    Environment = "staging",
                    Status = "success",
                    DeployedAt = new DateTime(2026, 1, 15, 10, 0, 0, DateTimeKind.Utc)
                },
                new DeploymentRecord
                {
                    Environment = "production",
                    Status = "success",
                    DeployedAt = new DateTime(2026, 1, 20, 14, 30, 0, DateTimeKind.Utc)
                });

            await dbContext.SaveChangesAsync();
        }
    }

    public async Task DisposeAsync()
    {
        await Factory.DisposeAsync();
    }
}

[CollectionDefinition("Postgres")]
public sealed class PostgresCollection : ICollectionFixture<PostgresDatabaseFixture>;
