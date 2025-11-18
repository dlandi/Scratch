using Bunit;
using Microsoft.AspNetCore.Components;
using QuickGridTest01.MultiState.Demo;
using Xunit;

namespace QuickGridTest01.MultiState.Tests.Integration;

/// <summary>
/// Integration tests verifying MultiStateColumn rendering expectations (limited by BUnit capability).
/// </summary>
public class MultiStateColumnIntegrationTests : TestContext
{
    [Fact(Skip = "BUnit cannot fully render QuickGrid - use Playwright for this test")]
    public async Task CRITICAL_MultiStateColumns_MustRenderInGrid()
    {
        var service = new ContactService();
        await service.SeedDataAsync();
        var cut = RenderComponent<QuickGridTest01.Pages.MultiStateColumnDemo>();
        var markup = cut.Markup;
        Assert.Contains("ID", markup);
        Assert.Contains("Name", markup);
        Assert.Contains("Email", markup);
        Assert.Contains("Phone", markup);
        Assert.Contains("Company", markup);
        Assert.Contains("Active", markup);
        Assert.Contains("Created", markup);
        Assert.Contains("multistate-cell", markup);
        Assert.Contains("cell-reading", markup);
    }

    [Fact]
    public void ComponentType_IsInCorrectNamespace()
    {
        var columnType = typeof(MultiStateColumn<Contact, string>);
        Assert.Equal("QuickGridTest01.MultiState", columnType.Namespace);
    }

    [Fact]
    public void ComponentType_InheritsFromColumnBase()
    {
        var columnType = typeof(MultiStateColumn<Contact, string>);
        var baseType = columnType.BaseType;
        Assert.NotNull(baseType);
        Assert.Equal("ColumnBase`1", baseType?.Name);
    }

    [Fact]
    public async Task ContactService_ProducesValidData()
    {
        var service = new ContactService();
        await service.SeedDataAsync();
        var contacts = await service.GetAllAsync();
        Assert.NotNull(contacts);
        Assert.True(contacts.Count >= 10);
        var first = contacts.First();
        Assert.True(first.Id > 0);
        Assert.False(string.IsNullOrEmpty(first.Name));
        Assert.False(string.IsNullOrEmpty(first.Email));
        Assert.False(string.IsNullOrEmpty(first.Phone));
        Assert.False(string.IsNullOrEmpty(first.Company));
    }
}

/// <summary>
/// Manual test instructions for verifying MultiStateColumn rendering in a real browser.
/// </summary>
public class ManualTestInstructions
{
    /*
     * MANUAL TEST CHECKLIST (Updated)
     * =====================
     * 1. Start app: dotnet run --project QuickGridTest01
     * 2. Navigate to /multistate-demo
     * 3. Verify all columns: ID, Name, Email, Phone, Company, Active, Created
     * 4. Inline editors present for editable columns (no explicit edit button in current design)
     * 5. Invalid input shows validation errors; Save disabled or prevented
     * 6. Valid input saves; loading state visible; event log updates
     * 7. Event Log shows lifecycle events (BEFORE_EDIT, VALUE_CHANGING, SAVE_SUCCESS, etc.)
     * 8. Statistics counters update (Successful Saves, Validation Errors, Cancelled Edits)
     */
}
