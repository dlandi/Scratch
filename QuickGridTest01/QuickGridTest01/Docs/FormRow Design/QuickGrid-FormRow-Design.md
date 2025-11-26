# QuickGrid Custom FormRow Design

## Document Information

| Attribute | Value |
|-----------|-------|
| Version | 1.0 |
| Created | November 2025 |
| Project | QuickGridTest01 |
| Target Framework | ASP.NET 9 Blazor Server |

---

## 1. Overview

### 1.1 Problem Statement

Traditional QuickGrid rendering constrains each row to a horizontal sequence of `<td>` cells. When editing data, users expect a form-like experience with:

- Logical field groupings
- Multi-column layouts within the form
- Labels positioned naturally with inputs
- Validation summaries
- Clear Save/Cancel actions

The existing `EditableColumn` provides per-cell editing but lacks the cohesive form experience users expect from a complete data entry interface.

### 1.2 Solution Concept

The **FormRowColumn** provides a "Form within a QuickGrid row" - a template-driven form that renders as a CSS card overlay when a row enters "form mode."

```
Normal Mode:
┌──────┬───────────┬───────────┬─────────────────┬─────────┐
│ ID   │ FirstName │ LastName  │ Email           │ Actions │
├──────┼───────────┼───────────┼─────────────────┼─────────┤
│ 1    │ John      │ Doe       │ john@email.com  │ [Edit]  │
├──────┼───────────┼───────────┼─────────────────┼─────────┤
│ 2    │ Jane      │ Smith     │ jane@email.com  │ [Edit]  │
└──────┴───────────┴───────────┴─────────────────┴─────────┘

Form Mode (Row 1 active, overlay):
┌──────┬───────────────────────────────────────────────────┐
│ 1    │ ╔═══════════════════════════════════════════════╗ │
│      │ ║  Edit Employee                                ║ │
│      │ ║  ┌─────────────────┐ ┌─────────────────┐     ║ │
│      │ ║  │ First Name      │ │ Last Name       │     ║ │
│      │ ║  │ [John_________] │ │ [Doe__________] │     ║ │
│      │ ║  └─────────────────┘ └─────────────────┘     ║ │
│      │ ║  ┌───────────────────────────────────────┐   ║ │
│      │ ║  │ Email                                 │   ║ │
│      │ ║  │ [john@email.com___________________]   │   ║ │
│      │ ║  └───────────────────────────────────────┘   ║ │
│      │ ║              [Save Changes]  [Cancel]        ║ │
│      │ ╚═══════════════════════════════════════════════╝ │
├──────┼───────────────────────────────────────────────────┤
│ 2    │ Jane      │ Smith     │ jane@email.com  │ [Edit]  │  (dimmed)
└──────┴───────────┴───────────┴─────────────────┴─────────┘
```

### 1.3 Key Design Decisions

| Decision | Choice | Rationale |
|----------|--------|-----------|
| Subclass QuickGrid? | **No** | Composition via FormRowColumn provides full functionality without framework coupling |
| Card Position | **Overlay** | Card overlays the row content, providing clear visual focus |
| Row Dimming | **Configurable** | When enabled, non-active rows are dimmed to focus attention |
| Trigger Mode | **Multiple options** | Button, RowClick, or Custom via DisplayTemplate |
| Concurrent Edit | **Configurable** | Block, CancelCurrent, SaveCurrent, or AllowMultiple |
| Form Authoring | **Declarative FormField** | Auto-wiring reduces boilerplate while allowing customization |

---

## 2. Architecture

### 2.1 Design Approach

The implementation uses **composition over inheritance**. FormRowColumn is a custom column that:

1. Manages which row(s) are in form mode
2. Provides cascading context to child form components
3. Renders either DisplayTemplate or FormTemplate based on state
4. Handles CSS overlay positioning

### 2.2 Component Hierarchy

