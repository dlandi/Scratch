namespace AppSysMetrics.LeakLab.Simulators.Helpers;

/// <summary>
/// Target object for <c>DotNetObjectReference&lt;T&gt;</c> leak simulation (S01).
/// Each instance carries a byte[] payload to ensure measurable heap impact.
/// </summary>
public sealed class DotNetObjectRefTarget
{
    public byte[] Payload { get; }

    public DotNetObjectRefTarget(int payloadSize)
    {
        Payload = new byte[payloadSize];
        Array.Fill(Payload, (byte)0xDE);
    }
}
