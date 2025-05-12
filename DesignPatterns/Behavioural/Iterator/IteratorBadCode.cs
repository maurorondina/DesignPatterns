public static class IteratorBadCode
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

        string TraverseDepthFirst(BinaryNode<string> node)
        {
            if (node == null)
                return string.Empty;
            var leftTraversal = TraverseDepthFirst(node.Left);
            var rightTraversal = TraverseDepthFirst(node.Right);
            return $"{node.Value} {leftTraversal} {rightTraversal}".Trim();
        }
        Console.WriteLine($"Depth-First: {TraverseDepthFirst(tree.Root)}"); // Output: A B D E C

        string TraverseBreadthFirst(BinaryNode<string> node)
        {
            if (node == null)
                return string.Empty;
            var queue = new Queue<BinaryNode<string>>();
            queue.Enqueue(node);
            var result = string.Empty;

            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                result += $"{current.Value} ";
                if (current.Left != null) queue.Enqueue(current.Left);
                if (current.Right != null) queue.Enqueue(current.Right);
            }
            return result.Trim();
        }
        Console.WriteLine($"Breadth-First: {TraverseBreadthFirst(tree.Root)}"); // Output: A B C D E
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

    public class Tree<T>(BinaryNode<T> root)
    {
        public BinaryNode<T> Root => root;

        public override string ToString() => $"{Root}";
    }
}