```
QuickGrid<TGridItem>  (unchanged, standard QuickGrid)
└── FormRowColumn<TGridItem>  (custom column)
    ├── Manages: which row(s) are in form mode
    ├── Provides: CascadingValue<FormRowContext<TGridItem>>
    └── Renders: DisplayTemplate (normal) OR FormTemplate (form mode)
        └── FormCard
            ├── FormSection (optional grouping)
            │   └── FormField<TGridItem, TValue> (auto-wired inputs)
            └── FormActions (Save/Cancel buttons)
```

### 2.3 Namespace Organization

Following existing project conventions:

```
QuickGridTest01.FormRow/
├── Core/                           # Enums, state, context
│   ├── FormTriggerMode.cs
│   ├── ConcurrentEditBehavior.cs
│   ├── FormRowState.cs
│   ├── FormRowContext.cs
│   ├── FormRowDisplayContext.cs
│   └── FormDraftManager.cs
├── Events/                         # Event argument classes
│   ├── FormSaveEventArgs.cs
│   ├── FormCancelEventArgs.cs
│   ├── FormBeforeEditEventArgs.cs
│   └── FormStateChangedEventArgs.cs
├── Validation/                     # Field validation
│   ├── FormFieldValidator.cs
│   └── FormValidationResult.cs
├── Components/                     # UI components
│   ├── FormCard.razor
│   ├── FormCard.razor.css
│   ├── FormSection.razor
│   ├── FormField.cs
│   └── FormActions.razor
├── FormRowColumn.cs                # Main column component
├── FormRowStateManager.cs          # Active row tracking
└── FormRowColumn.razor.css         # Overlay & dimming styles
```

---

## 3. Core Enumerations

### 3.1 FormTriggerMode

Defines how form mode is activated for a row.

```csharp
namespace QuickGridTest01.FormRow.Core;

/// <summary>
/// Defines how the form mode is triggered for a row.
/// </summary>
public enum FormTriggerMode
{
    /// <summary>
    /// Renders an Edit button in the column cell. Click opens form.
    /// </summary>
    Button,
    
    /// <summary>
    /// Entire row is clickable. Click anywhere opens form.
    /// </summary>
    RowClick,
    
    /// <summary>
    /// Developer controls via DisplayTemplate. Use context.EnterFormModeAsync().
    /// </summary>
    Custom
}
```

### 3.2 ConcurrentEditBehavior

Defines behavior when user attempts to edit another row while one is already open.

```csharp
namespace QuickGridTest01.FormRow.Core;

/// <summary>
/// Defines behavior when user attempts to edit another row while one is already open.
/// </summary>
public enum ConcurrentEditBehavior
{
    /// <summary>
    /// Only one row can be in form mode. New edit blocked until current is closed.
    /// </summary>
    Block,
    
    /// <summary>
    /// Auto-cancel current edit (discard changes) and open new row.
    /// </summary>
    CancelCurrent,
    
    /// <summary>
    /// Auto-save current edit (if valid) and open new row.
    /// </summary>
    SaveCurrent,
    
    /// <summary>
    /// Allow multiple rows in form mode simultaneously.
    /// </summary>
    AllowMultiple
}
```

### 3.3 FormRowState

Tracks the current state of a row's form.

```csharp
namespace QuickGridTest01.FormRow.Core;

/// <summary>
/// The state of a row's form mode.
/// </summary>
public enum FormRowState
{
    /// <summary>
    /// Normal display mode, not editing.
    /// </summary>
    Reading,
    
    /// <summary>
    /// Form is open, user is editing.
    /// </summary>
    Editing,
    
    /// <summary>
    /// Save operation in progress.
    /// </summary>
    Saving
}
```

---

## 4. Context Objects

### 4.1 FormRowDisplayContext

Provided to `DisplayTemplate` for custom trigger rendering.

