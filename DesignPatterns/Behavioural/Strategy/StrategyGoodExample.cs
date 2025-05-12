public static class StrategyGoodExample
{
    public static async Task RunAsync()
    {
        var paymentStrategyFactory = new PaymentStrategyFactory();
        var orderService = new OrderService(paymentStrategyFactory);

        await orderService.ProcessOrderPaymentAsync(PaymentMethod.CreditCard, 100.50m);
        await orderService.ProcessOrderPaymentAsync(PaymentMethod.PayPal, 230.00m);
        await orderService.ProcessOrderPaymentAsync(PaymentMethod.Crypto, 315.75m);
    }

    public enum PaymentMethod { CreditCard, PayPal, Crypto }

    // STRATEGY
    public interface IPaymentStrategy
    {
        Task ProcessPaymentAsync(decimal amount);
    }

    // CONCRETE STRATEGIES
    public class CreditCardPaymentStrategy : IPaymentStrategy
    {
        public async Task ProcessPaymentAsync(decimal amount)
        {
            Console.WriteLine($"Charging credit card: {amount}");
            await Task.Delay(500); // Simulate credit card processing ...
        }
    }
    public class PayPalPaymentStrategy : IPaymentStrategy
    {
        public async Task ProcessPaymentAsync(decimal amount)
        {
            Console.WriteLine($"Charging PayPal: {amount}");
            await Task.Delay(100); // Simulate PayPal processing ...
        }
    }
    public class CryptoPaymentStrategy : IPaymentStrategy
    {
        public async Task ProcessPaymentAsync(decimal amount)
        {
            Console.WriteLine($"Charging crypto: {amount}");
            await Task.Delay(300); // Simulate crypto processing ...
        }
    }

    // FACTORY
    public interface IPaymentStrategyFactory
    {
        IPaymentStrategy GetPaymentStrategy(PaymentMethod method);
    }
    public class PaymentStrategyFactory : IPaymentStrategyFactory
    {
        public IPaymentStrategy GetPaymentStrategy(PaymentMethod method)
        {
            return method switch
            {
                PaymentMethod.CreditCard => new CreditCardPaymentStrategy(),
                PaymentMethod.PayPal => new PayPalPaymentStrategy(),
                PaymentMethod.Crypto => new CryptoPaymentStrategy(),
                _ => throw new NotSupportedException("Payment method not supported")
            };
        }
    }

    // CONTEXT
    public class OrderService(IPaymentStrategyFactory paymentStrategyFactory)
    {
        // Other properties and methods...
        
        public async Task ProcessOrderPaymentAsync(PaymentMethod paymentMethod, decimal amount)
        {
            var paymentStrategy = paymentStrategyFactory.GetPaymentStrategy(paymentMethod);
            await paymentStrategy.ProcessPaymentAsync(amount);
        }
    }
}