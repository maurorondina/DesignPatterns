public static class AbstractFactoryBadExample
{
    public static void Run()
    {
        var uiThemeApp = new UIThemeApplication("dark");
        uiThemeApp.BuildUI();
    }

    public class UIThemeApplication(string theme)
    {
        public void BuildUI()
        {
            if (theme == "dark")
            {
                var button = new DarkButton();
                var textBox = new DarkTextBox();
                
                // Imagine these being used elsewhere
                button.Render();
                textBox.Display();
            }
            else
            {
                var button = new LightButton();
                var textBox = new LightTextBox();
                
                button.Render();
                textBox.Display();
            }
        }
    }

    // Concrete component implementations
    public class DarkButton
    {
        public void Render() => Console.WriteLine("Dark theme button rendered");
    }

    public class LightButton
    {
        public void Render() => Console.WriteLine("Light theme button rendered");
    }

    public class DarkTextBox
    {
        public void Display() => Console.WriteLine("Dark theme textbox displayed");
    }

    public class LightTextBox
    {
        public void Display() => Console.WriteLine("Light theme textbox displayed");
    }
}