```csharp
namespace QuickGridTest01.FormRow.Core;

/// <summary>
/// Context provided to DisplayTemplate for custom trigger rendering.
/// </summary>
public class FormRowDisplayContext<TGridItem> where TGridItem : class
{
    /// <summary>
    /// The data item for this row.
    /// </summary>
    public TGridItem Item { get; init; } = default!;
    
    /// <summary>
    /// True if any row in the grid is currently in form mode.
    /// </summary>
    public bool IsAnyRowInFormMode { get; init; }
    
    /// <summary>
    /// True if this row can enter form mode (based on ConcurrentEditBehavior).
    /// </summary>
    public bool CanEnterFormMode { get; init; }
    
    /// <summary>
    /// Call to enter form mode for this row.
    /// </summary>
    public Func<Task> EnterFormModeAsync { get; init; } = default!;
}
```

### 4.2 FormRowContext

Cascaded to `FormTemplate` and all child `FormField` components.

```csharp
namespace QuickGridTest01.FormRow.Core;

/// <summary>
/// Context provided to FormTemplate and cascaded to FormField components.
/// Manages draft state, validation, and form actions.
/// </summary>
public class FormRowContext<TGridItem> where TGridItem : class
{
    /// <summary>
    /// The data item being edited.
    /// </summary>
    public TGridItem Item { get; init; } = default!;
    
    #region Draft State Management
    
    /// <summary>
    /// Gets the draft value for a property.
    /// </summary>
    public TValue GetDraft<TValue>(Expression<Func<TGridItem, TValue>> property);
    
    /// <summary>
    /// Sets the draft value for a property.
    /// </summary>
    public void SetDraft<TValue>(Expression<Func<TGridItem, TValue>> property, TValue value);
    
    /// <summary>
    /// Returns true if the specified property has been modified.
    /// </summary>
    public bool IsDirty<TValue>(Expression<Func<TGridItem, TValue>> property);
    
    /// <summary>
    /// Returns true if any field has been modified.
    /// </summary>
    public bool IsDirty();
    
    #endregion
    
    #region Validation
    
    /// <summary>
    /// Gets validation errors for a specific property.
    /// </summary>
    public IReadOnlyList<string> GetErrors<TValue>(Expression<Func<TGridItem, TValue>> property);
    
    /// <summary>
    /// Returns true if the specified property has validation errors.
    /// </summary>
    public bool HasErrors<TValue>(Expression<Func<TGridItem, TValue>> property);
    
    /// <summary>
    /// Returns true if any field has validation errors.
    /// </summary>
    public bool HasErrors();
    
    /// <summary>
    /// Validates all fields.
    /// </summary>
    public Task ValidateAsync();
    
    /// <summary>
    /// Validates a specific field.
    /// </summary>
    public Task ValidateFieldAsync<TValue>(Expression<Func<TGridItem, TValue>> property);
    
    #endregion
    
    #region State
    
    /// <summary>
    /// Current state of the form (Reading, Editing, Saving).
    /// </summary>
    public FormRowState State { get; }
    
    /// <summary>
    /// Convenience property: true when State == Saving.
    /// </summary>
    public bool IsSaving => State == FormRowState.Saving;
    
    /// <summary>
    /// Error message from last failed save attempt.
    /// </summary>
    public string? SaveError { get; }
    
    #endregion
    
    #region Actions
    
    /// <summary>
    /// Validates and saves the form.
    /// </summary>
    public Func<Task> SaveAsync { get; init; } = default!;
    
    /// <summary>
    /// Cancels editing and reverts to original values.
    /// </summary>
    public Func<Task> CancelAsync { get; init; } = default!;
    
    #endregion
}
```

---

## 5. FormRowColumn Component

### 5.1 Parameters

