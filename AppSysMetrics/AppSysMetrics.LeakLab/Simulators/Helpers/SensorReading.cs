namespace AppSysMetrics.LeakLab.Simulators.Helpers;

/// <summary>
/// Entity for S17 (EF Core tracking leak simulation).
/// Each instance carries a byte[] payload representing raw sensor data.
/// When tracked by a long-lived DbContext, these entities accumulate
/// in the change tracker and are never released.
/// </summary>
public sealed class SensorReading
{
    public int Id { get; set; }
    public string SensorName { get; set; } = string.Empty;
    public double Value { get; set; }
    public byte[] RawData { get; set; } = [];
    public DateTime Timestamp { get; set; }
}
