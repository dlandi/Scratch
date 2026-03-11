using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace GCP.PubSub.Tests;

public class PubSubPublisherTests
{
    [Theory]
    [InlineData("", "my-topic")]
    [InlineData("  ", "my-topic")]
    [InlineData("my-project", "")]
    [InlineData("my-project", "  ")]
    public void OptionsValidation_RejectsEmptyProjectIdOrTopicId(string projectId, string topicId)
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddGcpPubSub(opts =>
        {
            opts.ProjectId = projectId;
            opts.TopicId = topicId;
            opts.SubscriptionId = "my-sub";
        });

        var provider = services.BuildServiceProvider();

        var ex = Assert.Throws<OptionsValidationException>(
            () => { _ = provider.GetRequiredService<IOptions<PubSubOptions>>().Value; });

        Assert.NotNull(ex);
    }

    [Fact]
    public void OptionsValidation_AcceptsValidOptions()
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

        var options = provider.GetRequiredService<IOptions<PubSubOptions>>().Value;

        Assert.Equal("my-project", options.ProjectId);
        Assert.Equal("my-topic", options.TopicId);
        Assert.Equal("my-sub", options.SubscriptionId);
    }

    [Fact]
    public void AddGcpPubSub_RegistersPublisherAsSingleton()
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
        var pub1 = provider.GetRequiredService<IPubSubPublisher>();
        var pub2 = provider.GetRequiredService<IPubSubPublisher>();

        Assert.Same(pub1, pub2);
    }
}
