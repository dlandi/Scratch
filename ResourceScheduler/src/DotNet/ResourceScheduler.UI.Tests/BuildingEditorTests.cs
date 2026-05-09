using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using ResourceScheduler.Components.Components;
using ResourceScheduler.Components.Models;
using ResourceScheduler.Components.Services;
using ResourceScheduler.UI.Tests.Fakes;
using Xunit;

namespace ResourceScheduler.UI.Tests;

public class BuildingEditorTests : BunitContext
{
    private FakeClientService RegisterFake(FakeClientService? fake = null)
    {
        var f = fake ?? new FakeClientService();
        Services.AddSingleton<IClientService>(f);
        return f;
    }

    [Fact]
    public void Title_is_New_building_when_Building_is_null()
    {
        RegisterFake();

        var cut = Render<BuildingEditor>(p => p
            .Add(c => c.IsOpen, true));

        Assert.Contains("New building", cut.Markup);
    }

    [Fact]
    public void Title_is_Edit_building_when_Building_is_provided()
    {
        RegisterFake();

        var building = new BuildingDto
        {
            BuildingId = Guid.NewGuid(),
            Name = "Lab A",
            Address = "1 Way",
            Version = 3,
        };

        var cut = Render<BuildingEditor>(p => p
            .Add(c => c.IsOpen, true)
            .Add(c => c.Building, building));

        Assert.Contains("Edit building", cut.Markup);
    }

    [Fact]
    public void Pre_fills_fields_in_edit_mode()
    {
        RegisterFake();

        var building = new BuildingDto
        {
            BuildingId = Guid.NewGuid(),
            Name = "LabZ",
            Address = "123 Main",
            Version = 2,
        };

        var cut = Render<BuildingEditor>(p => p
            .Add(c => c.IsOpen, true)
            .Add(c => c.Building, building));

        var nameInput = cut.Find("input.rs-input");
        Assert.Equal("LabZ", nameInput.GetAttribute("value"));
    }

    [Fact]
    public void Save_button_disabled_when_Name_is_empty()
    {
        RegisterFake();

        var cut = Render<BuildingEditor>(p => p
            .Add(c => c.IsOpen, true));

        var saveButton = cut.Find("button.rs-btn--primary");
        Assert.True(saveButton.HasAttribute("disabled"));
    }

    [Fact]
    public async Task Save_calls_CreateBuildingAsync_with_form_input()
    {
        var fake = RegisterFake(FakeClientService.WithStubs());

        BuildingDto? saved = null;
        var onSaved = EventCallback.Factory.Create<BuildingDto>(this, b => saved = b);

        var cut = Render<BuildingEditor>(p => p
            .Add(c => c.IsOpen, true)
            .Add(c => c.OnSaved, onSaved));

        cut.Find("input.rs-input").Input("Lab North");
        cut.Find("textarea.rs-textarea").Input("Far away");

        cut.Find("button.rs-btn--primary").Click();

        Assert.Single(fake.RecordedBuildingCreates);
        Assert.Equal("Lab North", fake.RecordedBuildingCreates[0].Name);
        Assert.Equal("Far away", fake.RecordedBuildingCreates[0].Address);
        Assert.NotNull(saved);
        await Task.CompletedTask;
    }

    [Fact]
    public void Save_calls_UpdateBuildingAsync_in_edit_mode_with_existing_version()
    {
        var fake = RegisterFake(FakeClientService.WithStubs());

        var building = new BuildingDto
        {
            BuildingId = Guid.NewGuid(),
            Name = "Original",
            Address = "Addr",
            Version = 7,
        };

        Guid? capturedId = null;
        BuildingUpdate? capturedUpdate = null;
        int? capturedVersion = null;
        fake.OnUpdateBuilding = (id, input, version) =>
        {
            capturedId = id;
            capturedUpdate = input;
            capturedVersion = version;
            return Task.FromResult(new BuildingDto
            {
                BuildingId = id,
                Name = input.Name,
                Address = input.Address,
                Version = version + 1,
            });
        };

        var cut = Render<BuildingEditor>(p => p
            .Add(c => c.IsOpen, true)
            .Add(c => c.Building, building));

        cut.Find("input.rs-input").Input("Renamed");
        cut.Find("button.rs-btn--primary").Click();

        Assert.Equal(building.BuildingId, capturedId);
        Assert.Equal(7, capturedVersion);
        Assert.NotNull(capturedUpdate);
        Assert.Equal("Renamed", capturedUpdate!.Name);
    }

    [Fact]
    public void ValidationException_is_surfaced_in_a_RuleViolationBanner()
    {
        var fake = RegisterFake();
        fake.OnCreateBuilding = _ => throw new ValidationException("R14", "msg");

        var cut = Render<BuildingEditor>(p => p
            .Add(c => c.IsOpen, true));

        cut.Find("input.rs-input").Input("X");
        cut.Find("button.rs-btn--primary").Click();

        Assert.Contains("R14", cut.Markup);
        Assert.Contains("msg", cut.Markup);
        Assert.Contains("rs-banner-rule", cut.Markup);
    }
}
