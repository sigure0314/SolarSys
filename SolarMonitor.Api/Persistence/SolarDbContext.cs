using Microsoft.EntityFrameworkCore;
using SolarMonitor.Api.Entities;

namespace SolarMonitor.Api.Persistence;

public class SolarDbContext : DbContext
{
    public SolarDbContext(DbContextOptions<SolarDbContext> options) : base(options)
    {
    }

    public DbSet<Site> Sites => Set<Site>();
    public DbSet<Inverter> Inverters => Set<Inverter>();
    public DbSet<TelemetryReading> TelemetryReadings => Set<TelemetryReading>();
    public DbSet<Alarm> Alarms => Set<Alarm>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Site>()
            .HasMany(s => s.Inverters)
            .WithOne(i => i.Site)
            .HasForeignKey(i => i.SiteId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Inverter>()
            .HasMany(i => i.TelemetryReadings)
            .WithOne(t => t.Inverter)
            .HasForeignKey(t => t.InverterId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Inverter>()
            .HasMany(i => i.Alarms)
            .WithOne(a => a.Inverter)
            .HasForeignKey(a => a.InverterId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Site>()
            .HasMany<Alarm>()
            .WithOne(a => a.Site)
            .HasForeignKey(a => a.SiteId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Site>()
            .HasMany<TelemetryReading>()
            .WithOne(tr => tr.Site)
            .HasForeignKey(tr => tr.SiteId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TelemetryReading>()
            .HasIndex(tr => new { tr.InverterId, tr.Timestamp });
    }
}
