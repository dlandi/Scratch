using ResourceScheduler.Components.Services;

namespace ResourceScheduler.Tests;

public class BackendSwitcherTests
{
    private static BackendSwitcher NewSwitcher()
    {
        var inMemory = new InMemoryClientService();
        // Concrete RemoteClientService is only invoked when Mode flips
        // to Rust; for these tests we never hit it, so the HttpClient
        // base address is just a parsable placeholder.
        var remote = new RemoteClientService(
            new HttpClient { BaseAddress = new Uri("http://localhost:1") });
        return new BackendSwitcher(inMemory, remote);
    }

    [Fact]
    public void Default_mode_is_in_memory()
    {
        var sut = NewSwitcher();
        Assert.Equal(BackendMode.InMemory, sut.Mode);
    }

    [Fact]
    public void SetMode_updates_mode_and_raises_changed_once()
    {
        var sut = NewSwitcher();
        var changes = 0;
        sut.Changed += () => changes++;

        sut.SetMode(BackendMode.Rust);

        Assert.Equal(BackendMode.Rust, sut.Mode);
        Assert.Equal(1, changes);
    }

    [Fact]
    public void SetMode_to_same_value_is_a_noop_and_does_not_raise()
    {
        var sut = NewSwitcher();
        var changes = 0;
        sut.Changed += () => changes++;

        sut.SetMode(BackendMode.InMemory); // already InMemory

        Assert.Equal(BackendMode.InMemory, sut.Mode);
        Assert.Equal(0, changes);
    }

    [Fact]
    public async Task Calls_forward_to_in_memory_when_mode_is_in_memory()
    {
        var sut = NewSwitcher();
        // Sanity check: in-memory CreateBuilding must succeed without
        // the Rust client ever being hit (we'd see a connection refused
        // since BaseAddress points to a closed port).
        var b = await sut.CreateBuildingAsync(new Components.Models.BuildingCreate("X", "y"));
        Assert.NotEqual(Guid.Empty, b.BuildingId);
    }
}
