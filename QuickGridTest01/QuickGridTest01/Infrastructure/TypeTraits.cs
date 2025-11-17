using System;
using System.Globalization;
using QuickGridTest01.CustomColumns;

namespace QuickGridTest01.Infrastructure
{
    /// <summary>
    /// Describes the primitive "shape" of a generic value type <typeparamref name="T"/>.
    /// </summary>
    /// <remarks>
    /// This enum is used by <see cref="TypeTraits{T}"/> to quickly route parsing/formatting logic
    /// without repeatedly consulting reflection APIs (e.g., <c>typeof(T)</c>, <see cref="Type.IsEnum"/>,
    /// or <see cref="Nullable.GetUnderlyingType(Type)"/>). The value is computed once per closed generic type
    /// and then reused across the application for fast type decisions in hot paths (like input parsing and
    /// QuickGrid cell rendering).
    /// </remarks>
    internal enum ValueKind
    {
        Boolean,
        Date,
        Time,
        DateTime,
        Int32,
        Int64,
        Decimal,
        Double,
        Single,
        Enum,
        String,
        Other
    }

    /// <summary>
    /// Provides cached type information and helpers for <typeparamref name="T"/> that are computed once per closed
    /// generic type and reused. This eliminates repeated calls to reflection (e.g., <c>typeof(T)</c>,
    /// <see cref="Nullable.GetUnderlyingType(Type)"/>, and <see cref="Type.IsEnum"/>) throughout hot paths.
    /// </summary>
    /// <remarks>
    /// Why not just use typeof each time?
    /// - While <c>typeof(T)</c> is itself cheap, the additional checks we usually perform next (nullable unwrap,
    ///   enum inspection, numeric/date categorization) tend to be repeated in many places (formatting, parsing,
    ///   editor selection). Centralizing these into a single static cache per <typeparamref name="T"/> ensures
    ///   they are computed once and reused.
    ///
    /// What about reflection inside this class?
    /// - The minimal use of reflection (e.g., <see cref="Nullable.GetUnderlyingType(Type)"/>, 
    ///   <see cref="Enum.GetNames(Type)"/>, and <see cref="Type.MakeGenericType(Type[])"/>) happens once per
    ///   closed generic type when the type is first used. This is an amortized cost, after which all lookups are
    ///   simple field reads and switch statements.
    ///
    /// Where is it useful?
    /// - Blazor/QuickGrid inputs need to parse/format values and choose appropriate editors frequently.
    ///   Using <see cref="TypeTraits{T}"/> avoids repeating those decisions and reduces branching/allocations.
    /// </remarks>
    internal static class TypeTraits<T>
    {
        public static readonly Type Type = typeof(T);
        public static readonly Type? NullableUnderlying = Nullable.GetUnderlyingType(Type);
        public static readonly Type NonNullableType = NullableUnderlying ?? Type;
        public static readonly bool IsNullable = NullableUnderlying is not null;
        public static readonly bool IsEnum = NonNullableType.IsEnum;
        public static readonly ValueKind Kind = ComputeKind(NonNullableType);

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
        /// Formats a value for use in HTML input elements using stable, culture-invariant formats where required.
        /// </summary>
        /// <param name="value">The value to format.</param>
        /// <param name="kindOverride">Optional explicit editor kind that affects Date vs DateTimeLocal rendering.</param>
        /// <param name="culture">The culture for formatting where applicable (numeric fallback uses invariant).</param>
        /// <returns>String representation for the input's <c>value</c> attribute.</returns>
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
                    var isDateTimeLocal = string.Equals(kindOverride?.ToString(), "DateTimeLocal", StringComparison.Ordinal);
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
        /// Converts a value to a stable option string for <c>&lt;option value&gt;</c> or radio values.
        /// </summary>
        public static string ToOptionValueString(T? value, CultureInfo culture)
        {
            if (value is null) return string.Empty;

            switch (Kind)
            {
                case ValueKind.Enum:
                    return value.ToString() ?? string.Empty; // enum name
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
        /// Attempts to parse an event value (from Blazor <c>ChangeEventArgs.Value</c>) into <typeparamref name="T"/>.
        /// </summary>
        /// <param name="eventValue">The raw event value (bool or string).</param>
        /// <param name="culture">The culture to use for parsing where applicable.</param>
        /// <param name="parsed">The parsed value, or default when parsing fails/empty.</param>
        /// <returns>True if a parse attempt was made (even if resulting in default), false for unexpected failures.</returns>
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

            try
            {
                switch (Kind)
                {
                    case ValueKind.Enum:
                    {
                        var ev = Enum.Parse(NonNullableType, s, ignoreCase: true);
                        object boxed = ev;
                        if (IsNullable) boxed = CreateNullable(NonNullableType, ev);
                        parsed = (T)boxed;
                        return true;
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
                        if (DateTime.TryParseExact(s, new[] { "yyyy-MM-dd", "yyyy-MM-ddTHH:mm" }, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out var dt))
                        { parsed = (T)(object)dt; return true; }
                        parsed = default; return true;
                    }
                    case ValueKind.Int32:
                        parsed = (T)(object)int.Parse(s, NumberStyles.Integer, CultureInfo.InvariantCulture); return true;
                    case ValueKind.Int64:
                        parsed = (T)(object)long.Parse(s, NumberStyles.Integer, CultureInfo.InvariantCulture); return true;
                    case ValueKind.Decimal:
                        parsed = (T)(object)decimal.Parse(s, NumberStyles.Number, CultureInfo.InvariantCulture); return true;
                    case ValueKind.Double:
                        parsed = (T)(object)double.Parse(s, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture); return true;
                    case ValueKind.Single:
                        parsed = (T)(object)float.Parse(s, NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture); return true;
                    case ValueKind.String:
                        parsed = (T)(object)s; return true;
                    default:
                        parsed = (T)Convert.ChangeType(s, NonNullableType, CultureInfo.InvariantCulture);
                        if (IsNullable) parsed = (T)CreateNullable(NonNullableType, parsed!);
                        return true;
                }
            }
            catch
            {
                parsed = default;
                return false;
            }
        }

        /// <summary>
        /// Builds the list of <see cref="SelectOption{T}"/> for enum types. For non-enum types returns an empty array.
        /// </summary>
        public static IReadOnlyList<SelectOption<T>> BuildEnumOptions()
        {
            if (!IsEnum) return Array.Empty<SelectOption<T>>();
            var names = Enum.GetNames(NonNullableType);
            var values = Enum.GetValues(NonNullableType);
            var list = new List<SelectOption<T>>(names.Length);
            int i = 0;
            foreach (var v in values)
            {
                object boxed = v;
                if (IsNullable) boxed = CreateNullable(NonNullableType, v);
                list.Add(new SelectOption<T>((T)boxed, names[i++]));
            }
            return list;
        }

        private static object CreateNullable(Type innerType, object value)
            => Activator.CreateInstance(typeof(Nullable<>).MakeGenericType(innerType), value)!;
    }
}
