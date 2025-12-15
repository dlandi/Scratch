namespace QuickGridTest01.ComposableColumns.Infrastructure;

/// <summary>
/// Describes the primitive shape/category of a generic value type,
/// allowing fast switching between formatting and parsing behaviors without repeated reflection.
/// </summary>
/// <remarks>
/// The values represent common UI/editor-relevant groupings. They are intentionally coarse-grained
/// to keep hot-path branching cheap while still providing high-quality input/output behavior.
/// </remarks>
internal enum ValueKind
{
    /// <summary>Boolean values (true/false).</summary>
    Boolean,

    /// <summary>Date-only semantic values (e.g., <see cref="DateOnly"/>).</summary>
    Date,

    /// <summary>Time-only semantic values (e.g., <see cref="TimeOnly"/>).</summary>
    Time,

    /// <summary>DateTime semantic values (e.g., <see cref="DateTime"/>).</summary>
    DateTime,

    /// <summary>32-bit integral numeric values (<see cref="int"/>).</summary>
    Int32,

    /// <summary>64-bit integral numeric values (<see cref="long"/>).</summary>
    Int64,

    /// <summary>High-precision decimal numeric values (<see cref="decimal"/>).</summary>
    Decimal,

    /// <summary>Double-precision floating point values (<see cref="double"/>).</summary>
    Double,

    /// <summary>Single-precision floating point values (<see cref="float"/>).</summary>
    Single,

    /// <summary>Enumeration types (including nullable enums).</summary>
    Enum,

    /// <summary>Strings.</summary>
    String,

    /// <summary>Any other value not covered by the specific categories.</summary>
    Other
}
