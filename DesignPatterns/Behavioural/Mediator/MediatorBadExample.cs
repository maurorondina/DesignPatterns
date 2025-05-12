public static class MediatorBadExample
{
    public static async Task RunAsync()
    {
        var sysadmin = new Admin("SysAdmin");
        var alice = new User("Alice");

        sysadmin.RegisterUser(alice);
        alice.RegisterAdmin(sysadmin);

        await alice.SendMessageAsync("Hello everyone!");
        await sysadmin.SendMessageAsync("System maintenance at 3 AM");
    }

    // Bad approach - Direct communication between components
    public class User(string name)
    {
        private readonly List<User> _users = new();
        private readonly List<Admin> _admins = new();
        
        public string Name => name;
        public void RegisterUser(User user) => _users.Add(user);
        public void RegisterAdmin(Admin admin) => _admins.Add(admin);

        public async Task SendMessageAsync(string message)
        {
            Console.WriteLine($"{Name} sends: {message}");
            
            // Manually notify all components
            foreach (var user in _users.Where(u => u != this))
                await user.ReceiveMessageAsync(message, isPinned: false);
            foreach (var admin in _admins)
                await admin.ReceiveMessageAsync(message, isPinned: false);
        }

        public async Task ReceiveMessageAsync(string message, bool isPinned)
        {
            await Task.Delay(50); // Simulate async processing
            Console.WriteLine($"[user {Name.ToUpper()} chat] {(isPinned ? "📌 " : "")}{message}");
        }
    }

    public class Admin(string name)
    {
        private readonly List<User> _users = new();
        private readonly List<Admin> _admins = new();
        
        public string Name => name;
        public void RegisterUser(User user) => _users.Add(user);
        public void RegisterAdmin(Admin admin) => _admins.Add(admin);

        public async Task SendMessageAsync(string message)
        {
            Console.WriteLine($"{Name} sends: {message}");
            var formattedMessage = $"[ADMIN] {message}";
            
            // Manually notify all components
            foreach (var user in _users)
                await user.ReceiveMessageAsync(formattedMessage, isPinned: false);
            foreach (var admin in _admins.Where(a => a != this))
                await admin.ReceiveMessageAsync(formattedMessage, isPinned: false);
        }

        public async Task ReceiveMessageAsync(string message, bool isPinned)
        {
            await Task.Delay(50); // Simulate async processing
            Console.WriteLine($"[admin {Name.ToUpper()} chat] {(isPinned ? "🔔 " : "")}{message}");
        }
    }
}