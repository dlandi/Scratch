using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace GCP.PubSub.Tests;

public class PubSubAdminTests
{
    [Fact]
    public void AddGcpPubSub_RegistersAdminAsSingleton()
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
        var admin1 = provider.GetRequiredService<IPubSubAdmin>();
        var admin2 = provider.GetRequiredService<IPubSubAdmin>();

        Assert.Same(admin1, admin2);
    }
}
