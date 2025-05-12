public static class FactoryMethodGoodExample
{
    public static void Run()
    {
        var userController = new UserController();
        userController.ShowProfile();

        var documentationController = new DocumentationController();
        documentationController.ShowGuide();
    }

    // Framework Code
    public interface IViewEngine // PRODUCT
    {
        string Generate(string viewFileName, Dictionary<string, object> data);
    }

    public class DefaultViewEngine : IViewEngine // CONCRETE PRODUCT
    {
        public string Generate(string viewFileName, Dictionary<string, object> data)
        {
            return $"Processed by DEFAULT engine: {viewFileName}";
        }
    }

    public abstract class PageController // CREATOR
    {
        public void RenderView(string viewFileName, Dictionary<string, object> data)
        {
            // Delegates creation to factory method
            var engine = CreateViewEngine();
            var generatedView = engine.Generate(viewFileName, data);
            Console.WriteLine(generatedView);
        }

        // Factory method (default implementation)
        protected virtual IViewEngine CreateViewEngine() => new DefaultViewEngine();
    }

    // Developer’s Code (extends Framework)
    public class MarkdownViewEngine : IViewEngine // CONCRETE PRODUCT
    {
        public string Generate(string viewFileName, Dictionary<string, object> data)
        {
            return $"Processed by MARKDOWN engine: {viewFileName}";
        }
    }

    public class MarkdownController : PageController // CONCRETE CREATOR
    {
        // Override factory method to use Markdown
        protected override IViewEngine CreateViewEngine() 
            => new MarkdownViewEngine();
    }

    // Developer’s Code (Controllers definition)
    public class UserController : PageController
    {
        public void ShowProfile()
        {
            var data = new Dictionary<string, object> { {"Username", "Alice"} };
            RenderView("profile.html", data); // Always uses DefaultViewEngine
        }
    }

    public class DocumentationController : MarkdownController
    {
        public void ShowGuide()
        {
            var data = new Dictionary<string, object> { {"Page", "API Reference"} };
            RenderView("guide.md", data); // Uses Custom Engine (MarkdownViewEngine)
        }
    }
}