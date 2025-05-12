public static class CompositeBadExample
{
    public static void Run()
    {
        // Box 1: Contains a Microphone
        var box1 = new Box();
        box1.Add(new Microphone(29.99m));

        // Box 2: Contains Box 3 and Box 4
        var box2 = new Box();
        var box3 = new Box();
        box3.Add(new Mouse(18.00m));
        var box4 = new Box();
        box4.Add(new Keyboard(40.00m));
        box2.Add(box3);
        box2.Add(box4);

        // Package: Contains Box 1 and Box 2
        var package = new Box();
        package.Add(box1);
        package.Add(box2);

        Console.WriteLine($"Total Price: {package.CalculateTotalPrice()}"); // 87.99
    }

    // Leaf classes (no common interface)
    public record Keyboard(decimal Price);
    public record Microphone(decimal Price);
    public record Mouse(decimal Price);

    // Composite class with type-checking hell
    public class Box
    {
        private List<object> _items = new List<object>();

        public void Add(object item) => _items.Add(item);

        public decimal CalculateTotalPrice()
        {
            decimal total = 0;
            foreach (var item in _items)
            {
                if (item is Keyboard)
                    total += ((Keyboard)item).Price;
                else if (item is Mouse)
                    total += ((Mouse)item).Price;
                else if (item is Microphone)
                    total += ((Microphone)item).Price;
                else if (item is Box)
                    total += ((Box)item).CalculateTotalPrice(); // Recursive call
            }
            return total;
        }
    }
}