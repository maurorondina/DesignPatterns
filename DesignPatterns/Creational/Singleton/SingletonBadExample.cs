public static class SingletonBadExample
{
    public static void Run()
    {
        var loggerA = new Logger();
        loggerA.Log("Started application"); // Writes to app.log

        var loggerB = new Logger(); // Creates a duplicate instance
        loggerB.Log("User logged in"); // Also writes to app.log

        Console.WriteLine($"Same instance? {loggerA == loggerB}"); // Output: False
    }

    public class Logger
    {
        private readonly string _logFilePath = "app.log";
        
        public void Log(string message)
        {
            File.AppendAllText(_logFilePath, $"{DateTime.Now}: {message}\n");
        }
    }
}