```csharp
namespace QuickGridTest01.FormRow;

public class FormRowColumn<TGridItem> : ColumnBase<TGridItem>, IDisposable
    where TGridItem : class
{
    #region Trigger & Behavior Parameters

    /// <summary>
    /// How form mode is triggered. Default: Button.
    /// </summary>
    [Parameter]
    public FormTriggerMode TriggerMode { get; set; } = FormTriggerMode.Button;

    /// <summary>
    /// Behavior when editing a new row while another is open. Default: Block.
    /// </summary>
    [Parameter]
    public ConcurrentEditBehavior ConcurrentEditBehavior { get; set; } = ConcurrentEditBehavior.Block;

    /// <summary>
    /// When true, non-active rows are visually dimmed. Default: true.
    /// </summary>
    [Parameter]
    public bool DimInactiveRows { get; set; } = true;

    #endregion

    #region Templates

    /// <summary>
    /// Content shown when row is NOT in form mode.
    /// If null and TriggerMode is Button, renders default Edit button.
    /// If null and TriggerMode is RowClick, renders nothing.
    /// If null and TriggerMode is Custom, throws at runtime.
    /// </summary>
    [Parameter]
    public RenderFragment<FormRowDisplayContext<TGridItem>>? DisplayTemplate { get; set; }

    /// <summary>
    /// The form layout rendered when row IS in form mode. Required.
    /// </summary>
    [Parameter, EditorRequired]
    public RenderFragment? FormTemplate { get; set; }

    #endregion

    #region Button Customization (when TriggerMode = Button)

    /// <summary>
    /// Text for the edit button. Default: "Edit".
    /// </summary>
    [Parameter]
    public string EditButtonText { get; set; } = "Edit";

    /// <summary>
    /// CSS class for the edit button. Default: "qg-btn qg-btn-secondary".
    /// </summary>
    [Parameter]
    public string EditButtonClass { get; set; } = "qg-btn qg-btn-secondary";

    /// <summary>
    /// Icon class for the edit button (prepended). Default: "bi bi-pencil".
    /// Set to null to hide icon.
    /// </summary>
    [Parameter]
    public string? EditButtonIcon { get; set; } = "bi bi-pencil";

    #endregion

    #region Events

    /// <summary>
    /// Called when save is requested. Return (true, null) for success.
    /// Return (false, "error message") for failure.
    /// If null, draft values are written directly to Item properties.
    /// </summary>
    [Parameter]
    public Func<TGridItem, Task<(bool Success, string? Error)>>? OnSaveAsync { get; set; }

    /// <summary>
    /// Called after successful save.
    /// </summary>
    [Parameter]
    public EventCallback<FormSaveEventArgs<TGridItem>> OnSaved { get; set; }

    /// <summary>
    /// Called when edit is cancelled.
    /// </summary>
    [Parameter]
    public EventCallback<FormCancelEventArgs<TGridItem>> OnCancelled { get; set; }

    /// <summary>
    /// Called before entering form mode. Set Cancel = true to prevent.
    /// </summary>
    [Parameter]
    public EventCallback<FormBeforeEditEventArgs<TGridItem>> OnBeforeEdit { get; set; }

    /// <summary>
    /// Called when form mode state changes (enter/exit).
    /// </summary>
    [Parameter]
    public EventCallback<FormStateChangedEventArgs<TGridItem>> OnFormStateChanged { get; set; }

    #endregion

    #region Validation

    /// <summary>
    /// Enable DataAnnotations validation on form fields. Default: true.
    /// </summary>
    [Parameter]
    public bool UseDataAnnotations { get; set; } = true;

    /// <summary>
    /// Validate fields on every input change. Default: true.
    /// If false, validation runs only on save attempt.
    /// </summary>
    [Parameter]
    public bool ValidateOnChange { get; set; } = true;

    #endregion
}
```

### 5.2 Internal State Management

The `FormRowColumn` uses a `FormRowStateManager` internally to track:

- Which row(s) are currently in form mode
- Draft values for each active row
- Validation state per field
- Dirty tracking

Memory management uses `ConditionalWeakTable<TGridItem, FormRowContext<TGridItem>>` to allow automatic cleanup when grid items are garbage collected.

