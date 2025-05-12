public static class ChainOfResponsibilityBadExample
{
    public static void Run()
    {
        var purchaseProcessor = new PurchaseProcessor();

        // Test with different amounts
        purchaseProcessor.ProcessRequest(500m);   // Manager
        purchaseProcessor.ProcessRequest(2000m);  // Director
        purchaseProcessor.ProcessRequest(7000m);  // Vice Presidentx
    }

    public class PurchaseProcessor
    {
        public void ProcessRequest(decimal amount)
        {
            var manager = new Manager();
            var director = new Director();
            var vicePresident = new VicePresident();

            if (manager.Approve(amount))
                Console.WriteLine("Request approved by Manager.");
            else if (director.Approve(amount))
                Console.WriteLine("Request approved by Director.");
            else if (vicePresident.Approve(amount))
                Console.WriteLine("Request approved by Vice President.");
            else
                Console.WriteLine("Request denied.");
        }
    }

    public class Manager
    {
        public bool Approve(decimal amount) => amount <= 1000m;
    }

    public class Director
    {
        public bool Approve(decimal amount) => amount <= 5000m;
    }

    public class VicePresident
    {
        public bool Approve(decimal amount) => true; // Always approves
    }
}