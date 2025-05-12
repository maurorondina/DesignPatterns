using System.Collections;

public static class CompositeGoodExample
{
    public static void Run()
    {
        var package = new Box
        {
            new Box { new Microphone(29.99m) }, // Box 1
            new Box // Box 2
            {
                new Box { new Mouse(18.00m) },    // Box 3
                new Box { new Keyboard(40.00m) } // Box 4
            }
        };

        Console.WriteLine($"Total Price: {package.Price}"); // 87.99
    }

    // COMPONENT INTERFACE (is implemented by both leaf and composite)
    public interface IItem
    {
        decimal Price { get; }
    }

    // LEAF
    public record Keyboard(decimal Price) : IItem;
    public record Microphone(decimal Price) : IItem;
    public record Mouse(decimal Price) : IItem;

    // COMPOSITE
    public class Box : IItem, IEnumerable<IItem>
    {
        private readonly List<IItem> _items = new();

        public decimal Price => _items.Sum(item => item.Price); // LINQ for clarity

        public void Add(IItem item) => _items.Add(item);

        public IEnumerator<IItem> GetEnumerator() => _items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}