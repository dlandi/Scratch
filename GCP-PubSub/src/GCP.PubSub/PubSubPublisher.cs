using Google.Cloud.PubSub.V1;
using Google.Protobuf;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GCP.PubSub;

public sealed class PubSubPublisher : IPubSubPublisher
{
    private readonly IOptions<PubSubOptions> _options;
    private readonly ILogger<PubSubPublisher> _logger;
    private readonly SemaphoreSlim _initLock = new(1, 1);
    private PublisherClient? _publisher;
    private bool _disposed;

    public PubSubPublisher(IOptions<PubSubOptions> options, ILogger<PubSubPublisher> logger)
    {
        _options = options;
        _logger = logger;
    }

    public async Task<string> PublishAsync(string message, CancellationToken cancellationToken = default)
    {
        return await PublishAsync(message, null, cancellationToken).ConfigureAwait(false);
    }

    public async Task<string> PublishAsync(string message, IDictionary<string, string>? attributes, CancellationToken cancellationToken = default)
    {
        var publisher = await GetPublisherAsync().ConfigureAwait(false);

        var pubsubMessage = new PubsubMessage
        {
            Data = ByteString.CopyFromUtf8(message)
        };

        if (attributes is not null)
        {
            foreach (var kvp in attributes)
            {
                pubsubMessage.Attributes.Add(kvp.Key, kvp.Value);
            }
        }

        try
        {
            var messageId = await publisher.PublishAsync(pubsubMessage).ConfigureAwait(false);
            _logger.PublishedMessage(messageId);
            return messageId;
        }
        catch (Exception ex)
        {
            _logger.PublishFailed(ex);
            throw;
        }
    }

    public async Task<int> PublishBatchAsync(IEnumerable<string> messages, CancellationToken cancellationToken = default)
    {
        var messageList = messages.ToList();
        if (messageList.Count == 0)
            return 0;

        var publisher = await GetPublisherAsync().ConfigureAwait(false);
        int publishedCount = 0;

        var publishTasks = messageList.Select(async text =>
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                var messageId = await publisher.PublishAsync(text).ConfigureAwait(false);
                _logger.PublishedMessage(messageId);
                Interlocked.Increment(ref publishedCount);
            }
            catch (Exception ex)
            {
                _logger.PublishFailed(ex);
            }
        });

        await Task.WhenAll(publishTasks).ConfigureAwait(false);
        return publishedCount;
    }

    public async ValueTask DisposeAsync()
    {
        if (_disposed) return;
        _disposed = true;

        if (_publisher is not null)
        {
            await _publisher.ShutdownAsync(TimeSpan.FromSeconds(15)).ConfigureAwait(false);
        }

        _initLock.Dispose();
    }

    private async Task<PublisherClient> GetPublisherAsync()
    {
        ObjectDisposedException.ThrowIf(_disposed, this);

        if (_publisher is not null)
            return _publisher;

        await _initLock.WaitAsync().ConfigureAwait(false);
        try
        {
            if (_publisher is not null)
                return _publisher;

            var opts = _options.Value;
            var topicName = TopicName.FromProjectTopic(opts.ProjectId, opts.TopicId);
            _publisher = await PublisherClient.CreateAsync(topicName).ConfigureAwait(false);
            return _publisher;
        }
        finally
        {
            _initLock.Release();
        }
    }
}
