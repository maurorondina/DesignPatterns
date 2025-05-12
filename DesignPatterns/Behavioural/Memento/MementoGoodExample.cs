public static class MementoGoodExample
{
    public static void Run()
    {
        var editor = new TextEditor();
        var history = new History();

        editor.TypeText("Hello, World!");
        history.SaveState(editor.Save());

        editor.TypeText(" How are you?");
        history.SaveState(editor.Save());
        Console.WriteLine($"Text: {editor.Text}, Cursor Position: {editor.CursorPosition}"); // Hello, World! How are you?

        var memento = history.Undo();
        if (memento is not null) {
            Console.WriteLine($"Undoing to: {memento.GetName()}");
            editor.Restore(memento);
            Console.WriteLine($"Text: {editor.Text}, Cursor Position: {editor.CursorPosition}"); // Hello, World!
        }
    }


    // ORIGINATOR (TextEditor)
    public class TextEditor
    {
        public string Text { get; private set; } = string.Empty;
        public int CursorPosition { get; private set; }

        // Creates a memento
        public IMemento Save() => new TextEditorMemento(Text, CursorPosition);

        // Restores state from a memento
        public void Restore(IMemento memento)
        {
            if (memento is null)
                return;
            Text = memento.GetText();
            CursorPosition = memento.GetCursorPosition();
        }

        // Methods to modify its state ...
        public void TypeText(string text)
        {
            Text = Text.Insert(CursorPosition, text);
            CursorPosition += text.Length;
        }

        public void MoveCursor(int position) => 
            CursorPosition = Math.Clamp(position, 0, Text.Length);


        // MEMENTO
        public interface IMemento
        {
            // state of the TextEditor
            string GetText();
            int GetCursorPosition();

            // metadata
            string GetName();
        }

        private class TextEditorMemento : IMemento
        {
            private readonly string _text;
            private readonly int _cursorPosition;
            private readonly DateTime _creationTimestamp;
            public TextEditorMemento(string text, int cursorPosition)
            {
                _text = text;
                _cursorPosition = cursorPosition;
                _creationTimestamp = DateTime.Now;
            }
            public string GetText() => _text;
            public int GetCursorPosition() => _cursorPosition;
            public string GetName() => $"{nameof(TextEditor)} - {_creationTimestamp}";
        }
    }


    // CARETAKER (History)
    public class History
    {
        private readonly Stack<TextEditor.IMemento> _undoStack = new();
        private readonly Stack<TextEditor.IMemento> _redoStack = new();

        public void SaveState(TextEditor.IMemento memento)
        {
            _undoStack.Push(memento);
            _redoStack.Clear(); // Clear redo on new action
        }

        public TextEditor.IMemento? Undo()
        {
            if (_undoStack.Count <= 1) return null; // Cannot undo initial state

            var current = _undoStack.Pop();
            _redoStack.Push(current);
            return _undoStack.Peek();
        }

        public TextEditor.IMemento? Redo()
        {
            if (_redoStack.Count == 0) return null;

            var state = _redoStack.Pop();
            _undoStack.Push(state);
            return state;
        }
    }
}

