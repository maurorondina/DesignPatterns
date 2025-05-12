// MEDIATOR pattern with Observer pattern using structured async/await and events for message handling
public static class MediatorWithObserver
{
    public static async Task RunAsync()
    {
        var chat = new ChatMediator();
        var sysadmin = new Admin("SysAdmin", chat);
        var moderator = new Admin("Moderator", chat);
        var alice = new User("Alice", chat);
        var bob = new User("Bob", chat);

        // Subscribe members to the mediator's events
        chat.Subscribe(sysadmin);
        chat.Subscribe(moderator);
        chat.Subscribe(alice);
        chat.Subscribe(bob);

        await alice.SendMessageAsync("Hello everyone!");
        await sysadmin.SendMessageAsync("System maintenance at 3 AM");
        await moderator.PinMessageAsync("Important update: Please read rules!");
    }

    // Event arguments class for message events
    public class MessageEventArgs : EventArgs
    {
        public ChatMember Sender { get; }
        public string Message { get; }
        public bool IsPinned { get; }

        public MessageEventArgs(ChatMember sender, string message, bool isPinned)
        {
            Sender = sender;
            Message = message;
            IsPinned = isPinned;
        }
    }

    // COMPONENT abstraction (subscriber)
    public abstract class ChatMember
    {
        private readonly string _name;
        protected readonly ChatMediator _chat;

        public ChatMember(string name, ChatMediator chat)
        {
            _name = name;
            _chat = chat;
        }

        public string Name => _name;

        public async Task SendMessageAsync(string message)
        {
            Console.WriteLine($"{Name} sends: {message}");
            await _chat.BroadcastAsync(this, message, false);
        }

        public abstract Task OnMessageReceivedAsync(object sender, MessageEventArgs e); // UPDATE method

        protected async Task ProcessMessage()
        {
            await Task.Delay(50); // Simulate async processing
        }
    }

    // CONCRETE COMPONENT 1
    public class User : ChatMember
    {
        public User(string name, ChatMediator chat) : base(name, chat) { }

        public override async Task OnMessageReceivedAsync(object sender, MessageEventArgs e)
        {
            if (e.Sender == this) return;   // Skip messages from self
            await HandleMessageAsync(e);
        }

        private async Task HandleMessageAsync(MessageEventArgs e)
        {
            try
            {
                await ProcessMessage();
                Console.WriteLine($"[user {Name.ToUpper()} chat] {(e.IsPinned ? "📌 " : "")}{e.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing message for {Name}: {ex.Message}");
            }
        }
    }

    // CONCRETE COMPONENT 2
    public class Admin : ChatMember
    {
        public Admin(string name, ChatMediator chat) : base(name, chat) { }

        public override async Task OnMessageReceivedAsync(object sender, MessageEventArgs e)
        {
            if (e.Sender == this) return;   // Skip messages from self
            await HandleMessageAsync(e);
        }

        private async Task HandleMessageAsync(MessageEventArgs e)
        {
            try
            {
                await ProcessMessage();
                Console.WriteLine($"[user {Name.ToUpper()} chat] {(e.IsPinned ? "📌 " : "")}{e.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing message for {Name}: {ex.Message}");
            }
        }

        public async Task PinMessageAsync(string message)
        {
            Console.WriteLine($"{Name} pins: {message}");
            await _chat.BroadcastAsync(this, message, true);
        }
    }

    // MEDIATOR implementing the publisher with events
    public class ChatMediator
    {
        private readonly Lock _lock = new();
        public delegate Task AsyncMessageHandler(object sender, MessageEventArgs e);
        public event AsyncMessageHandler MessageBroadcasted;

        private readonly MessagePinManager _pinManager = new MessagePinManager();
        public string PinnedMessage => _pinManager.PinnedMessage;
        
        public void Subscribe(ChatMember member)
        {
            // Register the member's event handler
            lock (_lock)
                MessageBroadcasted += member.OnMessageReceivedAsync;
        }
        
        public void Unsubscribe(ChatMember member)
        {
            // Unregister the member's event handler
            lock (_lock)
                MessageBroadcasted -= member.OnMessageReceivedAsync;
        }

        public async Task BroadcastAsync(ChatMember sender, string message, bool isPinned)
        {
            var messageToSend = message;
            if (sender is Admin)
            {
                messageToSend = $"[ADMIN] {message}";
                if (isPinned)
                {
                    _pinManager.PinMessage(message);
                    messageToSend = $"PINNED: {messageToSend}";
                }
            }

            // NOTIFY all subscribers and await their responses
            if (MessageBroadcasted is not null)
            {
                var eventArgs = new MessageEventArgs(sender, messageToSend, isPinned);
                var invocationTasks = MessageBroadcasted
                    .GetInvocationList()
                    .Cast<AsyncMessageHandler>()
                    .Select(handler => handler(this, eventArgs));
                await Task.WhenAll(invocationTasks);
            }
        }
    }

    private class MessagePinManager
    {
        private readonly Lock _lock = new();
        private string _pinnedMessage = string.Empty;

        public string PinnedMessage
        {
            get
            {
                lock (_lock)
                    return _pinnedMessage;
            }
        }

        public void PinMessage(string message)
        {
            lock (_lock)
                _pinnedMessage = message;
        }
    }
}