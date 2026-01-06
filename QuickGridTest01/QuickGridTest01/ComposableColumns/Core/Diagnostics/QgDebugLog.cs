namespace QuickGridTest01.ComposableColumns.Core.Diagnostics;

internal static class QgDebugLog
{
    public static bool Enabled { get; set; }

    public static void Write(string message)
    {
        if (!Enabled)
            return;

        Console.WriteLine($"[QG] {message}");
    }
}
