namespace AppSysMetrics.LeakLab.Simulators.Helpers;

/// <summary>
/// Long-lived event publisher for S03 (event handler leak simulation).
/// Registered as a DI singleton. Subscribers that don't unsubscribe
/// are retained by the event's delegate invocation list.
/// </summary>
public sealed class SingletonEventPublisher
{
    public event Action<byte[]>? OnDataReceived;

    public void Publish(byte[] data) => OnDataReceived?.Invoke(data);

    /// <summary>Number of subscribers currently in the invocation list.</summary>
    public int SubscriberCount => OnDataReceived?.GetInvocationList().Length ?? 0;
}
