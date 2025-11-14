namespace QuickGridTest01.FormattedValue.Demo.Models;

/// <summary>
/// System log entry model demonstrating timestamp, duration, and memory formats.
/// </summary>
public class LogEntry
{
    /// <summary>
    /// Gets or sets the log entry unique identifier.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the timestamp when the log entry was created.
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Gets or sets the log level (INFO, WARNING, ERROR, etc.).
    /// </summary>
    public string Level { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the log message.
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the source of the log entry.
    /// </summary>
    public string Source { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the operation duration in milliseconds.
    /// </summary>
    public double Duration { get; set; }

    /// <summary>
    /// Gets or sets the memory usage in bytes.
    /// </summary>
    public long MemoryBytes { get; set; }
}
