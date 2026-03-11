namespace GCP.PubSub;

public interface IPubSubPublisher : IAsyncDisposable
{
    Task<string> PublishAsync(string message, CancellationToken cancellationToken = default);

    Task<string> PublishAsync(string message, IDictionary<string, string>? attributes, CancellationToken cancellationToken = default);

    Task<int> PublishBatchAsync(IEnumerable<string> messages, CancellationToken cancellationToken = default);
}
