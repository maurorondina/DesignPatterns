public static class CommandGoodExample
{
    public static void Run()
    {
        Light light = new(isOn: false);
        Thermostat thermostat = new(temperature: 20);
        CommandInvoker invoker = new();

        // Execute Commands
        invoker.ExecuteCommand(new LightPowerCommand(light, turnOn: true));
        invoker.ExecuteCommand(new SetTemperatureCommand(thermostat, 22));

        // Undo Commands (LIFO Order)
        invoker.UndoLastCommand(); // Undo temperature change
        invoker.UndoLastCommand(); // Undo light on (turns it off)
    }


    // RECEIVER: Light
    public class Light(bool isOn)
    {
        public bool IsOn { get; private set; } = isOn;
        public void TurnOn() { IsOn = true; Console.WriteLine("Light: On"); }
        public void TurnOff() { IsOn = false; Console.WriteLine("Light: Off"); }
    }

    // RECEIVER: Thermostat
    public class Thermostat(int temperature)
    {
        public int CurrentTemperature { get; private set; } = temperature;
        public void SetTemperature(int temp)
        {
            CurrentTemperature = temp;
            Console.WriteLine($"Thermostat: {temp}°C");
        }
    }

    // COMMAND INTERFACE
    public interface ICommand
    {
        void Execute();
        void Undo();
    }

    // CONCRETE COMMAND: Toggle Light
    public class LightPowerCommand(Light light, bool turnOn) : ICommand
    {
        private bool _previousState;

        public void Execute()
        {
            _previousState = light.IsOn;
            if (turnOn) light.TurnOn();
            else light.TurnOff();
        }

        public void Undo()
        {
            if (_previousState) light.TurnOn();
            else light.TurnOff();
        }
    }

    // CONCRETE COMMAND: Set Thermostat Temperature
    public class SetTemperatureCommand(Thermostat thermostat, int newTemperature) : ICommand
    {
        private int _previousTemperature;

        public void Execute()
        {
            _previousTemperature = thermostat.CurrentTemperature;
            thermostat.SetTemperature(newTemperature);
        }

        public void Undo() => thermostat.SetTemperature(_previousTemperature);
    }

    // INVOKER (Manages Command History)
    public class CommandInvoker
    {
        private readonly Stack<ICommand> _commandHistory = new();

        public void ExecuteCommand(ICommand command)
        {
            command.Execute();
            _commandHistory.Push(command);
        }

        public void UndoLastCommand()
        {
            if (_commandHistory.TryPop(out var command))
                command.Undo();
        }
    }
}