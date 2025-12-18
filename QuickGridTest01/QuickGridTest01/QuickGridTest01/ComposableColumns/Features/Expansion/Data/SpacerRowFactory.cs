using QuickGridTest01.ComposableColumns.Features.Expansion.Core;

namespace QuickGridTest01.ComposableColumns.Features.Expansion.Data;

/// <summary>
/// Creates spacer rows for expanded overlay positioning.
/// Spacer rows push real data rows down so they remain visible below the overlay card.
/// </summary>
public static class SpacerRowFactory
{
    private const int Multiplier = 1000;

    public static IEnumerable<T> CreateSpacers<T>(int parentRowId, int count)
        where T : IRowIdentifiable, new()
    {
        for (int i = 1; i <= count; i++)
        {
            yield return new T { Id = EncodeSpacerId(parentRowId, i) };
        }
    }

    public static int EncodeSpacerId(int parentRowId, int offset)
        => checked(-(parentRowId * Multiplier + offset));

    public static int GetParentRowId(int spacerId)
        => Math.Abs(spacerId) / Multiplier;

    public static int GetSpacerOffset(int spacerId)
        => Math.Abs(spacerId) % Multiplier;

    public static bool IsSpacer(int id) => id < 0;

    public static bool IsSpacerItem<T>(T item) where T : IRowIdentifiable
        => item.Id < 0;
}
