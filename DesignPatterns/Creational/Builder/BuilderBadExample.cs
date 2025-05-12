public static class BuilderBadExample
{
    public static void Run()
    {
        var sportCar = new Car  // Car with all properties
        {
            Type = CarType.Sports,
            Seats = 2,
            Engine = new Engine(),
            Wheels = new Wheels(20),
            Dashboard = new Dashboard(true),
            IsConvertible = true,
            GpsNavigator = new GPSNavigator(),
        };

        var suvCar = new Car  // Car with only required properties
        {
            Type = CarType.SUV,
            Seats = 5,
            Engine = new Engine(),
            Wheels = new Wheels(18),
            Dashboard = new Dashboard(false),
        };

        var manualSportCar = new Manual  // Manual for the sportCar
        {
            Type = CarType.Sports,
            Seats = 2,
            Engine = new Engine(),
            Wheels = new Wheels(20),
            Dashboard = new Dashboard(true),
            IsConvertible = true,
            GpsNavigator = new GPSNavigator()
        };

        var manualSuvCar = new Manual  // Manual for the suvCar
        {
            Type = CarType.SUV,
            Seats = 5,
            Engine = new Engine(),
            Wheels = new Wheels(18),
            Dashboard = new Dashboard(false)
        };

        // Print manuals
        Console.WriteLine($"Manual for '{nameof(sportCar)}':\n{manualSportCar.Print()}");
        Console.WriteLine("==========================================");
        Console.WriteLine($"Manual for '{nameof(suvCar)}':\n{manualSuvCar.Print()}");
    }

    // Products definition
    public enum CarType { Sports, SUV }

    public record Engine;
    public record Dashboard(bool HasRevCounter);
    public record Wheels(int DiameterInInches);
    public record GPSNavigator;

    public record Car
    {
        public required CarType Type { get; init; }
        public required int Seats { get; init; }
        public required Engine Engine { get; init; }
        public required Wheels Wheels { get; init; }
        public required Dashboard Dashboard { get; init; }
        public bool IsConvertible { get; init; }
        public GPSNavigator? GpsNavigator { get; init; }
    }

    public record Manual
    {
        public required CarType Type { get; init; }
        public required int Seats { get; init; }
        public required Engine Engine { get; init; }
        public required Wheels Wheels { get; init; }
        public required Dashboard Dashboard { get; init; }
        public bool IsConvertible { get; init; }
        public GPSNavigator? GpsNavigator { get; init; }

        public string Print() =>
            $"""
            Car type: {Type}
            Seats: {Seats}
            Engine: info on engine...
            Wheels: diameter in inches = {Wheels.DiameterInInches}
            Dashboard: {(Dashboard.HasRevCounter ? "Has rev counter" : "No rev counter")}
            Is convertible: {(IsConvertible ? "Yes" : "No")}
            GPS Navigator: {(GpsNavigator is null ? "N/A" : "Info on gps...")}
            """;
    }
}