---

## 6. FormField Component

### 6.1 Auto-Wiring Behavior

The `FormField` component automatically handles:

| Concern | Auto-Wired Behavior |
|---------|---------------------|
| **Label** | Extracts from property name or `[Display(Name="...")]` attribute |
| **Input Type** | Infers from property type (`string` → text, `DateTime` → date, `bool` → checkbox, etc.) |
| **Draft Binding** | Binds to `FormRowContext.GetDraft()` / `SetDraft()` |
| **Validation** | Discovers DataAnnotations (`[Required]`, `[EmailAddress]`, etc.) |
| **Error Display** | Renders validation messages from context |
| **Dirty Tracking** | Visual indicator when field is modified |
| **Format/Parse** | Uses `[DisplayFormat]` or type-appropriate formatting |

### 6.2 Parameters

```csharp
namespace QuickGridTest01.FormRow.Components;

public class FormField<TGridItem, TValue> : ComponentBase
    where TGridItem : class
{
    /// <summary>
    /// Cascaded form context (auto-provided by FormRowColumn).
    /// </summary>
    [CascadingParameter]
    public FormRowContext<TGridItem> Context { get; set; } = default!;

    /// <summary>
    /// The property to bind. Required.
    /// </summary>
    [Parameter, EditorRequired]
    public Expression<Func<TGridItem, TValue>> Property { get; set; } = default!;

    /// <summary>
    /// Override the auto-detected label.
    /// </summary>
    [Parameter]
    public string? Label { get; set; }

    /// <summary>
    /// Placeholder text for the input.
    /// </summary>
    [Parameter]
    public string? Placeholder { get; set; }

    /// <summary>
    /// Override the auto-detected input type.
    /// </summary>
    [Parameter]
    public string? InputType { get; set; }

    /// <summary>
    /// Make the field read-only.
    /// </summary>
    [Parameter]
    public bool ReadOnly { get; set; }

    /// <summary>
    /// Additional validators beyond DataAnnotations.
    /// </summary>
    [Parameter]
    public IEnumerable<IValidator<TValue>>? Validators { get; set; }

    /// <summary>
    /// Custom input template for advanced scenarios.
    /// </summary>
    [Parameter]
    public RenderFragment<FormFieldContext<TValue>>? InputTemplate { get; set; }

    /// <summary>
    /// CSS class applied to the field wrapper.
    /// </summary>
    [Parameter]
    public string? Class { get; set; }
}
```

### 6.3 Input Type Inference

```csharp
private static string InferInputType(Type type)
{
    var t = Nullable.GetUnderlyingType(type) ?? type;
    
    return t switch
    {
        _ when t == typeof(string) => "text",
        _ when t == typeof(int) || t == typeof(long) || 
               t == typeof(decimal) || t == typeof(double) || 
               t == typeof(float) => "number",
        _ when t == typeof(bool) => "checkbox",
        _ when t == typeof(DateTime) || t == typeof(DateOnly) => "date",
        _ when t == typeof(TimeOnly) => "time",
        _ when t.IsEnum => "select",
        _ => "text"
    };
}
```

---

## 7. Supporting Components

### 7.1 FormCard

Container component providing card styling.

```razor
@namespace QuickGridTest01.FormRow.Components

<div class="form-card @Class">
    @if (!string.IsNullOrEmpty(Title))
    {
        <div class="form-card-header">
            <h3 class="form-card-title">@Title</h3>
        </div>
    }
    <div class="form-card-body">
        @ChildContent
    </div>
</div>

@code {
    [Parameter] public string? Title { get; set; }
    [Parameter] public string? Class { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
}
```

### 7.2 FormSection

Groups related fields with optional title.

