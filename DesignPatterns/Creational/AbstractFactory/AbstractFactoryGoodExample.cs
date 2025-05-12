public static class AbstractFactoryGoodExample
{
    public static void Run()
    {
        var theme = "dark"; // or "light"
        IThemeFactory factory = theme switch
        {
            "dark" => new DarkThemeFactory(),
            "light" => new LightThemeFactory(),
            _ => throw new ArgumentException("Invalid theme")
        };

        var uiThemeApp = new UIThemeApplication(factory);
        uiThemeApp.BuildUI();
        uiThemeApp.RenderUI();
    }

    public class UIThemeApplication(IThemeFactory factory)
    {
        private IButton _button;
        private ITextBox _textBox;

        public void BuildUI()
        {
            _button = factory.CreateButton();
            _textBox = factory.CreateTextBox();
        }

        public void RenderUI()
        {
            _button.Render();
            _textBox.Display();
        }
    }

    // ABSTRACT PRODUCT INTERFACES
    public interface IButton
    {
        void Render();
    }

    public interface ITextBox
    {
        void Display();
    }

    // CONCRETE PRODUCTS
    public sealed class DarkButton : IButton
    {
        public void Render() => Console.WriteLine("Dark theme button rendered");
    }

    public sealed class DarkTextBox : ITextBox
    {
        public void Display() => Console.WriteLine("Dark theme textbox displayed");
    }

    public sealed class LightButton : IButton
    {
        public void Render() => Console.WriteLine("Light theme button rendered");
    }

    public sealed class LightTextBox : ITextBox
    {
        public void Display() => Console.WriteLine("Light theme textbox displayed");
    }

    // ABSTRACT FACTORY INTERFACE
    public interface IThemeFactory
    {
        IButton CreateButton();
        ITextBox CreateTextBox();
    }

    // CONCRETE FACTORIES
        public sealed class DarkThemeFactory : IThemeFactory
    {
        public IButton CreateButton() => new DarkButton();
        public ITextBox CreateTextBox() => new DarkTextBox();
    }

    public sealed class LightThemeFactory : IThemeFactory
    {
        public IButton CreateButton() => new LightButton();
        public ITextBox CreateTextBox() => new LightTextBox();
    }
}