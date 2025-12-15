using System.Globalization;
using System.Linq.Expressions;

namespace QuickGridTest01.ComposableColumns.Infrastructure;

/// <summary>
/// Provides cached type information and helpers for <typeparamref name="T"/> that are computed once per
/// closed generic type and reused. This eliminates repeated calls to reflection in rendering and input hot paths.
/// </summary>
/// <typeparam name="T">The value type that the cached traits apply to.</typeparam>
/// <remarks>
/// Initialization is performed exactly once per closed generic type by the CLR and is thread-safe by design.
/// After initialization, lookups are simple static field reads and switch statements.
/// </remarks>
internal static class TypeTraits<T>
{
    /// <summary>
    /// The generic type parameter <typeparamref name="T"/> as a <see cref="Type"/> instance.
    /// </summary>
    public static readonly Type Type = typeof(T);

    /// <summary>
    /// When <typeparamref name="T"/> is nullable, contains the underlying non-nullable type; otherwise <c>null</c>.
    /// </summary>
    public static readonly Type? NullableUnderlying = Nullable.GetUnderlyingType(Type);

    /// <summary>
    /// The non-nullable representation of <typeparamref name="T"/> (same as <see cref="Type"/> when not nullable).
    /// </summary>
    public static readonly Type NonNullableType = NullableUnderlying ?? Type;

    /// <summary>
    /// Indicates whether <typeparamref name="T"/> is a nullable type (i.e., <c>Nullable&lt;&gt;</c>).
    /// </summary>
    public static readonly bool IsNullable = NullableUnderlying is not null;

    /// <summary>
    /// Indicates whether the non-nullable representation is an enum type.
    /// </summary>
    public static readonly bool IsEnum = NonNullableType.IsEnum;

    /// <summary>
    /// A cheap, precomputed categorization of <typeparamref name="T"/> used to drive parsing/formatting logic.
    /// </summary>
    public static readonly ValueKind Kind = ComputeKind(NonNullableType);

    /// <summary>
    /// Cached delegate that boxes a non-nullable value into its corresponding <c>Nullable&lt;&gt;</c> wrapper and
    /// returns it as <see cref="object"/>. This avoids using <see cref="Activator.CreateInstance(Type, object[])"/>
    /// on hot paths when dealing with nullable values.
    /// </summary>
    private static readonly Func<object, object>? s_nullableBoxer = CreateNullableBoxer();

    private static ValueKind ComputeKind(Type t)
    {
        if (t == typeof(bool)) return ValueKind.Boolean;
        if (t == typeof(DateOnly)) return ValueKind.Date;
        if (t == typeof(TimeOnly)) return ValueKind.Time;
        if (t == typeof(DateTime)) return ValueKind.DateTime;
        if (t == typeof(int)) return ValueKind.Int32;
        if (t == typeof(long)) return ValueKind.Int64;
        if (t == typeof(decimal)) return ValueKind.Decimal;
        if (t == typeof(double)) return ValueKind.Double;
        if (t == typeof(float)) return ValueKind.Single;
        if (t.IsEnum) return ValueKind.Enum;
        if (t == typeof(string)) return ValueKind.String;
        return ValueKind.Other;
    }

