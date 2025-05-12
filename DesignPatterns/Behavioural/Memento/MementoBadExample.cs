public static class MementoBadExample
{
    public static void Run()
    {
        var editor = new TextEditor();
        var history = new History();

        editor.Text = "Hello, World!";
        history.SaveState(editor);

        editor.Text = "Hello, Universe!";
        history.Undo(editor); // Directly manipulates the editor's state
        Console.WriteLine(editor.Text); // Outputs: Hello, World!
    }


    // Problem: Exposing internal state for undo/redo
    public class TextEditor
    {
        public string Text { get; set; } // Public setters break encapsulation
        public int CursorPosition { get; set; }

        // Other methods offered by the TextEditor class to modify its state (e.g., typing text, moving cursor)...
    }

    public record TextEditorState(string Text, int CursorPosition);

    public class History
    {
        private readonly List<TextEditorState> _states = new(); 

        public void SaveState(TextEditor editor) =>
            _states.Add(new (editor.Text, editor.CursorPosition));

        public void Undo(TextEditor editor)
        {
            var prevState = _states.Last();
            editor.Text = prevState.Text; // Directly manipulates the editor's state
            editor.CursorPosition = prevState.CursorPosition;
        }
    }
}