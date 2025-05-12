using System.Collections;

public static class IteratorWithEnumerable
{
    public static void Run()
    {
        var nodeE = new BinaryNode<string>("E");
        var nodeD = new BinaryNode<string>("D");
        var nodeC = new BinaryNode<string>("C");
        var nodeB = new BinaryNode<string>("B", nodeD, nodeE);
        var nodeA = new BinaryNode<string>("A", nodeB, nodeC);
        var tree = new Tree<string>(nodeA);
        Console.WriteLine(tree); // Output: [A, Left: [B, Left: [D], Right: [E]], Right: [C]]

        // Using the iterator
        var inOrderIterator = tree.GetEnumerator();
        Console.Write("In-Order: ");
        while (inOrderIterator.MoveNext())
            Console.Write($"{inOrderIterator.Current} ");
        inOrderIterator.Reset();
        Console.WriteLine(); // Output: D B E A C

        // Using the enumerable methods
        Console.Write("Depth-First: ");
        foreach (var value in tree.DepthFirst())
            Console.Write($"{value} ");
        Console.WriteLine(); // Output: A B D E C

        // Using the iterator extracted from the enumerable
        var breadthFirstIterator = tree.BreadthFirst().GetEnumerator();
        Console.Write("Breadth-First: ");
        while (breadthFirstIterator.MoveNext())
            Console.Write($"{breadthFirstIterator.Current} ");
        Console.WriteLine(); // Output: A B C D E

        // You can also use LINQ on the IEnumerable
        var lastValue = tree.LastOrDefault();
        Console.WriteLine($"Last value in In-Order: {lastValue}"); // Output: C
        lastValue = tree.BreadthFirst().LastOrDefault();
        Console.WriteLine($"Last value in readth-First: {lastValue}"); // Output: E
    }

    public class BinaryNode<T>(T value, BinaryNode<T> left = null, BinaryNode<T> right = null)
    {
        public T Value { get; init; } = value;
        public BinaryNode<T> Left { get; init; } = left;
        public BinaryNode<T> Right { get; init; } = right;

        public override string ToString() =>
            $"[{Value}" +
            $"{(Left is not null ? $", Left: {Left}" : string.Empty)}" +
            $"{(Right is not null ? $", Right: {Right}" : string.Empty)}]";
    }

    // CONCRETE COLLECTION
    public class Tree<T>(BinaryNode<T> root) : IEnumerable<T>
    {
        public BinaryNode<T> Root => root;
        public override string ToString() => $"{Root}";

        // Implementing IEnumerable<T> to allow direct use of Iterator (and foreach, LINQ, etc.)
        #region IEnumerable Members
        public IEnumerator<T> GetEnumerator() => new InOrderIterator<T>(this);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        #endregion

        // Methods that return IEnumerable<T> without
        // directly implementing the Iterator (IEnumerator)
        // and calling GetEnumerator() explicitly.
        public IEnumerable<T> DepthFirst()
        {
            if (Root == null) yield break;

            var stack = new Stack<BinaryNode<T>>();
            stack.Push(Root);

            while (stack.Count > 0)
            {
                var currentNode = stack.Pop();
                yield return currentNode.Value;

                if (currentNode.Right != null) stack.Push(currentNode.Right);
                if (currentNode.Left != null) stack.Push(currentNode.Left);
            }
        }

        public IEnumerable<T> BreadthFirst()
        {
            if (Root == null) yield break;

            var queue = new Queue<BinaryNode<T>>();
            queue.Enqueue(Root);

            while (queue.Count > 0)
            {
                var currentNode = queue.Dequeue();
                yield return currentNode.Value;

                if (currentNode.Left != null) queue.Enqueue(currentNode.Left);
                if (currentNode.Right != null) queue.Enqueue(currentNode.Right);
            }
        }
    }

    // CONCRETE ITERATOR
    public class InOrderIterator<T> : IEnumerator<T>
    {
        private readonly Stack<BinaryNode<T>> _stack = new();
        private BinaryNode<T> _current;

        public InOrderIterator(Tree<T> tree)
        {
            _current = tree.Root;
            PushLeft(_current);
        }

        private void PushLeft(BinaryNode<T> node)
        {
            while (node != null)
            {
                _stack.Push(node);
                node = node.Left;
            }
        }

        public T Current => _current.Value;

        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            if (_stack.Count == 0) return false;

            _current = _stack.Pop();
            PushLeft(_current.Right);
            return true;
        }

        public void Reset()
        {
            _current = null;
            _stack.Clear();
        }

        public void Dispose() { }
    }
}