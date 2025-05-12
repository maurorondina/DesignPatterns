public static class FacthoryMethodBadExample
{
    public static void Run()
    {
        var userController = new UserController();
        userController.ShowProfile();
    }

    // Framework Code
    public interface IViewEngine
    {
        string Generate(string viewFileName, Dictionary<string, object> data);
    }

    public class DefaultViewEngine : IViewEngine
    {
        public string Generate(string viewFileName, Dictionary<string, object> data)
        {
            return $"Rendered by DEFAULT engine: {viewFileName}";  // HTML generation logic
        }
    }

    public abstract class PageController
    {
        public void RenderView(string viewFileName, Dictionary<string, object> data)
        {
            var viewEngine = new DefaultViewEngine();   // Problem: Hardcoded dependency.
            var generatedHtml = viewEngine.Generate(viewFileName, data);
            Console.WriteLine(generatedHtml);
        }
    }

    // Developer’s Code (Cannot Change Engine)
    public class UserController : PageController
    {
        public void ShowProfile()
        {
            var data = new Dictionary<string, object> { {"Username", "Alice"} };
            RenderView("profile.html", data); // Always uses DefaultViewEngine
        }
    }
}