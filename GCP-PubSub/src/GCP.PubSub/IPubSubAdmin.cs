namespace GCP.PubSub;

public interface IPubSubAdmin
{
    Task<IReadOnlyList<string>> ListTopicsAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<string>> ListSubscriptionsAsync(CancellationToken cancellationToken = default);
}
