namespace AppSysMetrics.LeakLab.Simulators.Helpers;

/// <summary>
/// Simulates a Blazor component that subscribes to a singleton event
/// but never unsubscribes (S03). Each instance carries a byte[] payload
/// representing component state. The delegate to <see cref="OnData"/>
/// captures <c>this</c>, retaining the instance through the event's
/// invocation list.
/// </summary>
public sealed class EventSubscriberComponent
{
    public byte[] State { get; }

    public EventSubscriberComponent(int stateSize)
    {
        State = new byte[stateSize];
        Array.Fill(State, (byte)0xEE);
    }

    /// <summary>
    /// Event handler — implicitly captures <c>this</c> when used as a delegate.
    /// The actual data is irrelevant; the leak is the retained reference.
    /// </summary>
    public void OnData(byte[] data) { /* retained by delegate */ }
}
