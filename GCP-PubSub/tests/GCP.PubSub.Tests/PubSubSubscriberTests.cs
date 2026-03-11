using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace GCP.PubSub.Tests;

public class PubSubSubscriberTests
{
    [Theory]
    [InlineData("", "my-sub")]
    [InlineData("  ", "my-sub")]
    [InlineData("my-project", "")]
    [InlineData("my-project", "  ")]
    public void OptionsValidation_RejectsEmptyProjectIdOrSubscriptionId(string projectId, string subscriptionId)
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddGcpPubSub(opts =>
        {
            opts.ProjectId = projectId;
            opts.TopicId = "my-topic";
            opts.SubscriptionId = subscriptionId;
        });

        var provider = services.BuildServiceProvider();

        var ex = Assert.Throws<OptionsValidationException>(
            () => { _ = provider.GetRequiredService<IOptions<PubSubOptions>>().Value; });

        Assert.NotNull(ex);
    }

    [Fact]
    public void AddGcpPubSub_RegistersSubscriberAsSingleton()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddGcpPubSub(opts =>
        {
            opts.ProjectId = "my-project";
            opts.TopicId = "my-topic";
            opts.SubscriptionId = "my-sub";
        });

        var provider = services.BuildServiceProvider();
        var sub1 = provider.GetRequiredService<IPubSubSubscriber>();
        var sub2 = provider.GetRequiredService<IPubSubSubscriber>();

        Assert.Same(sub1, sub2);
    }

    [Fact]
    public void AddGcpPubSub_RegistersTimeProvider()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddGcpPubSub(opts =>
        {
            opts.ProjectId = "my-project";
            opts.TopicId = "my-topic";
            opts.SubscriptionId = "my-sub";
        });

        var provider = services.BuildServiceProvider();
        var tp = provider.GetRequiredService<TimeProvider>();

        Assert.Same(TimeProvider.System, tp);
    }
}
