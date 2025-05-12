public static class PrototypeWithRegistry
{
    public static void Run()
    {
        List<string> CreateSkills(string role)
        {
            // Simulate expensive base setup
            return role switch
            {
                "attacker" => new List<string> { "Slash", "Pierce" },
                "defender" => new List<string> { "Shield Block", "Parry" },
                _ => throw new ArgumentOutOfRangeException(nameof(role), role, null)
            };
        }

        var attacker = new User
        {
            Username = "Bob",
            Role = "attacker",
            Skills = CreateSkills("attacker")
        };
        Console.WriteLine(attacker);

        // Clone and customize prototypes
        var prototypeRegistry = new PrototypeRegistry();
        prototypeRegistry.Register("attacker", attacker);

        var attacker2 = prototypeRegistry.GetPrototype<User>("attacker")!         // Clone using the registry
                                           .With(x => x.Skills.Add("Fireball"));    // and modify the clone
        Console.WriteLine(attacker2);

        var attacker3 = attacker2.CloneWith(x => x.Username = "Alice"); // Clone and modify the clone
        Console.WriteLine(attacker3);
    }

    // PROTOTYPE
    public class User : ICloneable
    {
        public required string Username { get; set; }
        public required string Role { get; init; }
        public required List<string> Skills { get; init; }

        public object Clone() =>
            new User
            {
                Username = Username,
                Role = Role,
                Skills = [.. Skills] // Deep copy of the Skills list
            };

        public override string ToString() =>
            $"User: {{ Username: {Username}, Role: {Role}, Permissions: [{string.Join(", ", Skills)}] }}";
    }

    // PROTOTYPE REGISTRY
    public class PrototypeRegistry
    {
        private Dictionary<string, ICloneable> _prototypes = new();

        public void Register(string key, ICloneable prototype) =>
            _prototypes[key] = prototype;

        public T? GetPrototype<T>(string key) where T : class, ICloneable
        {
            if (_prototypes.ContainsKey(key))
                return _prototypes[key].Clone() as T;
            throw new KeyNotFoundException($"Prototype with key '{key}' not found.");
        }
    }
}

// PROTOTYPE EXTENSIONS
public static class PrototypeExtensions
{
    public static T CloneWith<T>(this T prototype, Action<T> action) where T : ICloneable
    {
        var clone = (T)prototype.Clone();
        action(clone);
        return clone;
    }
    public static T With<T>(this T prototype, Action<T> action) where T : ICloneable
    {
        action(prototype);
        return prototype;
    }
}