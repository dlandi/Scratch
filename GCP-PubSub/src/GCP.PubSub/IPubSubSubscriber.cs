namespace GCP.PubSub;

public interface IPubSubSubscriber : IAsyncDisposable
{
    Task<int> PullMessagesAsync(
        Func<string, IDictionary<string, string>, CancellationToken, Task<bool>> handler,
        CancellationToken cancellationToken = default);

    Task<int> PullMessagesAsync(
        Func<string, IDictionary<string, string>, CancellationToken, Task<bool>> handler,
        TimeSpan listenDuration,
        CancellationToken cancellationToken = default);
}
