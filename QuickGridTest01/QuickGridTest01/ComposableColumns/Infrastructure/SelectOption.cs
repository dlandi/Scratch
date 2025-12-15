namespace QuickGridTest01.ComposableColumns.Infrastructure;

/// <summary>
/// Represents a selectable option for select/radio editors.
/// </summary>
/// <typeparam name="T">The type of the option value.</typeparam>
/// <param name="Value">The value of the option.</param>
/// <param name="Text">The display text for the option.</param>
/// <param name="Disabled">Whether the option is disabled.</param>
public record SelectOption<T>(T Value, string Text, bool Disabled = false);
