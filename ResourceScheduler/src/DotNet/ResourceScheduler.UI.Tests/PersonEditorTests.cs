using Bunit;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using ResourceScheduler.Components.Components;
using ResourceScheduler.Components.Models;
using ResourceScheduler.Components.Services;
using ResourceScheduler.UI.Tests.Fakes;
using Xunit;

namespace ResourceScheduler.UI.Tests;

public class PersonEditorTests : BunitContext
{
    private FakeClientService RegisterFake(FakeClientService? fake = null)
    {
        var f = fake ?? new FakeClientService();
        Services.AddSingleton<IClientService>(f);
        return f;
    }

    [Fact]
    public void Title_is_New_person_when_Person_is_null()
    {
        RegisterFake();

        var cut = Render<PersonEditor>(p => p
            .Add(c => c.IsOpen, true));

        Assert.Contains("New person", cut.Markup);
    }

    [Fact]
    public void Title_is_Edit_person_when_Person_is_provided()
    {
        RegisterFake();

        var person = new PersonDto
        {
            PersonId = Guid.NewGuid(),
            Name = "Aoife",
            Email = "aoife@example.com",
        };

        var cut = Render<PersonEditor>(p => p
            .Add(c => c.IsOpen, true)
            .Add(c => c.Person, person));

        Assert.Contains("Edit person", cut.Markup);
    }

    [Fact]
    public void Pre_fills_fields_in_edit_mode()
    {
        RegisterFake();

        var person = new PersonDto
        {
            PersonId = Guid.NewGuid(),
            Name = "Aoife",
            Email = "aoife@example.com",
        };

        var cut = Render<PersonEditor>(p => p
            .Add(c => c.IsOpen, true)
            .Add(c => c.Person, person));

        var inputs = cut.FindAll("input.rs-input");
        Assert.Equal("Aoife", inputs[0].GetAttribute("value"));
        Assert.Equal("aoife@example.com", inputs[1].GetAttribute("value"));
    }

    [Fact]
    public void Save_button_disabled_when_Name_is_empty()
    {
        RegisterFake();

        var cut = Render<PersonEditor>(p => p
            .Add(c => c.IsOpen, true));

        var saveButton = cut.Find("button.rs-btn--primary");
        Assert.True(saveButton.HasAttribute("disabled"));
    }

    [Fact]
    public void Save_calls_CreatePersonAsync_with_form_input()
    {
        var fake = RegisterFake(FakeClientService.WithStubs());

        var cut = Render<PersonEditor>(p => p
            .Add(c => c.IsOpen, true));

        // Re-query after each input event: each Input() triggers a re-render
        // and stale handler ids would throw UnknownEventHandlerIdException.
        cut.FindAll("input.rs-input")[0].Input("Maeve");
        cut.FindAll("input.rs-input")[1].Input("maeve@example.com");

        cut.Find("button.rs-btn--primary").Click();

        Assert.Single(fake.RecordedPersonCreates);
        Assert.Equal("Maeve", fake.RecordedPersonCreates[0].Name);
        Assert.Equal("maeve@example.com", fake.RecordedPersonCreates[0].Email);
    }

    [Fact]
    public void Save_calls_UpdatePersonAsync_in_edit_mode()
    {
        var fake = RegisterFake(FakeClientService.WithStubs());

        var person = new PersonDto
        {
            PersonId = Guid.NewGuid(),
            Name = "Old Name",
            Email = "old@example.com",
        };

        Guid? capturedId = null;
        PersonUpdate? capturedUpdate = null;
        fake.OnUpdatePerson = (id, input) =>
        {
            capturedId = id;
            capturedUpdate = input;
            return Task.FromResult(new PersonDto
            {
                PersonId = id,
                Name = input.Name,
                Email = input.Email,
            });
        };

        var cut = Render<PersonEditor>(p => p
            .Add(c => c.IsOpen, true)
            .Add(c => c.Person, person));

        var inputs = cut.FindAll("input.rs-input");
        inputs[0].Input("New Name");

        cut.Find("button.rs-btn--primary").Click();

        Assert.Equal(person.PersonId, capturedId);
        Assert.NotNull(capturedUpdate);
        Assert.Equal("New Name", capturedUpdate!.Name);
    }

    [Fact]
    public void Email_validation_blocks_save_when_email_is_non_empty_without_at_sign()
    {
        // PersonEditor does not disable the Save button for a malformed
        // email: it short-circuits inside Save() and sets an inline error
        // rather than calling the client. Assert the actual behavior.
        var fake = RegisterFake(FakeClientService.WithStubs());

        var cut = Render<PersonEditor>(p => p
            .Add(c => c.IsOpen, true));

        cut.FindAll("input.rs-input")[0].Input("Maeve");
        cut.FindAll("input.rs-input")[1].Input("lab.example");

        cut.Find("button.rs-btn--primary").Click();

        Assert.Empty(fake.RecordedPersonCreates);
        Assert.Contains("@", cut.Markup);
        Assert.Contains("Email must contain", cut.Markup);
    }

    [Fact]
    public void ValidationException_is_surfaced_in_a_RuleViolationBanner()
    {
        var fake = RegisterFake();
        fake.OnCreatePerson = _ => throw new ValidationException("R20", "name taken");

        var cut = Render<PersonEditor>(p => p
            .Add(c => c.IsOpen, true));

        cut.Find("input.rs-input").Input("Maeve");
        cut.Find("button.rs-btn--primary").Click();

        Assert.Contains("R20", cut.Markup);
        Assert.Contains("name taken", cut.Markup);
        Assert.Contains("rs-banner-rule", cut.Markup);
    }
}
