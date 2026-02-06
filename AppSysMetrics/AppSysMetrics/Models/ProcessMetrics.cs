namespace AppSysMetrics.Models;

public sealed record ProcessMetrics
{
    public long WorkingSet64 { get; init; }
    public long PrivateMemorySize64 { get; init; }
    public long VirtualMemorySize64 { get; init; }
    public long PagedMemorySize64 { get; init; }
    public int ThreadCount { get; init; }
    public int HandleCount { get; init; }
}
