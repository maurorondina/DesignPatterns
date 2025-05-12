public static class PrototypeBadExample
{
    public static void Run()
    {
        var original = new Product("Laptop", 999.99m);
        original.Tags.Add(new ("Sale", "20% off")); // Simulate expensive setup

        var shallowclone = new Product(original.Name, original.Price);
        shallowclone.Tags = original.Tags; // Shallow copy: Tags list is shared!
        shallowclone.Tags.Add(new ("Warranty", "2 years")); // Affects original.Tags too!

        var deepClone = new Product(original.Name, original.Price);
        deepClone.Tags = new (original.Tags); // Deep copy: New list!
        deepClone.Tags.Add(new ("New", "Perfect")); // Does not affect original.Tags!

        Console.WriteLine($"Original Product: {original}");
        Console.WriteLine($"Shallow Clone Product: {shallowclone}");
        Console.WriteLine($"Deep Clone Product: {deepClone}");
    }

    public class Product
    {
        public record Tag(string Name, string Description)
        {
            public override string ToString() => $"{Name} ({Description})";
        }

        public string Name { get; init; }
        public decimal Price { get; init; }
        public List<Tag> Tags { get; set; } = new();

        // Costly initialization (e.g., database call)
        public Product(string name, decimal price)
        {
            Name = name;
            Price = price;
            LoadTagsFromDatabase(); // Simulate expensive operation
        }
        public override string ToString() =>
            $"Product: {Name}, Price: {Price}, Tags: {string.Join(", ", Tags)}";

        private void LoadTagsFromDatabase()
        {
            // Assume this takes seconds
            Tags.Add(new ("Tech", "Technology related"));
            Tags.Add(new ("Portable", "Easy to carry"));
        }
    }
}