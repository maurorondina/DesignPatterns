public static class StrategyBadExample
{
    public static async Task RunAsync()
    {
        var orderService = new OrderService();
        await orderService.ProcessOrderPaymentAsync(PaymentMethod.CreditCard, 100.50m);
        await orderService.ProcessOrderPaymentAsync(PaymentMethod.PayPal, 230.00m);
        await orderService.ProcessOrderPaymentAsync(PaymentMethod.Crypto, 315.75m);
    }

    public enum PaymentMethod { CreditCard, PayPal, Crypto }

    public class OrderService
    {
        // Other properties and methods...

        public async Task ProcessOrderPaymentAsync(PaymentMethod method, decimal amount)
        {
            if (method == PaymentMethod.CreditCard)
                await ChargeCreditCard(amount);
            else if (method == PaymentMethod.PayPal)
                await ChargePayPalAsync(amount);
            else if (method == PaymentMethod.Crypto)
                await ChargeCryptoAsync(amount);
            // ... possibly more conditionals for other payment methods
            else
                throw new NotSupportedException("Payment method not supported");
        }

        private async Task ChargeCreditCard(decimal amount)
        {
            Console.WriteLine($"Charging credit card: {amount}");
            await Task.Delay(500); // Simulate credit card processing ...
        }
        private async Task ChargePayPalAsync(decimal amount)
        {
            Console.WriteLine($"Charging PayPal: {amount}");
            await Task.Delay(100); // Simulate PayPal processing ...
        }
        private async Task ChargeCryptoAsync(decimal amount)
        {
            Console.WriteLine($"Charging crypto: {amount}");
            await Task.Delay(300); // Simulate crypto processing ...
        }
    }
}