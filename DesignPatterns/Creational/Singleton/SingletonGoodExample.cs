public static class SingletonGoodExample
{
    public static void Run()
    {
        Logger loggerA = Logger.Instance;
        loggerA.Log("Application started"); // Creates instance and logs

        Logger loggerB = Logger.Instance; 
        loggerB.Log("User logged in"); // Reuses the same instance

        Console.WriteLine($"Same instance? {loggerA == loggerB}"); // Output: True
    }

    // Safe SINGLETON
    public sealed class Logger
    {
        private static readonly Lazy<Logger> _instance = 
            new Lazy<Logger>(() => new Logger(), LazyThreadSafetyMode.ExecutionAndPublication);
        
        private readonly string _logFilePath = "app.log";
        private static readonly Lock _lock = new();
        
        private Logger() { }  // Private constructor to block external instantiation
        
        public static Logger Instance => _instance.Value;

        
        public void Log(string message)
        {
            lock (_lock)
            {
                File.AppendAllText(_logFilePath, $"{DateTime.Now}: {message}\n");
            }
        }
    } 

    // UNSAFE SINGLETON EXAMPLE
    // This is a bad example of a singleton pattern in multiple-threaded environments.
    // It uses a static field to hold the instance, but does not use any locking mechanism.
    // It is not thread-safe and can lead to multiple instances being created.
    public sealed class UnsafeLogger
    {
        private static UnsafeLogger _instance;
        private UnsafeLogger() { }
        
        // Not thread-safe! Two threads could call this simultaneously.
        public static UnsafeLogger Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new UnsafeLogger(); // Race condition here!
                return _instance;
            }
        }
    }
}