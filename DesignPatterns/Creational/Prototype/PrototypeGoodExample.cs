using System.Text.Json;

public static class PrototypeGoodExample
{
    public static void Run()
    {
        var original = new Product("Laptop", 999.99m);

        var shallowclone = (Product)original.ShallowClone();
        shallowclone.Tags.Add(new Product.Tag("Refurbished", "Like new")); // Affects original.Tags too!

        var deepClone = (Product)original.DeepClone();
        deepClone.Tags.Add(new Product.Tag("New", "Perfect")); // Does not affect original.Tags!

        Console.WriteLine($"Original Product: {original}");
        Console.WriteLine($"Shallow Clone Product: {shallowclone}");
        Console.WriteLine($"Deep Clone Product: {deepClone}");
    }

    // PROTOTYPE
    public interface IPrototype
    {
        IPrototype DeepClone();
        IPrototype ShallowClone();
    }

    // CONCRETE PROTOTYPE
    public class Product : IPrototype
    {
        public record Tag(string Name, string Description)
        {
            public override string ToString() => $"{Name} ({Description})";
        }

        public string Name { get; init; }
        public decimal Price { get; init; }
        public List<Tag> Tags { get; init; } = new();

        // Empty constructor for deserialization
        public Product() { }

        // Constructor for initial creation (with costly initialization)
        public Product(string name, decimal price)
        {
            Name = name;
            Price = price;
            LoadTagsFromDatabase(); // Simulate expensive setup
        }

	    // CLONE: they avoid re-initialization
        public IPrototype DeepClone() // Deep clone
        {
            var json = JsonSerializer.Serialize(this);
            return JsonSerializer.Deserialize<Product>(json)!;
        }
        public IPrototype ShallowClone() =>
            (IPrototype)MemberwiseClone(); // only copies the reference of the Tags list

        public override string ToString() => 
            $"Product: {Name}, Price: {Price}, Tags: {string.Join(", ", Tags)}";

        private void LoadTagsFromDatabase()
        {
            // Assume this takes seconds
            Tags.Add(new Tag("Tech", "Technology related"));
            Tags.Add(new Tag("Portable", "Easy to carry"));
        }
    }
}