using Google.Api.Gax.ResourceNames;
using Google.Cloud.PubSub.V1;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GCP.PubSub;

public sealed class PubSubAdmin : IPubSubAdmin
{
    private readonly IOptions<PubSubOptions> _options;
    private readonly ILogger<PubSubAdmin> _logger;

    public PubSubAdmin(IOptions<PubSubOptions> options, ILogger<PubSubAdmin> logger)
    {
        _options = options;
        _logger = logger;
    }

    public async Task<IReadOnlyList<string>> ListTopicsAsync(CancellationToken cancellationToken = default)
    {
        var opts = _options.Value;
        var projectName = new ProjectName(opts.ProjectId);

        var client = await PublisherServiceApiClient.CreateAsync(cancellationToken).ConfigureAwait(false);
        var topics = client.ListTopicsAsync(projectName);

        var topicIds = new List<string>();
        await foreach (var topic in topics.ConfigureAwait(false))
        {
            cancellationToken.ThrowIfCancellationRequested();
            topicIds.Add(topic.TopicName.TopicId);
        }

        _logger.ListedTopics(topicIds.Count);
        return topicIds;
    }

    public async Task<IReadOnlyList<string>> ListSubscriptionsAsync(CancellationToken cancellationToken = default)
    {
        var opts = _options.Value;
        var projectName = new ProjectName(opts.ProjectId);

        var client = await SubscriberServiceApiClient.CreateAsync(cancellationToken).ConfigureAwait(false);
        var subscriptions = client.ListSubscriptionsAsync(projectName);

        var subscriptionIds = new List<string>();
        await foreach (var sub in subscriptions.ConfigureAwait(false))
        {
            cancellationToken.ThrowIfCancellationRequested();
            subscriptionIds.Add(sub.SubscriptionName.SubscriptionId);
        }

        _logger.ListedSubscriptions(subscriptionIds.Count);
        return subscriptionIds;
    }
}
