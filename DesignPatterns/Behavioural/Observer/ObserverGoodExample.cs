public static class ObserverGoodExample
{
    public static void Run()
    {
        var market = new StockMarket();
        var display = new StockPriceDisplay();
        var alerts = new StockAlertService();

        // Subscribe observers
        using (market.Subscribe(display))
        using (market.Subscribe(alerts))
        {
            market.UpdateStockPrice("AAPL", 150.00m);
            market.UpdateStockPrice("AAPL", 158.00m); // 5.33% increase
            market.UpdateStockPrice("MSFT", 300.00m);

            // (optional) Graceful shutdown → observers are notified (onCompleted())
            market.Shutdown(); 
        }

        // Unsubscribed here; further updates won't notify
        market.UpdateStockPrice("AAPL", 160.00m); 
    }

    // Stock Update Data
    public record StockPriceUpdate(string Symbol, decimal OldPrice, decimal NewPrice);

    // PUBLISHER (or SUBJECT)
    public sealed class StockMarket : IObservable<StockPriceUpdate>
    {
        private readonly Dictionary<string, decimal> _stockPrices = new();
        private readonly List<IObserver<StockPriceUpdate>> _observers = new();
        private readonly Lock _lock = new();

        public IDisposable Subscribe(IObserver<StockPriceUpdate> observer)
        {
            lock (_lock)
            {
                if (!_observers.Contains(observer))
                    _observers.Add(observer);
            }

            return new Unsubscriber(() =>
            {
                lock (_lock)
                    _observers.Remove(observer);
            });
        }

        public void UpdateStockPrice(string symbol, decimal newPrice)
        {
            decimal oldPrice = _stockPrices.GetValueOrDefault(symbol);
            _stockPrices[symbol] = newPrice;

            var update = new StockPriceUpdate(symbol, oldPrice, newPrice);
            NotifyObservers(update);
        }

        private void NotifyObservers(StockPriceUpdate update)
        {
            IObserver<StockPriceUpdate>[] observersCopy;
            lock (_lock)
                observersCopy = _observers.ToArray();

            foreach (var observer in observersCopy)
                observer.OnNext(update);
        }

        public void Shutdown()
        {
            lock (_lock)
            {
                foreach (var observer in _observers)
                    observer.OnCompleted();
                _observers.Clear();
            }
        }

        private sealed class Unsubscriber : IDisposable
        {
            private readonly Action _unsubscribe;
            public Unsubscriber(Action unsubscribe) => _unsubscribe = unsubscribe;
            public void Dispose() => _unsubscribe();
        }
    }

    // SUBSCRIBERs (or OBSERVERs)
    public sealed class StockPriceDisplay : IObserver<StockPriceUpdate>
    {
        public void OnNext(StockPriceUpdate update) =>
            Console.WriteLine($"[Display] {update.Symbol}: {update.OldPrice:C} → {update.NewPrice:C}");

        public void OnError(Exception ex) =>
            Console.WriteLine($"[Display] Error: {ex.Message}");

        public void OnCompleted() =>
            Console.WriteLine("[Display] Monitoring stopped.");
    }

    public sealed class StockAlertService : IObserver<StockPriceUpdate>
    {
        private const decimal Threshold = 0.05m; // 5% change

        public void OnNext(StockPriceUpdate update)
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

        public void OnError(Exception ex) =>
            Console.WriteLine($"[Alert] Error: {ex.Message}");

        public void OnCompleted() =>
            Console.WriteLine("[Alert] Service stopped.");
    } 
}