```razor
@namespace QuickGridTest01.FormRow.Components

<div class="form-section @Class">
    @if (!string.IsNullOrEmpty(Title))
    {
        <h4 class="form-section-title">@Title</h4>
    }
    <div class="form-section-content">
        @ChildContent
    </div>
</div>

@code {
    [Parameter] public string? Title { get; set; }
    [Parameter] public string? Class { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
}
```

### 7.3 FormActions

Provides Save/Cancel buttons wired to context.

```razor
@namespace QuickGridTest01.FormRow.Components
@typeparam TGridItem where TGridItem : class

<div class="form-actions @Class">
    <button type="button" 
            class="@SaveButtonClass" 
            @onclick="Context.SaveAsync"
            disabled="@(Context.IsSaving || (DisableIfInvalid && Context.HasErrors()))">
        @if (Context.IsSaving)
        {
            <span class="spinner"></span>
            <span>@SavingText</span>
        }
        else
        {
            @if (!string.IsNullOrEmpty(SaveIcon))
            {
                <i class="@SaveIcon"></i>
            }
            <span>@SaveText</span>
        }
    </button>
    
    <button type="button" 
            class="@CancelButtonClass" 
            @onclick="Context.CancelAsync"
            disabled="@Context.IsSaving">
        @if (!string.IsNullOrEmpty(CancelIcon))
        {
            <i class="@CancelIcon"></i>
        }
        <span>@CancelText</span>
    </button>
</div>

@code {
    [CascadingParameter]
    public FormRowContext<TGridItem> Context { get; set; } = default!;

    [Parameter] public string SaveText { get; set; } = "Save";
    [Parameter] public string SavingText { get; set; } = "Saving...";
    [Parameter] public string CancelText { get; set; } = "Cancel";
    [Parameter] public string? SaveIcon { get; set; } = "bi bi-check";
    [Parameter] public string? CancelIcon { get; set; } = "bi bi-x";
    [Parameter] public string SaveButtonClass { get; set; } = "qg-btn qg-btn-primary";
    [Parameter] public string CancelButtonClass { get; set; } = "qg-btn qg-btn-secondary";
    [Parameter] public bool DisableIfInvalid { get; set; } = true;
    [Parameter] public string? Class { get; set; }
}
```

---

## 8. Event Arguments

```csharp
namespace QuickGridTest01.FormRow.Events;

public class FormSaveEventArgs<TGridItem> where TGridItem : class
{
    public TGridItem Item { get; init; } = default!;
    public Dictionary<string, object?> SavedValues { get; init; } = new();
}

public class FormCancelEventArgs<TGridItem> where TGridItem : class
{
    public TGridItem Item { get; init; } = default!;
    public bool WasDirty { get; init; }
}

public class FormBeforeEditEventArgs<TGridItem> where TGridItem : class
{
    public TGridItem Item { get; init; } = default!;
    public bool Cancel { get; set; }
}

public class FormStateChangedEventArgs<TGridItem> where TGridItem : class
{
    public TGridItem Item { get; init; } = default!;
    public FormRowState OldState { get; init; }
    public FormRowState NewState { get; init; }
}
```

---

## 9. CSS Design

### 9.1 Overlay Positioning

The form card uses absolute positioning relative to the row:

```css
/* Applied to QuickGrid when FormRowColumn is present */
.qg-form-grid tbody tr {
    position: relative;
}

/* The form card overlay */
.form-card {
    position: absolute;
    top: 0;
    left: 0;
    right: 0;
    min-height: 100%;
    z-index: 10;
    
    background: var(--color-surface);
    border: 1px solid var(--color-border-emphasis);
    border-radius: var(--card-radius);
    box-shadow: var(--shadow-lg);
    padding: var(--space-16);
}
```

### 9.2 Row Dimming

```css
/* When a row is in form mode, dim other rows */
.qg-form-grid.has-active-form tbody tr:not(.form-active) {
    opacity: 0.5;
    pointer-events: none;
    transition: opacity var(--duration-normal) var(--ease-in-out);
}

.qg-form-grid.has-active-form tbody tr.form-active {
    z-index: 11;
}
```