    /// <summary>
    /// Formats a value of <typeparamref name="T"/> for use in HTML input elements using stable, culture-invariant
    /// formats where required (e.g., date/time). For non-special types, falls back to <see cref="object.ToString"/>.
    /// </summary>
    /// <param name="value">The value to format.</param>
    /// <param name="kindOverride">Optional explicit editor kind that affects Date vs DateTimeLocal formatting.</param>
    /// <param name="culture">Culture for numeric formatting when applicable (dates use invariant formats).</param>
    /// <returns>String representation appropriate for the input element's <c>value</c> attribute.</returns>
    public static string FormatForInput(T? value, object? kindOverride, CultureInfo culture)
    {
        if (value is null) return string.Empty;

        switch (Kind)
        {
            case ValueKind.Date:
                return ((DateOnly)(object)value).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            case ValueKind.Time:
                return ((TimeOnly)(object)value).ToString("HH:mm", CultureInfo.InvariantCulture);
            case ValueKind.DateTime:
            {
                var dt = (DateTime)(object)value;
                var isDateTimeLocal = string.Equals(kindOverride?.ToString(), "DateTime", StringComparison.Ordinal);
                return isDateTimeLocal
                    ? dt.ToString("yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture)
                    : dt.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            }
            case ValueKind.Int32:
            case ValueKind.Int64:
            case ValueKind.Decimal:
            case ValueKind.Double:
            case ValueKind.Single:
                return Convert.ToString(value, CultureInfo.InvariantCulture) ?? string.Empty;
            default:
                return value?.ToString() ?? string.Empty;
        }
    }

    /// <summary>
    /// Converts a value of <typeparamref name="T"/> to a stable string suitable for <c>&lt;option value&gt;</c>
    /// attributes and radio input values, using invariant formats for date/time and numeric kinds.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <param name="culture">Culture for numeric formatting when applicable (dates use invariant formats).</param>
    /// <returns>A stable, culture-invariant string representation for option values.</returns>
    public static string ToOptionValueString(T? value, CultureInfo culture)
    {
        if (value is null) return string.Empty;

        switch (Kind)
        {
            case ValueKind.Enum:
                return value.ToString() ?? string.Empty;
            case ValueKind.Date:
                return ((DateOnly)(object)value).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            case ValueKind.Time:
                return ((TimeOnly)(object)value).ToString("HH:mm", CultureInfo.InvariantCulture);
            case ValueKind.DateTime:
                return ((DateTime)(object)value).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            case ValueKind.Int32:
            case ValueKind.Int64:
            case ValueKind.Decimal:
            case ValueKind.Double:
            case ValueKind.Single:
                return Convert.ToString(value, CultureInfo.InvariantCulture) ?? string.Empty;
            default:
                return value.ToString() ?? string.Empty;
        }
    }

    /// <summary>
    /// Attempts to parse a raw event value (from <see cref="Microsoft.AspNetCore.Components.ChangeEventArgs.Value"/>)
    /// into <typeparamref name="T"/> using fast, non-throwing paths for common kinds. For empty strings, returns
    /// <c>true</c> and sets <paramref name="parsed"/> to default (null for reference/nullable types).
    /// </summary>
    /// <param name="eventValue">The raw event value (often <see cref="string"/> or <see cref="bool"/>).</param>
    /// <param name="culture">Culture for numeric parsing when applicable.</param>
    /// <param name="parsed">Receives the parsed value when parsing succeeds or empty input is provided.</param>
    /// <returns>
    /// <c>true</c> when a parse attempt occurred (including empty-to-default); <c>false</c> only for unexpected
    /// failures (e.g., invalid enum name) where the value cannot be produced.
    /// </returns>
    public static bool TryParseFromEventValue(object? eventValue, CultureInfo culture, out T? parsed)
    {
        if (Kind == ValueKind.Boolean)
        {
            bool? pb = null;
            switch (eventValue)
            {
                case bool b:
                    pb = b; break;
                case string sb:
                    if (string.IsNullOrWhiteSpace(sb)) pb = null;
                    else if (sb == "on") pb = true;
                    else if (bool.TryParse(sb, out var b2)) pb = b2;
                    break;
            }
            parsed = pb is null ? default : (T)(object)pb.Value;
            return true;
        }

        var s = eventValue?.ToString();
        if (string.IsNullOrWhiteSpace(s))
        {
            parsed = default;
            return true;
        }

        switch (Kind)
        {
            case ValueKind.Enum:
            {
                try
                {
                    var ev = Enum.Parse(NonNullableType, s, ignoreCase: true);
                    object boxed = ev;
                    if (IsNullable && s_nullableBoxer is not null) boxed = s_nullableBoxer(ev);
                    parsed = (T)boxed;
                    return true;
                }
                catch
                {
                    parsed = default;
                    return false;
                }
            }
            case ValueKind.Date:
            {
                if (DateOnly.TryParseExact(s, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var d))
                { parsed = (T)(object)d; return true; }
                parsed = default; return true;
            }
            case ValueKind.Time:
            {
                if (TimeOnly.TryParseExact(s, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out var tm))
                { parsed = (T)(object)tm; return true; }
                parsed = default; return true;
            }
            case ValueKind.DateTime:
            {
                if (DateTime.TryParseExact(s, ["yyyy-MM-dd", "yyyy-MM-ddTHH:mm"], CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out var dt))
                { parsed = (T)(object)dt; return true; }
                parsed = default; return true;
            }
            case ValueKind.Int32:
                if (int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out var i32))
                { parsed = (T)(object)i32; return true; }
                parsed = default; return false;
            case ValueKind.Int64:
                if (long.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out var i64))
                { parsed = (T)(object)i64; return true; }
                parsed = default; return false;
            case ValueKind.Decimal:
                if (decimal.TryParse(s, NumberStyles.Number, CultureInfo.InvariantCulture, out var dec))
                { parsed = (T)(object)dec; return true; }
                parsed = default; return false;
            case ValueKind.Double:
                if (double.TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out var dbl))
                { parsed = (T)(object)dbl; return true; }
                parsed = default; return false;
            case ValueKind.Single:
                if (float.TryParse(s, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out var fl))
                { parsed = (T)(object)fl; return true; }
                parsed = default; return false;
            case ValueKind.String:
                parsed = (T)(object)s; return true;
            default:
                try
                {
                    var obj = Convert.ChangeType(s, NonNullableType, CultureInfo.InvariantCulture);
                    if (IsNullable && s_nullableBoxer is not null) obj = s_nullableBoxer(obj!);
                    parsed = (T)obj!;
                    return true;
                }
                catch
                {
                    parsed = default;
                    return false;
                }
        }
    }

    /// <summary>
    /// Builds an immutable list of <see cref="SelectOption{T}"/> for enum types (including nullable enums).
    /// For non-enum types, returns <see cref="Array.Empty{T}()"/>.
    /// </summary>
    /// <returns>An immutable list of enum values and their string names.</returns>
    public static IReadOnlyList<SelectOption<T>> BuildEnumOptions()
    {
        if (!IsEnum) return Array.Empty<SelectOption<T>>();
        var names = Enum.GetNames(NonNullableType);
        var values = Enum.GetValues(NonNullableType);
        var list = new List<SelectOption<T>>(names.Length);
        int i = 0;
        foreach (var v in values)
        {
            object boxed = v!;
            if (IsNullable && s_nullableBoxer is not null) boxed = s_nullableBoxer(v!);
            list.Add(new SelectOption<T>((T)boxed, names[i++]));
        }
        return list;
    }

    /// <summary>
    /// Creates a compiled delegate that wraps a non-nullable <see cref="object"/> into <c>Nullable&lt;NonNullableType&gt;</c>
    /// and returns it as <see cref="object"/>. Returns <c>null</c> when <typeparamref name="T"/> is not nullable.
    /// </summary>
    private static Func<object, object>? CreateNullableBoxer()
    {
        if (!IsNullable) return null;
        var inner = NonNullableType;
        var ctor = typeof(Nullable<>).MakeGenericType(inner).GetConstructor([inner])!;
        var objParam = Expression.Parameter(typeof(object), "o");
        var newExpr = Expression.New(ctor, Expression.Convert(objParam, inner));
        var body = Expression.Convert(newExpr, typeof(object));
        return Expression.Lambda<Func<object, object>>(body, objParam).Compile();
    }
}
