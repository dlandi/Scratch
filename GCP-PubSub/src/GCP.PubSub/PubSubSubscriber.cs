using Google.Cloud.PubSub.V1;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GCP.PubSub;

public sealed class PubSubSubscriber : IPubSubSubscriber
{
    private static readonly TimeSpan DefaultListenDuration = TimeSpan.FromSeconds(5);

    private readonly IOptions<PubSubOptions> _options;
    private readonly ILogger<PubSubSubscriber> _logger;
    private readonly TimeProvider _timeProvider;
    private SubscriberClient? _activeSubscriber;
    private bool _disposed;

    public PubSubSubscriber(
        IOptions<PubSubOptions> options,
        ILogger<PubSubSubscriber> logger,
        TimeProvider timeProvider)
    {
        _options = options;
        _logger = logger;
        _timeProvider = timeProvider;
    }

    public Task<int> PullMessagesAsync(
        Func<string, IDictionary<string, string>, CancellationToken, Task<bool>> handler,
        CancellationToken cancellationToken = default)
    {
        return PullMessagesAsync(handler, DefaultListenDuration, cancellationToken);
    }

    public async Task<int> PullMessagesAsync(
        Func<string, IDictionary<string, string>, CancellationToken, Task<bool>> handler,
        TimeSpan listenDuration,
        CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        var opts = _options.Value;
        var subscriptionName = SubscriptionName.FromProjectSubscription(opts.ProjectId, opts.SubscriptionId);
        var subscriber = await SubscriberClient.CreateAsync(subscriptionName).ConfigureAwait(false);
        _activeSubscriber = subscriber;

        int messageCount = 0;

        _logger.SubscriberStarted(listenDuration);

        var startTask = subscriber.StartAsync((PubsubMessage message, CancellationToken cancel) =>
        {
            var text = message.Data.ToStringUtf8();
            var attributes = (IDictionary<string, string>)message.Attributes;

            _logger.ReceivedMessage(message.MessageId, text);
            Interlocked.Increment(ref messageCount);

            return handler(text, attributes, cancel).ContinueWith(
                t => t.Result ? SubscriberClient.Reply.Ack : SubscriberClient.Reply.Nack,
                cancel,
                TaskContinuationOptions.ExecuteSynchronously,
                TaskScheduler.Default);
        });

        // TimeProvider-aware delay: CancellationTokenSource(TimeSpan, TimeProvider) is .NET 8 BCL
        using var delayCts = new CancellationTokenSource(listenDuration, _timeProvider);
        using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, delayCts.Token);

        try
        {
            await Task.Delay(Timeout.InfiniteTimeSpan, linkedCts.Token).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            // Either timer expired (normal) or external cancellation — both stop gracefully
        }

        await subscriber.StopAsync(CancellationToken.None).ConfigureAwait(false);
        await startTask.ConfigureAwait(false);

        _activeSubscriber = null;

        _logger.SubscriberStopped(messageCount);
        return messageCount;
    }

    public async ValueTask DisposeAsync()
    {
        if (_disposed) return;
        _disposed = true;

        if (_activeSubscriber is not null)
        {
            await _activeSubscriber.StopAsync(CancellationToken.None).ConfigureAwait(false);
            _activeSubscriber = null;
        }
    }
}