### 9.3 Form Layout

```css
.form-section {
    margin-bottom: var(--space-16);
}

.form-section-title {
    font-size: var(--font-size-sm);
    font-weight: var(--font-weight-medium);
    text-transform: uppercase;
    letter-spacing: var(--letter-spacing-wide);
    color: var(--color-text-tertiary);
    margin-bottom: var(--space-8);
    padding-bottom: var(--space-4);
    border-bottom: 1px solid var(--color-border-default);
}

.form-section-content {
    display: grid;
    grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));
    gap: var(--space-12);
}

.form-actions {
    display: flex;
    justify-content: flex-end;
    gap: var(--space-8);
    margin-top: var(--space-16);
    padding-top: var(--space-16);
    border-top: 1px solid var(--color-border-default);
}
```

---

## 10. Usage Examples

### 10.1 Default Button Trigger

```razor
<QuickGrid Items="@employees" Class="qg-form-grid">
    <PropertyColumn Property="@(e => e.Id)" />
    <PropertyColumn Property="@(e => e.FirstName)" />
    <PropertyColumn Property="@(e => e.LastName)" />
    <PropertyColumn Property="@(e => e.Email)" />
    
    <FormRowColumn TGridItem="Employee" 
                   TriggerMode="FormTriggerMode.Button"
                   OnSaveAsync="@SaveEmployeeAsync">
        <FormTemplate>
            <FormCard Title="Edit Employee">
                <FormSection>
                    <FormField Property="@(e => e.FirstName)" />
                    <FormField Property="@(e => e.LastName)" />
                </FormSection>
                <FormSection Title="Contact Information">
                    <FormField Property="@(e => e.Email)" />
                    <FormField Property="@(e => e.Phone)" />
                </FormSection>
                <FormActions />
            </FormCard>
        </FormTemplate>
    </FormRowColumn>
</QuickGrid>

@code {
    private async Task<(bool, string?)> SaveEmployeeAsync(Employee employee)
    {
        try
        {
            await _employeeService.UpdateAsync(employee);
            return (true, null);
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }
}
```

### 10.2 Row Click with Auto-Save

```razor
<FormRowColumn TGridItem="Employee"
               TriggerMode="FormTriggerMode.RowClick"
               ConcurrentEditBehavior="ConcurrentEditBehavior.SaveCurrent"
               DimInactiveRows="true"
               OnSaveAsync="@SaveEmployeeAsync">
    <FormTemplate>
        <FormCard Title="Quick Edit">
            <FormField Property="@(e => e.Email)" />
            <FormField Property="@(e => e.Phone)" />
            <FormActions SaveText="Update" />
        </FormCard>
    </FormTemplate>
</FormRowColumn>
```

### 10.3 Custom DisplayTemplate

```razor
<FormRowColumn TGridItem="Employee"
               TriggerMode="FormTriggerMode.Custom"
               ConcurrentEditBehavior="ConcurrentEditBehavior.Block"
               OnSaveAsync="@SaveEmployeeAsync">
    <DisplayTemplate Context="ctx">
        <div class="action-buttons">
            <button class="qg-btn qg-btn-primary qg-btn-sm" 
                    @onclick="ctx.EnterFormModeAsync"
                    disabled="@(!ctx.CanEnterFormMode)">
                <i class="bi bi-pencil-square"></i> Edit
            </button>
            <button class="qg-btn qg-btn-ghost qg-btn-sm" 
                    @onclick="() => DeleteEmployee(ctx.Item)">
                <i class="bi bi-trash"></i>
            </button>
        </div>
    </DisplayTemplate>
    <FormTemplate>
        <FormCard Title="Edit Employee Details">
            <FormSection Title="Personal">
                <FormField Property="@(e => e.FirstName)" />
                <FormField Property="@(e => e.LastName)" />
                <FormField Property="@(e => e.DateOfBirth)" />
            </FormSection>
            <FormSection Title="Contact">
                <FormField Property="@(e => e.Email)" />
                <FormField Property="@(e => e.Phone)" Placeholder="555-123-4567" />
            </FormSection>
            <FormSection Title="Employment">
                <FormField Property="@(e => e.Department)" />
                <FormField Property="@(e => e.HireDate)" />
                <FormField Property="@(e => e.Salary)" />
            </FormSection>
            <FormActions SaveText="Save Changes" CancelText="Discard" />
        </FormCard>
    </FormTemplate>
</FormRowColumn>
```

