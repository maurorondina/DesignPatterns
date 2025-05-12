public static class BridgeGoodExample
{
    public static void Run()
    {
        IRenderer vector = new VectorRenderer();
        IRenderer raster = new RasterRenderer();

        Shape circle = new Circle(vector, 5);
        Shape square = new Square(raster, 10);

        circle.Draw(); // "Vector Circle (radius: 5)"
        square.Draw(); // "Raster Square (side: 10)"

        // You can also change the rendering strategy at runtime without modifying the shape
        circle.Renderer = raster;
        circle.Draw(); // "Raster Circle (radius: 5)"
    }

    // IMPLEMENTATION
    public interface IRenderer
    {
        void RenderCircle(float radius);
        void RenderSquare(float side);
    }

    // CONCRETE IMPLEMENTATIONS
    public sealed class VectorRenderer : IRenderer
    {
        public void RenderCircle(float radius) =>
            Console.WriteLine($"Vector Circle (radius: {radius})");

        public void RenderSquare(float side) =>
            Console.WriteLine($"Vector Square (side: {side})");
    }

    public sealed class RasterRenderer : IRenderer
    {
        public void RenderCircle(float radius) =>
            Console.WriteLine($"Raster Circle (radius: {radius})");

        public void RenderSquare(float side) =>
            Console.WriteLine($"Raster Square (side: {side})");
    }

    // ABSTRACTION
    public abstract class Shape(IRenderer renderer)
    {
        public IRenderer Renderer { get; set; } = renderer;
        public abstract void Draw();
    }

    // REFINED ABSTRACTIONS
     public class Circle(IRenderer Renderer, float Radius) : Shape(Renderer)
    {
        public override void Draw() => Renderer.RenderCircle(Radius);
    }

    public class Square(IRenderer Renderer, float Side) : Shape(Renderer)
    {
        public override void Draw() => Renderer.RenderSquare(Side);
    }
}