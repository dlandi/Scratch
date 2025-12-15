# Features


This is the description of what the code block changes:
Create a comprehensive features matrix documenting all custom QuickGrid columns and their capabilities

This is the code block that represents the suggested code change:


# Custom QuickGrid Columns - Features Matrix

This document provides a comprehensive overview of all custom QuickGrid column implementations in this application, comparing their features and capabilities.

---

## Column Overview

| Column | Purpose | Namespace |
|--------|---------|-----------|
| **FormRowColumn** | Inline form editing with full row overlay | `QuickGridTest01.FormRow` |
| **EditableColumn** | Inline cell editing with validation | `QuickGridTest01.CustomColumns` |
| **MultiStateColumn** | Cell editing with explicit state machine (Reading/Editing/Loading) | `QuickGridTest01.MultiState` |
| **FilterableColumn** | Column with built-in filtering UI | `QuickGridTest01.Filterable` |
| **ConditionalStyleColumn** | Dynamic styling based on value rules | `QuickGridTest01.ConditionalStyling` |
| **FormattedValueColumn** | Value formatting with culture support | `QuickGridTest01.FormattedValue.Component` |
| **IconColumn** | Icon display with value mapping | `QuickGridTest01.CustomColumns` |
| **OptimizedColumn** | Performance-optimized rendering | `QuickGridTest01.CustomColumns` |
| **VirtualColumn** | Optimized for virtual scrolling | `QuickGridTest01.CustomColumns` |
| **RowColumn** | Expandable row overlay with spacer row support | `QuickGridTest01.RowColumn` |

---

## Feature Comparison Matrix

### Core Features

| Feature | FormRow | Editable | MultiState | Filterable | ConditionalStyle | FormattedValue | Icon | Optimized | Virtual | RowColumn |
|---------|:-------:|:--------:|:----------:|:----------:|:----------------:|:--------------:|:----:|:---------:|:-------:|:---------:|
| **Property Expression** | ? | ? | ? | ? | ? | ? | ? | ? | ? | ? |
| **Compiled Accessor** | ? | ? | ? | ? | ? | ? | ? | ? | ? | ? |
| **TypeTraits Integration** | ? | ? | ? | ? | ? | ? | ? | ? | ? | ? |
| **Auto Title Inference** | ? | ? | ? | ? | ? | ? | ? | ? | ? | ? |
| **Sortable** | ? | ? | ? | ? | ? | ? | ? | ? | ? | ? |
| **IDisposable** | ? | ? | ? | ? | ? | ? | ? | ? | ? | ? |

### Editing Features

| Feature | FormRow | Editable | MultiState | Filterable | ConditionalStyle | FormattedValue | Icon | Optimized | Virtual | RowColumn |
|---------|:-------:|:--------:|:----------:|:----------:|:----------------:|:--------------:|:----:|:---------:|:-------:|:---------:|
| **Inline Editing** | ? | ? | ? | ? | ? | ? | ? | ? | ? | ? |
| **Form Overlay Editing** | ? | ? | ? | ? | ? | ? | ? | ? | ? | ? |
| **Edit State Management** | ? | ? | ? | ? | ? | ? | ? | ? | ? | ? |
| **Multiple Edit Modes** | ? | ? | ? | ? | ? | ? | ? | ? | ? | ? |
| **Read-Only Mode** | ? | ? | ? | ? | ? | ? | ? | ? | ? | ? |
| **Debouncing** | ? | ? | ? | ? | ? | ? | ? | ? | ? | ? |

### Editor Types (EditableColumn)

| Editor Type | Description |
|-------------|-------------|
| **Auto** | Automatically select based on TypeTraits |
| **Text** | Plain text input |
| **Number** | Numeric input |
| **Checkbox** | Boolean checkbox |
| **Date** | Date picker (yyyy-MM-dd) |
| **DateTimeLocal** | DateTime picker |
| **Time** | Time picker (HH:mm) |
| **TextArea** | Multiline text input |
| **Select** | Dropdown select |
| **RadioGroup** | Radio button group |