### 10.4 Multi-Edit Mode

```razor
<FormRowColumn TGridItem="Employee"
               TriggerMode="FormTriggerMode.Button"
               ConcurrentEditBehavior="ConcurrentEditBehavior.AllowMultiple"
               DimInactiveRows="false"
               EditButtonText=""
               EditButtonIcon="bi bi-pencil"
               OnSaveAsync="@SaveEmployeeAsync">
    <FormTemplate>
        <FormCard>
            <FormField Property="@(e => e.Email)" />
            <FormActions />
        </FormCard>
    </FormTemplate>
</FormRowColumn>
```

### 10.5 Custom FormField Input

```razor
<FormField Property="@(e => e.Status)">
    <InputTemplate Context="field">
        <select class="qg-select" 
                value="@field.Value" 
                @onchange="field.OnChange"
                disabled="@field.IsDisabled">
            @foreach (var status in Enum.GetValues<EmployeeStatus>())
            {
                <option value="@status">@status.GetDisplayName()</option>
            }
        </select>
    </InputTemplate>
</FormField>
```

---

## 11. Implementation Considerations

### 11.1 Performance

- Use `ConditionalWeakTable` for state storage to avoid memory leaks
- Compile property expressions once and cache
- Minimize re-renders through targeted `StateHasChanged()` calls
- Consider virtualization compatibility for large grids

### 11.2 Accessibility

- Form card should trap focus when open
- Escape key closes form (cancels)
- Enter key in last field submits form
- ARIA attributes for form state
- Screen reader announcements for save/cancel

### 11.3 Keyboard Navigation

| Key | Action |
|-----|--------|
| Tab | Move between form fields |
| Shift+Tab | Move backwards |
| Enter | Submit form (when in last field or on button) |
| Escape | Cancel and close form |

### 11.4 Validation

- DataAnnotations discovered via reflection (cached)
- Custom validators via `IValidator<TValue>` interface
- Cross-field validation supported via `FormRowContext.ValidateAsync()` override
- Error messages displayed per-field and/or in summary

---

## 12. Future Enhancements

Potential future extensions (not in initial implementation):

1. **Expandable Form Row (Option A)**: Alternative rendering that pushes content down instead of overlay
2. **Nested Forms**: Support for editing related entities within the same form
3. **Form Wizard**: Multi-step forms within a row
4. **Undo/Redo**: Track changes and allow reverting individual fields
5. **Batch Editing**: Select multiple rows and edit common fields
6. **Template Library**: Pre-built form layouts for common scenarios
7. **Mobile Responsive**: Full-screen form overlay on small screens

---

## 13. Glossary

| Term | Definition |
|------|------------|
| **Form Mode** | State where a row displays the form template instead of normal cell content |
| **Draft State** | Temporary copy of field values being edited, not yet committed |
| **Dirty** | A field or form where draft values differ from original |
| **FormRowContext** | Cascading context providing state and actions to form components |
| **Auto-Wire** | Automatic configuration of FormField based on property metadata |

---

## 14. References

- QuickGrid Monograph (quickgrid-monograph-v1.pdf)
- Existing EditableColumn implementation
- Existing MultiStateColumn implementation
- QuickGrid Design System (quickgrid-refined-minimalism.css)
