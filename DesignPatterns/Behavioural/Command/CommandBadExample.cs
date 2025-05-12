public static class CommandBadExample
{
    public static void Run()
    {
        Light light = new(isOn: false);
        Thermostat thermostat = new(temperature: 20);
        RemoteControl remoteControl = new(light, thermostat);

        remoteControl.PressLightButton(turnOn: true);
        remoteControl.PressThermostatButton(temperature: 22);
    }
    
    public class Light(bool isOn)
    {
        public bool IsOn { get; private set; } = isOn;
        public void TurnOn() { IsOn = true; Console.WriteLine("Light: On"); }
        public void TurnOff() { IsOn = false; Console.WriteLine("Light: Off"); }
    }

    public class Thermostat(int temperature)
    {
        public int CurrentTemperature { get; private set; } = temperature;
        public void SetTemperature(int temp)
        {
            CurrentTemperature = temp;
            Console.WriteLine($"Thermostat: {temp}°C");
        }
    }

    public class RemoteControl(Light light, Thermostat thermostat)
    {
        public void PressLightButton(bool turnOn)
        {
            if (turnOn) light.TurnOn();
            else light.TurnOff();
        }

        public void PressThermostatButton(int temperature)
        {
            thermostat.SetTemperature(temperature);
        }
    }
}