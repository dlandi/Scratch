using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using ResourceScheduler.Components.Components;
using ResourceScheduler.Components.Models;
using ResourceScheduler.Components.Services;
using ResourceScheduler.UI.Tests.Fakes;
using Xunit;

namespace ResourceScheduler.UI.Tests;

public class DeviceEditorTests : BunitContext
{
    private FakeClientService RegisterFake(FakeClientService? fake = null)
    {
        var f = fake ?? new FakeClientService();
        Services.AddSingleton<IClientService>(f);
        return f;
    }

    private static IReadOnlyList<BuildingDto> SampleBuildings()
    {
        return new[]
        {
            new BuildingDto
            {
                BuildingId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Name = "Building One",
                Address = "1 First St",
                Version = 1,
            },
            new BuildingDto
            {
                BuildingId = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Name = "Building Two",
                Address = "2 Second St",
                Version = 1,
            },
        };
    }

    [Fact]
    public void Title_is_New_device_when_Device_is_null()
    {
        RegisterFake();

        var cut = Render<DeviceEditor>(p => p
            .Add(c => c.IsOpen, true)
            .Add(c => c.Buildings, SampleBuildings()));

        Assert.Contains("New device", cut.Markup);
    }

    [Fact]
    public void Title_is_Edit_device_when_Device_is_provided()
    {
        RegisterFake();
        var buildings = SampleBuildings();
        var device = new DeviceDto
        {
            DeviceId = Guid.NewGuid(),
            Name = "Scope",
            Status = DeviceStatus.Available,
            BuildingId = buildings[0].BuildingId,
            Version = 4,
        };

        var cut = Render<DeviceEditor>(p => p
            .Add(c => c.IsOpen, true)
            .Add(c => c.Buildings, buildings)
            .Add(c => c.Device, device));

        Assert.Contains("Edit device", cut.Markup);
    }

    [Fact]
    public void Pre_fills_fields_in_edit_mode()
    {
        RegisterFake();
        var buildings = SampleBuildings();
        var device = new DeviceDto
        {
            DeviceId = Guid.NewGuid(),
            Name = "Scope-7",
            Status = DeviceStatus.Maintenance,
            BuildingId = buildings[1].BuildingId,
            Version = 4,
        };

        var cut = Render<DeviceEditor>(p => p
            .Add(c => c.IsOpen, true)
            .Add(c => c.Buildings, buildings)
            .Add(c => c.Device, device));

        var nameInput = cut.Find("input.rs-input");
        Assert.Equal("Scope-7", nameInput.GetAttribute("value"));
    }

    [Fact]
    public void Building_select_lists_provided_buildings()
    {
        RegisterFake();
        var buildings = SampleBuildings();

        var cut = Render<DeviceEditor>(p => p
            .Add(c => c.IsOpen, true)
            .Add(c => c.Buildings, buildings));

        var options = cut.FindAll("option");
        Assert.Contains(options, o => o.TextContent.Contains("Building One"));
        Assert.Contains(options, o => o.TextContent.Contains("Building Two"));
    }

    [Fact]
    public void Save_button_disabled_when_Name_is_empty()
    {
        RegisterFake();

        var cut = Render<DeviceEditor>(p => p
            .Add(c => c.IsOpen, true)
            .Add(c => c.Buildings, SampleBuildings()));

        var saveButton = cut.Find("button.rs-btn--primary");
        Assert.True(saveButton.HasAttribute("disabled"));
    }

    [Fact]
    public void Save_calls_CreateDeviceAsync_with_form_input()
    {
        var fake = RegisterFake(FakeClientService.WithStubs());
        var buildings = SampleBuildings();

        var cut = Render<DeviceEditor>(p => p
            .Add(c => c.IsOpen, true)
            .Add(c => c.Buildings, buildings));

        cut.Find("input.rs-input").Input("New Scope");
        cut.Find("button.rs-btn--primary").Click();

        Assert.Single(fake.RecordedDeviceCreates);
        Assert.Equal("New Scope", fake.RecordedDeviceCreates[0].Name);
        Assert.Equal(buildings[0].BuildingId, fake.RecordedDeviceCreates[0].BuildingId);
    }

    [Fact]
    public void Save_calls_UpdateDeviceAsync_in_edit_mode_with_existing_version()
    {
        var fake = RegisterFake(FakeClientService.WithStubs());
        var buildings = SampleBuildings();
        var device = new DeviceDto
        {
            DeviceId = Guid.NewGuid(),
            Name = "Old",
            Status = DeviceStatus.Available,
            BuildingId = buildings[0].BuildingId,
            Version = 9,
        };

        Guid? capturedId = null;
        DeviceUpdate? capturedUpdate = null;
        int? capturedVersion = null;
        fake.OnUpdateDevice = (id, input, version) =>
        {
            capturedId = id;
            capturedUpdate = input;
            capturedVersion = version;
            return Task.FromResult(new DeviceDto
            {
                DeviceId = id,
                Name = input.Name,
                Status = input.Status,
                BuildingId = input.BuildingId,
                Version = version + 1,
            });
        };

        var cut = Render<DeviceEditor>(p => p
            .Add(c => c.IsOpen, true)
            .Add(c => c.Buildings, buildings)
            .Add(c => c.Device, device));

        cut.Find("input.rs-input").Input("Renamed Device");
        cut.Find("button.rs-btn--primary").Click();

        Assert.Equal(device.DeviceId, capturedId);
        Assert.Equal(9, capturedVersion);
        Assert.NotNull(capturedUpdate);
        Assert.Equal("Renamed Device", capturedUpdate!.Name);
    }

    [Fact]
    public void ValidationException_R14_is_surfaced()
    {
        var fake = RegisterFake();
        fake.OnCreateDevice = _ => throw new ValidationException("R14", "every device needs a building");

        var cut = Render<DeviceEditor>(p => p
            .Add(c => c.IsOpen, true)
            .Add(c => c.Buildings, SampleBuildings()));

        cut.Find("input.rs-input").Input("Scope");
        cut.Find("button.rs-btn--primary").Click();

        Assert.Contains("R14", cut.Markup);
        Assert.Contains("every device needs a building", cut.Markup);
        Assert.Contains("rs-banner-rule", cut.Markup);
    }
}
