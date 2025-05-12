public static class ObserverBadExample
{
    public static void Run()
    {
        var stockMarket = new StockMarket();

        stockMarket.UpdateStockPrice("AAPL", 150.00m);
        stockMarket.UpdateStockPrice("AAPL", 158.00m); // 5.33% increase
        stockMarket.UpdateStockPrice("MSFT", 300.00m);
    }

    // Stock Update Data
    public record StockPriceUpdate(string Symbol, decimal OldPrice, decimal NewPrice);

    // StockMarket (Subject) directly manages concrete observers
    public class StockMarket
    {
        // Hardcoded dependencies (no interfaces)
        private readonly StockPriceDisplay _priceDisplay = new();
        private readonly StockAlertService _alertService = new();
        private readonly Dictionary<string, decimal> _stockPrices = new();

        public void UpdateStockPrice(string symbol, decimal newPrice)
        {
            decimal oldPrice = _stockPrices.GetValueOrDefault(symbol);
            _stockPrices[symbol] = newPrice;

            // Directly call observer methods (tight coupling)
            StockPriceUpdate stockPriceUpdate = new (symbol, oldPrice, newPrice);
            _priceDisplay.UpdateDisplay(stockPriceUpdate);
            _alertService.TriggerAlert(stockPriceUpdate);
        }
    }

    // Services (Observers) with no shared interface
    public class StockPriceDisplay
    {
        public void UpdateDisplay(StockPriceUpdate update) =>
            Console.WriteLine($"[Display] {update.Symbol}: {update.OldPrice:C} → {update.NewPrice:C}");
    }

    public class StockAlertService
    {
        private const decimal Threshold = 0.05m; // 5% change

        public void TriggerAlert(StockPriceUpdate update)
        {
            if (update.OldPrice == 0)
            {
                Console.WriteLine($"[Alert] New stock: {update.Symbol} @ {update.NewPrice:C}");
                return;
            }

            decimal change = Math.Abs(update.NewPrice - update.OldPrice) / update.OldPrice;
            if (change >= Threshold)
                Console.WriteLine($"[Alert] {update.Symbol} changed by {change:P2}!");
        }
    }
}