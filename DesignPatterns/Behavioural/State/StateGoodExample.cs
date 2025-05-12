public static class StateGoodExample
{
    public static void Run()
    {
        var document = new Document();  // Draft
        document.Publish(UserRole.Editor); // Moderation
        document.Publish(UserRole.Editor); // Moderation
        document.Publish(UserRole.Admin); // Published
    }

    public enum UserRole { Admin, Editor }

    // CONTEXT with state management
    public class Document
    {
        private IDocumentState state;
        public Document()
        {
            state = new DraftState(this); // initial state for new documents
            Console.WriteLine($"Initial state: {state.GetType().Name}");
        }

        public void TransitionTo(IDocumentState newState) => state = newState;
        public void Publish(UserRole user)
        {
            state.Publish(user);
            Console.WriteLine($"State after publish by {user}: {state.GetType().Name}");
        }
        public void Edit()
        {
            state.Edit();
            Console.WriteLine($"State after edit: {state.GetType().Name}");
        }
    }

    // STATE interface with context-aware methods
    public interface IDocumentState
    {
        void Publish(UserRole user);
        void Edit();
    }

    // CONCRETE STATES
    public sealed class DraftState(Document document) : IDocumentState
    {
        public void Publish(UserRole user) => 
            document.TransitionTo(new ModerationState(document));

        public void Edit() => Console.WriteLine("Draft: Editing enabled.");
    }

    public sealed class ModerationState(Document document) : IDocumentState
    {
        public void Publish(UserRole user)
        {
            if (user == UserRole.Admin)
                document.TransitionTo(new PublishedState(document));
            else
                Console.WriteLine("Moderation: ❌ Publish denied. Require admin privileges.");
        }

        public void Edit() => 
            document.TransitionTo(new DraftState(document));
    }

    public sealed class PublishedState(Document document) : IDocumentState
    {
        public void Publish(UserRole user) => 
            Console.WriteLine("Published: ✅ Already published. No action.");

        public void Edit() => 
            document.TransitionTo(new DraftState(document));
    }
}