### Validation Features

| Feature | FormRow | Editable | MultiState | Filterable | ConditionalStyle | FormattedValue | Icon | Optimized | Virtual | RowColumn |
|---------|:-------:|:--------:|:----------:|:----------:|:----------------:|:--------------:|:----:|:---------:|:-------:|:---------:|
| **Custom Validators** | ? | ? | ? | ? | ? | ? | ? | ? | ? | ? |
| **DataAnnotations** | ? | ? | ? | ? | ? | ? | ? | ? | ? | ? |
| **Validate on Change** | ? | ? | ? | ? | ? | ? | ? | ? | ? | ? |
| **Validation Error Display** | ? | ? | ? | ? | ? | ? | ? | ? | ? | ? |
| **Async Validation** | ? | ? | ? | ? | ? | ? | ? | ? | ? | ? |

### Formatting Features

| Feature | FormRow | Editable | MultiState | Filterable | ConditionalStyle | FormattedValue | Icon | Optimized | Virtual | RowColumn |
|---------|:-------:|:--------:|:----------:|:----------:|:----------------:|:--------------:|:----:|:---------:|:-------:|:---------:|
| **Format String** | ? | ? | ? | ? | ? | ? | ? | ? | ? | ? |
| **Custom Formatter** | ? | ? | ? | ? | ? | ? | ? | ? | ? | ? |
| **Culture Support** | ? | ? | ? | ? | ? | ? | ? | ? | ? | ? |
| **IFormattable Support** | ? | ? | ? | ? | ? | ? | ? | ? | ? | ? |

### Template Features

| Feature | FormRow | Editable | MultiState | Filterable | ConditionalStyle | FormattedValue | Icon | Optimized | Virtual | RowColumn |
|---------|:-------:|:--------:|:----------:|:----------:|:----------------:|:--------------:|:----:|:---------:|:-------:|:---------:|
| **Display Template** | ? | ? | ? | ? | ? | ? | ? | ? | ? | ? |
| **Form/Expanded Template** | ? | ? | ? | ? | ? | ? | ? | ? | ? | ? |
| **Cell Template** | ? | ? | ? | ? | ? | ? | ? | ? | ? | ? |
| **Cascading Value Context** | ? | ? | ? | ? | ? | ? | ? | ? | ? | ? |

### Event Callbacks

| Event | FormRow | Editable | MultiState | Filterable | ConditionalStyle | FormattedValue | Icon | Optimized | Virtual | RowColumn |
|-------|:-------:|:--------:|:----------:|:----------:|:----------------:|:--------------:|:----:|:---------:|:-------:|:---------:|
| **OnBeforeEdit/Expand** | ? | ? | ? | ? | ? | ? | ? | ? | ? | ? |
| **OnValueChanged** | ? | ? | ? | ? | ? | ? | ? | ? | ? | ? |
| **OnSaveAsync** | ? | ? | ? | ? | ? | ? | ? | ? | ? | ? |
| **OnSaved/OnSaveResult** | ? | ? | ? | ? | ? | ? | ? | ? | ? | ? |
| **OnCancelled/OnCancelEdit** | ? | ? | ? | ? | ? | ? | ? | ? | ? | ? |
| **OnStateChanged** | ? | ? | ? | ? | ? | ? | ? | ? | ? | ? |
| **OnFilterChanged** | ? | ? | ? | ? | ? | ? | ? | ? | ? | ? |
| **OnExpanded/OnCollapsed** | ? | ? | ? | ? | ? | ? | ? | ? | ? | ? |

### Filtering Features (FilterableColumn)

| Feature | Description |
|---------|-------------|
| **Filter Operators** | Extensible operator system (Contains, Equals, StartsWith, etc.) |
| **Type-Aware Defaults** | Auto-selects operators based on property type |
| **Filter Debouncing** | Configurable delay before applying filter |
| **IQueryable Integration** | Applies filters directly to queryable data source |
| **Filter UI in Header** | Optional inline filter toggle in column header |
| **FilterableGrid Integration** | Coordinates filtering across multiple columns |

