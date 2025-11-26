# FormRow Column Implementation

## Overview

The FormRowColumn provides inline form editing within QuickGrid rows, rendering a developer-defined form template as a CSS card overlay when a row enters edit mode.

## File Structure

```
FormRow/
├── Core/
│   ├── FormTriggerMode.cs           (22 lines)   - Enum: Button, RowClick, Custom
│   ├── ConcurrentEditBehavior.cs    (27 lines)   - Enum: Block, CancelCurrent, SaveCurrent, AllowMultiple
│   ├── FormRowState.cs              (22 lines)   - Enum: Reading, Editing, Saving
│   ├── FormRowDisplayContext.cs     (28 lines)   - Context for DisplayTemplate
│   ├── FormRowContext.cs            (295 lines)  - Main context with draft state, validation
│   └── FormRowStateManager.cs       (140 lines)  - Tracks active rows
├── Events/
│   └── FormRowEventArgs.cs          (76 lines)   - Event argument classes
├── Validation/
│   └── FormFieldValidator.cs        (150 lines)  - DataAnnotations + custom validators
├── Components/
│   ├── FormCard.razor               (57 lines)   - Card wrapper with title
│   ├── FormSection.razor            (37 lines)   - Field grouping with columns
│   ├── FormField.cs                 (524 lines)  - Auto-wiring input component
│   └── FormActions.razor            (170 lines)  - Save/Cancel/Reset buttons
├── FormRowColumn.cs                 (473 lines)  - Main column component
├── FormRowColumn.razor.css          (403 lines)  - Overlay and form styles
├── FormRowDemo.razor                (368 lines)  - Demo page
└── FormRowDemo.razor.css            (131 lines)  - Demo styles

Total: ~2,923 lines of code
```

## Installation

1. Copy the `FormRow` folder to your project
2. Add namespace imports to `_Imports.razor`:

```razor
@using QuickGridTest01.FormRow
@using QuickGridTest01.FormRow.Core
@using QuickGridTest01.FormRow.Components
```

3. Reference the CSS in your layout or include in your CSS bundle

## Quick Start

```razor
<QuickGrid Items="@employees" Class="qg-grid qg-form-grid">
    <PropertyColumn Property="@(e => e.Id)" />
    <PropertyColumn Property="@(e => e.FirstName)" />
    <PropertyColumn Property="@(e => e.LastName)" />
    
    <FormRowColumn TGridItem="Employee" OnSaveAsync="@SaveAsync">
        <FormTemplate>
            <FormCard Title="Edit Employee">
                <FormSection>
                    <FormField TGridItem="Employee" TValue="string" Property="@(e => e.FirstName)" />
                    <FormField TGridItem="Employee" TValue="string" Property="@(e => e.LastName)" />
                </FormSection>
                <FormActions TGridItem="Employee" />
            </FormCard>
        </FormTemplate>
    </FormRowColumn>
</QuickGrid>

@code {
    private async Task<(bool, string?)> SaveAsync(Employee employee)
    {
        await _service.UpdateAsync(employee);
        return (true, null);
    }
}
```

## Key Features

### Trigger Modes

| Mode | Description |
|------|-------------|
| `Button` | Default Edit button in cell |
| `RowClick` | Click anywhere on row |
| `Custom` | Developer-defined via DisplayTemplate |

### Concurrent Edit Behavior

| Behavior | Description |
|----------|-------------|
| `Block` | Only one row editable at a time (default) |
| `CancelCurrent` | Auto-cancel current, open new |
| `SaveCurrent` | Auto-save current (if valid), open new |
| `AllowMultiple` | Multiple rows editable simultaneously |

### FormField Auto-Wiring

FormField automatically discovers from property metadata:
- **Label**: From `[Display(Name="...")]` or property name
- **Input Type**: Inferred from property type
- **Validation**: DataAnnotations (`[Required]`, `[EmailAddress]`, etc.)
- **Placeholder**: From `[Display(Prompt="...")]`

### Events

| Event | Description |
|-------|-------------|
| `OnBeforeEdit` | Before entering form mode (cancellable) |
| `OnSaveAsync` | Save handler, return `(success, errorMessage)` |
| `OnSaved` | After successful save |
| `OnCancelled` | After edit cancelled |
| `OnFormStateChanged` | State transitions |

## Components

### FormCard

```razor
<FormCard Title="Edit Record">
    @* Form content *@
</FormCard>
```

### FormSection

```razor
<FormSection Title="Contact Info" Columns="2">
    <FormField ... />
    <FormField ... />
</FormSection>
```

### FormField

```razor
@* Basic usage - auto-wires everything *@
<FormField TGridItem="Employee" TValue="string" Property="@(e => e.Email)" />

@* Custom label and placeholder *@
<FormField TGridItem="Employee" TValue="string" 
           Property="@(e => e.Email)" 
           Label="Work Email"
           Placeholder="name@company.com" />

@* Textarea *@
<FormField TGridItem="Employee" TValue="string" 
           Property="@(e => e.Notes)" 
           InputType="textarea" 
           Rows="4" />

@* Custom input template *@
<FormField TGridItem="Employee" TValue="Status" Property="@(e => e.Status)">
    <InputTemplate Context="field">
        <select class="qg-select" @onchange="field.OnChange">
            @foreach (var status in Enum.GetValues<Status>())
            {
                <option value="@status">@status</option>
            }
        </select>
    </InputTemplate>
</FormField>
```

### FormActions

```razor
<FormActions TGridItem="Employee" 
             SaveText="Update" 
             CancelText="Discard"
             ShowResetButton="true" />
```

## CSS Classes

| Class | Description |
|-------|-------------|
| `qg-form-grid` | Add to QuickGrid for proper positioning |
| `form-row-overlay` | The overlay card container |
| `form-card` | Card styling |
| `form-section` | Section grouping |
| `form-field` | Field wrapper |
| `form-field.dirty` | Field has been modified |
| `form-field.has-errors` | Field has validation errors |
| `form-actions` | Button container |

## Dependencies

- Microsoft.AspNetCore.Components.QuickGrid
- QuickGridTest01.CustomColumns (for IValidator<T>)
- Bootstrap Icons (bi bi-* classes) - optional, customize via parameters

## Browser Support

- Modern browsers with CSS Grid and custom properties support
- Responsive design adjusts for mobile viewports
