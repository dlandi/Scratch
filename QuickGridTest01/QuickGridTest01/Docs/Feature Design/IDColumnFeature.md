# ID Column Feature

## 1. Overview
I want to express an GUID ID column as an immutable SVG icon that visually represents the ID. 
This feature will render an SVG graphic based on the GUID value, allowing for a compact and 
visually distinctive representation of the identifier.

### Formula:
```csharp
// Each byte of the GUID is represented as a square in a 4x4 grid

using System;
using System.Globalization;
using System.Text;

public static class GuidSvg
{
    /// <summary>
    /// Packs a GUID string into a compact square SVG (4x4 cells = 16 bytes).
    /// Each cell encodes one byte from the GUID as a grayscale value.
    /// </summary>
    public static string PackGuidToSvg(string guidText, int cellSize = 24, int padding = 2, bool drawHexLabels = false)
    {
        if (!Guid.TryParse(guidText, out var guid))
            throw new ArgumentException("Invalid GUID string.", nameof(guidText));

        Span<byte> bytes = stackalloc byte[16];
        guid.TryWriteBytes(bytes);

        const int grid = 4; // 4x4 = 16 bytes
        int width = (grid * cellSize) + (padding * 2);
        int height = (grid * cellSize) + (padding * 2);

        var sb = new StringBuilder(capacity: 2048);

        sb.Append(CultureInfo.InvariantCulture,
            $"<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"{width}\" height=\"{height}\" viewBox=\"0 0 {width} {height}\">");

        sb.Append("<rect width=\"100%\" height=\"100%\" fill=\"white\"/>");

        for (int i = 0; i < 16; i++)
        {
            int row = i / grid;
            int col = i % grid;

            int x = padding + (col * cellSize);
            int y = padding + (row * cellSize);

            byte b = bytes[i];
            // Grayscale mapping: 0 => black, 255 => white
            string fill = $"rgb({b},{b},{b})";

            sb.Append(CultureInfo.InvariantCulture,
                $"<rect x=\"{x}\" y=\"{y}\" width=\"{cellSize}\" height=\"{cellSize}\" fill=\"{fill}\" stroke=\"#000\" stroke-width=\"1\"/>");

            if (drawHexLabels)
            {
                string hex = b.ToString("X2", CultureInfo.InvariantCulture);
                int tx = x + (cellSize / 2);
                int ty = y + (cellSize / 2) + 5; // visual centering tweak

                // Pick a contrasting label color
                string textColor = b < 128 ? "#fff" : "#000";

                sb.Append(CultureInfo.InvariantCulture,
                    $"<text x=\"{tx}\" y=\"{ty}\" text-anchor=\"middle\" font-family=\"Consolas, monospace\" font-size=\"12\" fill=\"{textColor}\">{hex}</text>");
            }
        }

        sb.Append("</svg>");
        return sb.ToString();
    }
}
```

### Example Usage
```csharp

var svg = GuidSvg.PackGuidToSvg("7f2c9a61-4b3e-4f6e-9d1a-8c3e51c0b7a4", drawHexLabels: true);
File.WriteAllText("guid.svg", svg);

```