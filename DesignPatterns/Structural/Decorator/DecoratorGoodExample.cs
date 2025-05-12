public static class DecoratorGoodExample
{
    public static void Run()
    {
        // Build a stackable pipeline
        IFileStorage storage = new BasicFileStorage();
        storage = new CompressedStorage(storage);
        storage = new EncryptedStorage(storage);

        storage.WriteFile("secret.txt", "Hello World");
        var data = storage.ReadFile("secret.txt");
        Console.WriteLine($"Read: {data}");

        // Add caching later? Just wrap with a new decorator!
    }

    // COMPONENT interface
    public interface IFileStorage
    {
        void WriteFile(string path, string data);
        string ReadFile(string path);
    }

    // CONCRETE COMPONENT (Core implementation)
    public class BasicFileStorage : IFileStorage
    {
        private readonly Dictionary<string, string> _files = new();

        public void WriteFile(string path, string data)
        {
            _files[path] = data;
            Console.WriteLine($"Base write '{path}': {data}");
        }

        public string ReadFile(string path)
        {
            if (_files.TryGetValue(path, out var data))
                return data;

            throw new FileNotFoundException();
        }
    }

    // BASE DECORATOR
    public abstract class FileStorageDecorator : IFileStorage
    {
        protected readonly IFileStorage Storage; // wrapped component

        protected FileStorageDecorator(IFileStorage storage)
        {
            Storage = storage ?? throw new ArgumentNullException(nameof(storage));
        }

        public virtual void WriteFile(string path, string data) =>
            Storage.WriteFile(path, data);

        public virtual string ReadFile(string path) =>
            Storage.ReadFile(path);
    }

    // CONCRETE DECORATORS
    public sealed class EncryptedStorage(IFileStorage storage) : FileStorageDecorator(storage)
    {
        public override void WriteFile(string path, string data)
        {
            var encrypted = $"🔒ENCRYPTED({data})🔒";
            base.WriteFile(path, encrypted); // Reuse base behavior
        }

        public override string ReadFile(string path)
        {
            var data = base.ReadFile(path);
            return data.Replace("🔒ENCRYPTED(", "").Replace(")🔒", "");
        }
    }

    public sealed class CompressedStorage(IFileStorage storage) : FileStorageDecorator(storage)
    {
        public override void WriteFile(string path, string data)
        {
            var compressed = $"🗜COMPRESSED({data})🗜";
            base.WriteFile(path, compressed);
        }

        public override string ReadFile(string path)
        {
            var data = base.ReadFile(path);
            return data.Replace("🗜COMPRESSED(", "").Replace(")🗜", "");
        }
    }
}