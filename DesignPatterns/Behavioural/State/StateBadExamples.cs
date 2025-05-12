public static class StateBadExample
{
    public static void Run()
    {
        var document = new Document(Document.State.Published);
        Console.WriteLine($"Initial state: {document.CurrentState}"); // Published
        document.Edit();
        Console.WriteLine($"State after edit: {document.CurrentState}"); // Draft
        document.Publish(UserRole.Editor);
        Console.WriteLine($"State after publish by editor: {document.CurrentState}"); // Moderation
        document.Publish(UserRole.Editor);
        Console.WriteLine($"State after publish by editor again: {document.CurrentState}"); // Moderation
        document.Publish(UserRole.Admin);
        Console.WriteLine($"State after publish by admin: {document.CurrentState}"); // Published
    }

    public enum UserRole { Admin, Editor }

    public class Document(Document.State state)
    {
        public enum State { Draft, Moderation, Published }
        public State CurrentState { get; private set; } = state;

        public void Publish(UserRole user)
        {
            if (CurrentState == State.Draft)
                CurrentState = State.Moderation;
            else if (CurrentState == State.Moderation)
            {
                if (user == UserRole.Admin)
                    CurrentState = State.Published;
                else
                    Console.WriteLine("Moderation: ❌ Publish denied. Require admin privileges.");
            }
            else if (CurrentState == State.Published)
                Console.WriteLine("Published: ✅ Already published. No action.");
        }

        public void Edit()
        {
            if (CurrentState == State.Draft)
                Console.WriteLine("Editing allowed.");
            else if (CurrentState == State.Moderation || CurrentState == State.Published)
                CurrentState = State.Draft;
        }
    }
}