public static class BridgeBadExample
{
    public static void Run()
    {
        var vectorCircle = new VectorCircle();
        var rasterSquare = new RasterSquare();

        vectorCircle.Draw(); // "Vector Circle"
        rasterSquare.Draw(); // "Raster Square"
    }

    // Base class (Tight coupling between shapes and renderers)
    public abstract class Shape
    {
        public abstract void Draw();
    }

    // Circle implementations
    public class VectorCircle : Shape
    {
        public override void Draw() => Console.WriteLine("Vector Circle");
    }

    public class RasterCircle : Shape
    {
        public override void Draw() => Console.WriteLine("Raster Circle");
    }

    // Square implementations
    public class VectorSquare : Shape
    {
        public override void Draw() => Console.WriteLine("Vector Square");
    }

    public class RasterSquare : Shape
    {
        public override void Draw() => Console.WriteLine("Raster Square");
    }
}