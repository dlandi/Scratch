namespace QuickGridTest01.CustomColumns;

/// <summary>
/// Metadata descriptor for dynamically generating columns at runtime.
/// </summary>
public class ColumnMetadata
{
    public string PropertyPath { get; set; } = default!;
    public string? Title { get; set; }
    public Type? PropertyType { get; set; }
    public string? Format { get; set; }
    public bool Sortable { get; set; } = true;
    public int? Width { get; set; }
    public Func<object?, string>? CustomFormatter { get; set; }
}

/// <summary>
/// Builder for creating column metadata configurations.
/// </summary>
public class ColumnMetadataBuilder
{
    private readonly List<ColumnMetadata> _columns = new();

    public ColumnMetadataBuilder Add(string propertyPath,
        Action<ColumnMetadata>? configure = null)
    {
        var metadata = new ColumnMetadata { PropertyPath = propertyPath };
        configure?.Invoke(metadata);
        _columns.Add(metadata);
        return this;
    }

    public List<ColumnMetadata> Build() => _columns;

    // Convenience methods
    public ColumnMetadataBuilder AddProperty(string propertyPath, string? title = null)
    {
        return Add(propertyPath, m => m.Title = title);
    }

    public ColumnMetadataBuilder AddFormatted(string propertyPath, string format, string? title = null)
    {
        return Add(propertyPath, m =>
        {
            m.Format = format;
            m.Title = title;
        });
    }

    public ColumnMetadataBuilder AddCustom(string propertyPath,
        Func<object?, string> formatter, string? title = null)
    {
        return Add(propertyPath, m =>
        {
            m.CustomFormatter = formatter;
            m.Title = title;
        });
    }
}