public static class MediatorGoodExample
{
    public static async Task RunAsync()
    {
        var chat = new ChatMediator();
        var sysadmin = new Admin("SysAdmin", chat);
        var moderator = new Admin("Moderator", chat);
        var alice = new User("Alice", chat);
        var bob = new User("Bob", chat);

        chat.Register(sysadmin);
        chat.Register(moderator);
        chat.Register(alice);
        chat.Register(bob);

        await alice.SendMessageAsync("Hello everyone!");
        await sysadmin.SendMessageAsync("System maintenance at 3 AM");
        await moderator.PinMessageAsync("Important update: Please read rules!");
    }

    // COMPONENT abstraction
    public abstract class ChatMember(string name, ChatMediator chat)
    {
        protected string Name => name;
        protected ChatMediator Chat => chat;

        public async Task SendMessageAsync(string message) {
            Console.WriteLine($"{Name} sends: {message}");
            await Chat.BroadcastAsync(this, message);          // notify the mediator
        }

        public virtual async Task ReceiveMessageAsync(string message, bool isPinned)
        {
            await Task.Delay(50); // Simulate async processinxg
        }
    }

    // CONCRETE COMPONENT 1
    public class User : ChatMember
    {
        public User(string name, ChatMediator chat) : base(name, chat) { }

        public override async Task ReceiveMessageAsync(string message, bool isPinned)
        {
            await base.ReceiveMessageAsync(message, isPinned);
            Console.WriteLine($"[user {Name.ToUpper()} chat] {(isPinned ? "📌 " : "")}{message}");
        }
    }

    // CONCRETE COMPONENT 2
    public class Admin : ChatMember
    {
        public Admin(string name, ChatMediator chat) : base(name, chat) { }

        public override async Task ReceiveMessageAsync(string message, bool isPinned)
        {
            await base.ReceiveMessageAsync(message, isPinned);
            Console.WriteLine($"[admin {Name.ToUpper()} chat] {(isPinned ? "🔔 " : "")}{message}");
        }

        public async Task PinMessageAsync(string message)
        {
            Console.WriteLine($"{Name} pins: {message}");
            await Chat.BroadcastAsync(this, message, isPinned: true);   // notify the mediator
        }
    }

    // MEDIATOR
    public class ChatMediator
    {
        private readonly List<ChatMember> _members = new();

        private readonly MessagePinManager _pinManager = new MessagePinManager();
        public string PinnedMessage => _pinManager.PinnedMessage;

        public void Register(ChatMember member) => _members.Add(member);

        public async Task BroadcastAsync(ChatMember sender, string message, bool isPinned = false)
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

            var tasks = _members
                .Where(m => m != sender) // Notify all members except the sender
                .Select(m => m.ReceiveMessageAsync(messageToSend, isPinned));
            await Task.WhenAll(tasks);
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