### Styling Features

| Feature | FormRow | Editable | MultiState | Filterable | ConditionalStyle | FormattedValue | Icon | Optimized | Virtual | RowColumn |
|---------|:-------:|:--------:|:----------:|:----------:|:----------------:|:--------------:|:----:|:---------:|:-------:|:---------:|
| **Conditional CSS Classes** | ? | ? | ? | ? | ? | ? | ? | ? | ? | ? |
| **Style Rules** | ? | ? | ? | ? | ? | ? | ? | ? | ? | ? |
| **Icon Display** | ? | ? | ? | ? | ? | ? | ? | ? | ? | ? |
| **Icon/Color Mapping** | ? | ? | ? | ? | ? | ? | ? | ? | ? | ? |
| **Tooltip Support** | ? | ? | ? | ? | ? | ? | ? | ? | ? | ? |
| **Dim Inactive Rows** | ? | ? | ? | ? | ? | ? | ? | ? | ? | ? |
| **Base CSS Class** | ? | ? | ? | ? | ? | ? | ? | ? | ? | ? |

### Concurrent Edit/Expand Behaviors

| Behavior | FormRowColumn | RowColumn |
|----------|:-------------:|:---------:|
| **Block** | ? | ? |
| **AllowMultiple** | ? | ? |
| **CancelCurrent/CollapseCurrent** | ? | ? |
| **SaveCurrent** | ? | ? |

### Performance Optimizations

| Feature | FormRow | Editable | MultiState | Filterable | ConditionalStyle | FormattedValue | Icon | Optimized | Virtual | RowColumn |
|---------|:-------:|:--------:|:----------:|:----------:|:----------------:|:--------------:|:----:|:---------:|:-------:|:---------:|
| **Expression Compilation** | ? | ? | ? | ? | ? | ? | ? | ? | ? | ? |
| **Memoization/Caching** | ? | ? | ? | ? | ? | ? | ? | ? | ? | ? |
| **Minimal DOM** | ? | ? | ? | ? | ? | ? | ? | ? | ? | ? |
| **SetKey for Reconciliation** | ? | ? | ? | ? | ? | ? | ? | ? | ? | ? |

### Virtualization Support

| Feature | FormRow | Editable | MultiState | Filterable | ConditionalStyle | FormattedValue | Icon | Optimized | Virtual | RowColumn |
|---------|:-------:|:--------:|:----------:|:----------:|:----------------:|:--------------:|:----:|:---------:|:-------:|:---------:|
| **Optimized for Virtualization** | ? | ? | ? | ? | ? | ? | ? | ? | ? | ? |
| **Spacer Row Support** | ? | ? | ? | ? | ? | ? | ? | ? | ? | ? |
| **Configurable Row Height** | ? | ? | ? | ? | ? | ? | ? | ? | ? | ? |
| **ExpandedRowSpan** | ? | ? | ? | ? | ? | ? | ? | ? | ? | ? |

---

## Column Details

### FormRowColumn

**Purpose:** Renders a full inline form when a row is in edit mode, with the form spanning across the row.

**Key Features:**
- Trigger modes: Button, RowClick, Custom
- Concurrent edit behaviors: Block, AllowMultiple, CancelCurrent, SaveCurrent
- Form/Display templates with cascading context
- DataAnnotations and custom validation support
- Dim inactive rows visual feedback
- FormField component integration via cascading value

**Use When:** You need a complete form experience within the grid row, not just individual cell editing.

---

### EditableColumn

**Purpose:** Most feature-rich inline cell editor with multiple editor types and validation.

**Key Features:**
- 10 editor types with auto-detection
- DataAnnotations + custom validators
- Debounce support for input events
- Commit on input (auto-save) or explicit save
- Display template for read mode
- Select/Radio options with enum support
- Culture-aware formatting and parsing

