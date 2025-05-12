public static class FlyweightBadExample
{
    public static void Run()
    {
        var editor = new List<TextCharacter>();
        var style1 = new { FontFamily = "Arial", FontSize = 12, Color = "Red" };

        editor.Add(new TextCharacter('A', 0, 0, style1.FontFamily, style1.FontSize, style1.Color));
        editor.Add(new TextCharacter('B', 10, 0, style1.FontFamily, style1.FontSize, style1.Color)); // Duplicated style data!
        editor.Add(new TextCharacter('C', 20, 0, "Times New Roman", 14, "Blue"));

        foreach (var c in editor)
            c.Render();

        // Memory: 3 TextCharacter objects, each storing full style data
    }

    public class TextCharacter(char character, int x, int y, string fontFamily, int fontSize, string color)
    {
        public void Render() =>
            Console.WriteLine($"Rendered '{character}' at ({x}, {y}) with {fontFamily} {fontSize}px in {color}");
    }
}