using AppSysMetrics.LeakLab.Simulators.Helpers;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace AppSysMetrics.LeakLab.Simulators;

/// <summary>
/// S17 — Long-lived DbContext with tracking enabled.
/// Creates an in-memory SQLite database and a <see cref="LeakLabDbContext"/>
/// that is kept alive. Entities are inserted with change tracking enabled,
/// so the change tracker accumulates <see cref="SensorReading"/> instances
/// that are never released.
/// </summary>
public sealed class S17_EfCoreTrackingSimulator : LeakSimulatorBase
{
    private SqliteConnection? _connection;
    private LeakLabDbContext? _db;

    public override string ScenarioId => "S17";

    public override string Description =>
        "Long-lived DbContext with tracking enabled — EntityEntry accumulates with each query";

    public override IReadOnlyList<string> ExpectedLeakTypes { get; } =
        ["AppSysMetrics.LeakLab.Simulators.Helpers.SensorReading", "System.Byte[]"];

    protected override async Task OnStartAsync(CancellationToken ct)
    {
        // In-memory SQLite requires keeping the connection open
        _connection = new SqliteConnection("Data Source=:memory:");
        await _connection.OpenAsync(ct);

        var options = new DbContextOptionsBuilder<LeakLabDbContext>()
            .UseSqlite(_connection)
            .Options;

        _db = new LeakLabDbContext(options);
        await _db.Database.EnsureCreatedAsync(ct);

        for (var i = 0; i < 600 && !ct.IsCancellationRequested; i++)
        {
            _db.SensorReadings.Add(new SensorReading
            {
                SensorName = $"Sensor_{i}",
                Value = Random.Shared.NextDouble() * 100,
                RawData = new byte[5_000], // 5KB per entity
                Timestamp = DateTime.UtcNow
            });

            // Save periodically to commit, but tracking stays
            if (i % 50 == 0)
                await _db.SaveChangesAsync(ct);

            await Task.Delay(3, ct);
        }

        await _db.SaveChangesAsync(ct);
    }

    public override void Reset()
    {
        _db?.Dispose();
        _db = null;
        _connection?.Dispose();
        _connection = null;
    }
}
