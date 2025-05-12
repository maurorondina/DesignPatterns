public static class IteratorGoodCode
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

        var depthFirstIterator = tree.CreateDepthFirstIterator();
        Console.Write("Depth-First: ");
        while (depthFirstIterator.HasMore())
            Console.Write($"{depthFirstIterator.GetNext()} ");
        Console.WriteLine(); // Output: A B D E C

        var breadthFirstIterator = tree.CreateBreadthFirstIterator();
        Console.Write("Breadth-First: ");
        while (breadthFirstIterator.HasMore())
            Console.Write($"{breadthFirstIterator.GetNext()} ");
        Console.WriteLine(); // Output: A B C D E
    }

    // ITERABLE COLLECTION
    public interface IIterableTree<T>
    {
        ITreeIterator<T> CreateDepthFirstIterator();
        ITreeIterator<T> CreateBreadthFirstIterator();
    }

    // CONCRETE COLLECTION
    public class Tree<T>(BinaryNode<T> root) : IIterableTree<T>
    {
        public BinaryNode<T> Root => root;
        public override string ToString() => $"{Root}";

        public ITreeIterator<T> CreateDepthFirstIterator() => new DepthFirstIterator<T>(this);

        public ITreeIterator<T> CreateBreadthFirstIterator() => new BreadthFirstIterator<T>(this);
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

    // ITERATOR
    public interface ITreeIterator<T>
    {
        bool HasMore();
        T GetNext();
    }

    // CONCRETE ITERATORS
    public class DepthFirstIterator<T> : ITreeIterator<T>
    {
        private readonly Stack<BinaryNode<T>> _stack = new();
        private readonly IIterableTree<T> _tree;

        public DepthFirstIterator(IIterableTree<T> tree)
        {
            _tree = tree;
            if (_tree is Tree<T> t)
                _stack.Push(t.Root);
        }

        public bool HasMore() => _stack.Count > 0;

        public T GetNext()
        {
            if (!HasMore())
                throw new InvalidOperationException("No more elements in the iterator.");

            var currentNode = _stack.Pop();
            if (currentNode.Right != null) _stack.Push(currentNode.Right);
            if (currentNode.Left != null) _stack.Push(currentNode.Left);
            return currentNode.Value;
        }
    }

    public class BreadthFirstIterator<T> : ITreeIterator<T>
    {
        private readonly Queue<BinaryNode<T>> _queue = new();
        private readonly IIterableTree<T> _tree;

        public BreadthFirstIterator(IIterableTree<T> tree)
        {
            _tree = tree;
            if (_tree is Tree<T> t)
                _queue.Enqueue(t.Root);
        }

        public bool HasMore() => _queue.Count > 0;

        public T GetNext()
        {
            if (!HasMore())
                throw new InvalidOperationException("No more elements in the iterator.");

            var currentNode = _queue.Dequeue();
            if (currentNode.Left != null) _queue.Enqueue(currentNode.Left);
            if (currentNode.Right != null) _queue.Enqueue(currentNode.Right);
            return currentNode.Value;
        }
    }
}