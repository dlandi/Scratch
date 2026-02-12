using Microsoft.EntityFrameworkCore;

namespace AppSysMetrics.LeakLab.Simulators.Helpers;

/// <summary>
/// DbContext for S17 (EF Core tracking leak simulation).
/// Uses in-memory SQLite. Accepts externally-managed options so the
/// simulator can control the connection lifecycle.
/// </summary>
public sealed class LeakLabDbContext : DbContext
{
    public LeakLabDbContext(DbContextOptions<LeakLabDbContext> options)
        : base(options)
    {
    }

    public DbSet<SensorReading> SensorReadings => Set<SensorReading>();
}
