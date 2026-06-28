using Microsoft.EntityFrameworkCore;

namespace ReleasePipeline.Api.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<DeploymentRecord> Deployments => Set<DeploymentRecord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DeploymentRecord>(entity =>
        {
            entity.ToTable("deployments");
            entity.HasKey(record => record.Id);
            entity.Property(record => record.Environment).HasMaxLength(64).IsRequired();
            entity.Property(record => record.Status).HasMaxLength(32).IsRequired();
        });
    }
}