**Use When:** You need maximum flexibility in cell editing with different input types.

---

### MultiStateColumn

**Purpose:** Explicit state machine (Reading ? Editing ? Loading) for controlled editing workflows.

**Key Features:**
- Three explicit states with visual feedback
- Async save handler with success/error callbacks
- Before/after edit events with cancellation
- Inline mode for always-visible editor
- Placeholder and read-only support
- State transition events

**Use When:** You need explicit control over edit states and async save operations.

---

### FilterableColumn

**Purpose:** Column with built-in filtering capabilities and UI.

**Key Features:**
- Extensible filter operator system
- Type-aware default operators (String, Numeric, Date, Boolean)
- Debounced filter input
- IQueryable integration for server-side filtering
- FilterableGrid coordination for multi-column filtering
- Inline filter UI or dedicated filter toolbar

**Use When:** You need per-column filtering with server-side query support.

---

### ConditionalStyleColumn

**Purpose:** Apply dynamic CSS classes, icons, and tooltips based on value rules.

**Key Features:**
- Multiple style rules with priority
- Combine multiple matching rules option
- Icon and tooltip per rule
- Custom cell template override
- Value formatter support
- Base CSS class configuration

**Use When:** You need visual indicators or styling based on cell values.

---

### FormattedValueColumn

**Purpose:** Display formatted values with culture-aware formatting.

**Key Features:**
- Custom formatter function
- Culture name parameter for re-rendering on culture change
- Auto-sorting enabled by default
- TypeTraits fallback for date/time

**Use When:** You need simple value display with custom formatting.

---

### IconColumn

**Purpose:** Display icons based on cell values with optional value text.

**Key Features:**
- Icon mapper function (value ? CSS class)
- Color mapper for icon coloring
- Tooltip mapper
- Show/hide value text option
- Enum domain memoization for performance
- TypeTraits date/time formatting

**Use When:** You need to display status icons or visual indicators based on values.

---

### OptimizedColumn

**Purpose:** Demonstrates performance best practices for QuickGrid columns.

**Key Features:**
- CSS class caching (max 8 combinations for 3 boolean states)
- Compiled expression accessors
- Proper sequence number management
- Minimal DOM manipulation
- Highlight/Warning/Error conditions
- Cache statistics for analysis

**Use When:** You need maximum rendering performance and want to learn optimization techniques.

---

### VirtualColumn

**Purpose:** Minimal overhead column optimized for virtual scrolling.

**Key Features:**
- Custom formatter or format string
- Optional CSS class wrapper
- Text-only rendering for minimal DOM
- TypeTraits date/time formatting
- Compiled accessor

**Use When:** You have large datasets with virtual scrolling and need minimal per-cell overhead.

---

### RowColumn

**Purpose:** Expandable row overlay with spacer row support for virtualization compatibility.

**Key Features:**
- Trigger modes: Button, RowClick
- Concurrent behaviors: Block, AllowMultiple, CollapseCurrent
- Configurable ExpandedRowSpan and RowHeight
- ExpandableGridDataSource integration for spacer rows
- IRowIdentifiable constraint for row identification
- Before/after expand events with cancellation

**Use When:** You need expanded row details that work with virtualization.

---

## Recommended Column Selection

| Scenario | Recommended Column |
|----------|-------------------|
| Simple display with formatting | `FormattedValueColumn` or `VirtualColumn` |
| Status indicators with icons | `IconColumn` |
| Conditional styling based on values | `ConditionalStyleColumn` |
| Quick inline cell editing | `EditableColumn` |
| Controlled async editing workflow | `MultiStateColumn` |
| Full form editing within row | `FormRowColumn` |
| Column filtering | `FilterableColumn` |
| Expandable row details | `RowColumn` |
| Large datasets with virtualization | `VirtualColumn` or `OptimizedColumn` |
| Maximum performance | `OptimizedColumn` |