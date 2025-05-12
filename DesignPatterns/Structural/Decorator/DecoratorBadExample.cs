public static class DecoratorBadExample
{
    public static void Run()
    {
        // Need encryption + compression? Use a predefined subclass.
        var storage = new EncryptedAndCompressedFileStorage();
        storage.WriteFile("secret.txt", "Hello World");
        var data = storage.ReadFile("secret.txt");
        Console.WriteLine($"Read: {data}");

        // What if we need caching too? Create a new subclass: EncryptedCompressedCachedFileStorage 😱
    }

    // Base class with core logic
    public class FileStorage
    {
        private readonly Dictionary<string, string> _files = new();

        public virtual void WriteFile(string path, string data)
        {
            _files[path] = data;
            Console.WriteLine($"Base write '{path}': {data}");
        }

        public virtual string ReadFile(string path)
        {
            if (_files.TryGetValue(path, out var data))
                return data;

            throw new FileNotFoundException();
        }
    }

    // Subclasses for every feature combination
    public class EncryptedFileStorage : FileStorage
    {
        public override void WriteFile(string path, string data)
        {
            var encrypted = $"🔒ENCRYPTED({data})🔒";
            base.WriteFile(path, encrypted); // Reuse base logic
        }

        public override string ReadFile(string path)
        {
            var data = base.ReadFile(path);
            return data.Replace("🔒ENCRYPTED(", "").Replace(")🔒", "");
        }
    }

    public class CompressedFileStorage : FileStorage
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

    // A class combining two features
    public class EncryptedAndCompressedFileStorage : FileStorage
    {
        public override void WriteFile(string path, string data)
        {
            var encrypted = $"🔒ENCRYPTED({data})🔒";
            var compressed = $"🗜COMPRESSED({encrypted})🗜";
            base.WriteFile(path, compressed); // Code duplication!
        }

        public override string ReadFile(string path)
        {
            var data = base.ReadFile(path);
            var decompressed = data.Replace("🗜COMPRESSED(", "").Replace(")🗜", "");
            return decompressed.Replace("🔒ENCRYPTED(", "").Replace(")🔒", "");
        }
    }
}