public static class ChainOfResponsibilityGoodExample
{
    public static async Task RunAsync()
    {
        // Build the chain
        var manager = new ManagerHandler();
        var director = new DirectorHandler();
        var vp = new VicePresidentHandler();

        manager.SetNext(director).SetNext(vp); // Fluent chaining

        // Simulate requests
        await manager.HandleAsync(new PurchaseRequest(500m));   // Manager
        await manager.HandleAsync(new PurchaseRequest(2500m));  // Director
        await manager.HandleAsync(new PurchaseRequest(10000m)); // Vice President
    }

    public record PurchaseRequest(decimal Amount);

    // HANDLER interface
    public interface IHandler<TRequest> where TRequest : class
    {
        IHandler<TRequest> SetNext(IHandler<TRequest> next);
        Task HandleAsync(TRequest request);
    }

    // BASE HANDLER
    public abstract class HandlerBase<TRequest> : IHandler<TRequest> where TRequest : class
    {
        private IHandler<TRequest>? _next;

        public IHandler<TRequest> SetNext(IHandler<TRequest> next)
        {
            _next = next;
            return next; // Fluent interface for chaining
        }

        public async Task HandleAsync(TRequest request)
        {
            if (await CanHandleAsync(request))
            {
                await ProcessAsync(request);
            }
            else if (_next is not null)
            {
                await _next.HandleAsync(request);
            }
            else
            {
                Console.WriteLine("No handler could process the request.");
            }
        }

        protected abstract Task<bool> CanHandleAsync(TRequest request);
        protected abstract Task ProcessAsync(TRequest request);
    }

    // CONCRETE HANDLERS
    public sealed class ManagerHandler : HandlerBase<PurchaseRequest>
    {
        private const decimal ApprovalLimit = 1000m;

        protected override Task<bool> CanHandleAsync(PurchaseRequest request)
            => Task.FromResult(request.Amount <= ApprovalLimit);

        protected override Task ProcessAsync(PurchaseRequest request)
        {
            Console.WriteLine($"Manager approved: {request.Amount:C}");
            return Task.CompletedTask;
        }
    }

    public sealed class DirectorHandler : HandlerBase<PurchaseRequest>
    {
        private const decimal ApprovalLimit = 5000m;

        protected override Task<bool> CanHandleAsync(PurchaseRequest request)
            => Task.FromResult(request.Amount <= ApprovalLimit);

        protected override Task ProcessAsync(PurchaseRequest request)
        {
            Console.WriteLine($"Director approved: {request.Amount:C}");
            return Task.CompletedTask;
        }
    }

    public sealed class VicePresidentHandler : HandlerBase<PurchaseRequest>
    {
        protected override Task<bool> CanHandleAsync(PurchaseRequest request)
            => Task.FromResult(true); // Always handles the request

        protected override Task ProcessAsync(PurchaseRequest request)
        {
            Console.WriteLine($"Vice President approved: {request.Amount:C}");
            return Task.CompletedTask;
        }
    }
}