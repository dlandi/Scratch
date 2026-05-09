using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using ResourceScheduler.Components.Components;
using ResourceScheduler.Components.Models;
using ResourceScheduler.Components.Services;
using ResourceScheduler.UI.Tests.Fakes;
using Xunit;

namespace ResourceScheduler.UI.Tests;

public class TestGroupEditorTests : BunitContext
{
    private FakeClientService RegisterFake(FakeClientService? fake = null)
    {
        var f = fake ?? new FakeClientService();
        Services.AddSingleton<IClientService>(f);
        return f;
    }

    [Fact]
    public void Title_is_New_test_group_when_TestGroup_is_null()
    {
        RegisterFake();

        var cut = Render<TestGroupEditor>(p => p
            .Add(c => c.IsOpen, true));

        Assert.Contains("New test-group", cut.Markup);
    }

    [Fact]
    public void Title_is_Rename_test_group_when_TestGroup_is_provided()
    {
        RegisterFake();

        var group = new TestGroupDto
        {
            TestGroupId = Guid.NewGuid(),
            Name = "Crew One",
            MemberIds = new List<Guid>(),
            Version = 2,
        };

        var cut = Render<TestGroupEditor>(p => p
            .Add(c => c.IsOpen, true)
            .Add(c => c.TestGroup, group));

        Assert.Contains("Rename test-group", cut.Markup);
    }

    [Fact]
    public void Pre_fills_name_in_edit_mode()
    {
        RegisterFake();

        var group = new TestGroupDto
        {
            TestGroupId = Guid.NewGuid(),
            Name = "Crew One",
            MemberIds = new List<Guid>(),
            Version = 4,
        };

        var cut = Render<TestGroupEditor>(p => p
            .Add(c => c.IsOpen, true)
            .Add(c => c.TestGroup, group));

        var nameInput = cut.Find("input.rs-input");
        Assert.Equal("Crew One", nameInput.GetAttribute("value"));
    }

    [Fact]
    public void Save_button_disabled_when_name_is_empty()
    {
        RegisterFake();

        var cut = Render<TestGroupEditor>(p => p
            .Add(c => c.IsOpen, true));

        var saveButton = cut.Find("button.rs-btn--primary");
        Assert.True(saveButton.HasAttribute("disabled"));
    }

    [Fact]
    public void Save_calls_CreateTestGroupAsync_in_create_mode_with_typed_input()
    {
        var fake = RegisterFake(FakeClientService.WithStubs());

        TestGroupDto? saved = null;
        var onSaved = EventCallback.Factory.Create<TestGroupDto>(this, g => saved = g);

        var cut = Render<TestGroupEditor>(p => p
            .Add(c => c.IsOpen, true)
            .Add(c => c.OnSaved, onSaved));

        cut.Find("input.rs-input").Input("Crew Alpha");

        cut.Find("button.rs-btn--primary").Click();

        Assert.Single(fake.RecordedTestGroupCreates);
        Assert.Equal("Crew Alpha", fake.RecordedTestGroupCreates[0].Name);
        Assert.NotNull(saved);
    }

    [Fact]
    public void Save_calls_UpdateTestGroupAsync_in_edit_mode_with_id_and_version()
    {
        var fake = RegisterFake(FakeClientService.WithStubs());

        var group = new TestGroupDto
        {
            TestGroupId = Guid.NewGuid(),
            Name = "Original",
            MemberIds = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() },
            Version = 5,
        };

        Guid? capturedId = null;
        TestGroupUpdate? capturedUpdate = null;
        int? capturedVersion = null;
        fake.OnUpdateTestGroup = (id, input, version) =>
        {
            capturedId = id;
            capturedUpdate = input;
            capturedVersion = version;
            return Task.FromResult(new TestGroupDto
            {
                TestGroupId = id,
                Name = input.Name,
                MemberIds = input.MemberIds.ToList(),
                Version = version + 1,
            });
        };

        var cut = Render<TestGroupEditor>(p => p
            .Add(c => c.IsOpen, true)
            .Add(c => c.TestGroup, group));

        cut.Find("input.rs-input").Input("Renamed");
        cut.Find("button.rs-btn--primary").Click();

        Assert.Equal(group.TestGroupId, capturedId);
        Assert.Equal(5, capturedVersion);
        Assert.NotNull(capturedUpdate);
        Assert.Equal("Renamed", capturedUpdate!.Name);
    }

    [Fact]
    public void ValidationException_is_surfaced_in_a_RuleViolationBanner()
    {
        var fake = RegisterFake();
        fake.OnCreateTestGroup = _ => throw new ValidationException("R21", "name taken");

        var cut = Render<TestGroupEditor>(p => p
            .Add(c => c.IsOpen, true));

        cut.Find("input.rs-input").Input("X");
        cut.Find("button.rs-btn--primary").Click();

        Assert.Contains("R21", cut.Markup);
        Assert.Contains("name taken", cut.Markup);
        Assert.Contains("rs-banner-rule", cut.Markup);
    }
}
