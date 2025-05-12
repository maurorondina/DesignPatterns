using System.Collections.Concurrent;
public static class FlyweightGoodExample
{
    public static void Run()
    {
        var editor = new TextEditor();
        var style1 = new TextStyle("Arial", 12, "Red");
        var style2 = new TextStyle("Times New Roman", 14, "Blue");

        editor.AddCharacter('A', 0, 0, style1);
        editor.AddCharacter('B', 10, 0, style1); // Reuses existing flyweight
        editor.AddCharacter('C', 20, 0, style2);

        editor.RenderAll();

        // Memory: 2 flyweight objects (style1 + style2) shared across all characters
    }

    // Intrinsic State: shared across all instances
    public record TextStyle(string FontFamily, int FontSize, string Color);

    // FLYWEIGHT
    // The Intrinsic State (TextStyle) is stored in the flyweight object
    // The Extrinsic State (character, x, y) is passed as parameters to the Render method
    public interface ITextCharacterFlyweight
    {
        void Render(char character, int x, int y);
    }

    public class TextCharacterFlyweight(TextStyle style) : ITextCharacterFlyweight
    {
        public void Render(char character, int x, int y) =>
            Console.WriteLine($"Rendered '{character}' at ({x}, {y}) with {style.FontFamily} {style.FontSize}px in {style.Color}");
    }

    // FLYWEIGHT FACTORY
    // The factory manages the flyweight instances, ensuring that only one instance of
    // each unique flyweight is created and reused for all clients
    public static class TextCharacterFlyweightFactory
    {
        private static readonly ConcurrentDictionary<TextStyle, Lazy<ITextCharacterFlyweight>> _flyweights = new();

        public static ITextCharacterFlyweight GetFlyweight(TextStyle style) =>
            _flyweights.GetOrAdd(
                style,
                key => new Lazy<ITextCharacterFlyweight>(() => new TextCharacterFlyweight(key))
                ).Value;
    }

    // CONTEXT
    public class TextCharacter
    {
        private readonly char _character;
        private readonly int _x;
        private readonly int _y;
        private readonly ITextCharacterFlyweight _flyweight;

        public TextCharacter(char character, int x, int y, TextStyle style)
        {
            _character = character;
            _x = x;
            _y = y;
            _flyweight = TextCharacterFlyweightFactory.GetFlyweight(style);
        }

        public void Render() => _flyweight.Render(_character, _x, _y);
    }

    // CLIENT
    public class TextEditor
    {
        private readonly List<TextCharacter> _characters = new();

        public void AddCharacter(char c, int x, int y, TextStyle style) =>
            _characters.Add(new(c, x, y, style));

        public void RenderAll()
        {
            foreach (var context in _characters)
                context.Render();
        }
    }
}