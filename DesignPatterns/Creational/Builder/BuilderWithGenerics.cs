public static class BuilderWithGenerics
{
    public static void Run()
    {
        var director = new Director();

        // Build Cars
        var carBuilder = new CarBuilder();
        Car sportCar = director.ConstructSportsCar(carBuilder);
        Car suvCar = director.ConstructSUV(carBuilder);

        // Build Manuals
        var manualBuilder = new ManualBuilder();
        Manual manualSportCar = director.ConstructSportsCar(manualBuilder);
        Manual manualSuvCar = director.ConstructSUV(manualBuilder);

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

    // BUILDER interface
    public interface IBuilder<T>
    {
        void Reset();
        IBuilder<T> SetCarType(CarType type);
        IBuilder<T> SetSeats(int seats);
        IBuilder<T> SetEngine(Engine engine);
        IBuilder<T> SetWheels(Wheels wheels);
        IBuilder<T> SetDashboard(Dashboard dashboard);
        IBuilder<T> SetGPSNavigator(GPSNavigator? gps);
        IBuilder<T> SetConvertible(bool convertible);
        T Build();  // The build method is called to create the product
    }

    // CONCRETE BUILDERS
    public sealed class CarBuilder : IBuilder<Car>
    {
        private CarType? _type;
        private int? _seats;
        private Engine? _engine;
        private Wheels? _wheels;
        private Dashboard? _dashboard;
        private bool _isConvertible;
        private GPSNavigator? _gps;

        public void Reset()
        {
            _type = null;
            _seats = null;
            _engine = null;
            _wheels = null;
            _dashboard = null;
            _isConvertible = false;
            _gps = null;
        }

        public IBuilder<Car> SetCarType(CarType type)
        {
            _type = type;
            return this;
        }

        public IBuilder<Car> SetSeats(int seats)
        {
            _seats = seats;
            return this;
        }

        public IBuilder<Car> SetEngine(Engine engine)
        {
            _engine = engine;
            return this;
        }

        public IBuilder<Car> SetWheels(Wheels wheels)
        {
            _wheels = wheels;
            return this;
        }

        public IBuilder<Car> SetDashboard(Dashboard dashboard)
        {
            _dashboard = dashboard;
            return this;
        }

        public IBuilder<Car> SetGPSNavigator(GPSNavigator? gps)
        {
            _gps = gps;
            return this;
        }

        public IBuilder<Car> SetConvertible(bool convertible)
        {
            _isConvertible = convertible;
            return this;
        }

        public Car Build()
        {
            ArgumentNullException.ThrowIfNull(_type);
            ArgumentNullException.ThrowIfNull(_seats);
            ArgumentNullException.ThrowIfNull(_engine);
            ArgumentNullException.ThrowIfNull(_wheels);
            ArgumentNullException.ThrowIfNull(_dashboard);

            return new Car
            {
                Type = _type.Value,
                Seats = _seats.Value,
                Engine = _engine,
                Wheels = _wheels,
                Dashboard = _dashboard,
                IsConvertible = _isConvertible,
                GpsNavigator = _gps
            };
        }
    }

    public sealed class ManualBuilder : IBuilder<Manual>
    {
        private CarType? _type;
        private int? _seats;
        private Engine? _engine;
        private Wheels? _wheels;
        private Dashboard? _dashboard;
        private bool _isConvertible;
        private GPSNavigator? _gps;

        public void Reset()
        {
            _type = null;
            _seats = null;
            _engine = null;
            _wheels = null;
            _dashboard = null;
            _isConvertible = false;
            _gps = null;
        }

        public IBuilder<Manual> SetCarType(CarType type)
        {
            _type = type;
            return this;
        }

        public IBuilder<Manual> SetSeats(int seats)
        {
            _seats = seats;
            return this;
        }

        public IBuilder<Manual> SetEngine(Engine engine)
        {
            _engine = engine;
            return this;
        }

        public IBuilder<Manual> SetWheels(Wheels wheels)
        {
            _wheels = wheels;
            return this;
        }

        public IBuilder<Manual> SetDashboard(Dashboard dashboard)
        {
            _dashboard = dashboard;
            return this;
        }

        public IBuilder<Manual> SetConvertible(bool convertible)
        {
            _isConvertible = convertible;
            return this;
        }

        public IBuilder<Manual> SetGPSNavigator(GPSNavigator? gps)
        {
            _gps = gps;
            return this;
        }

        public Manual Build()
        {
            ArgumentNullException.ThrowIfNull(_type);
            ArgumentNullException.ThrowIfNull(_seats);
            ArgumentNullException.ThrowIfNull(_engine);
            ArgumentNullException.ThrowIfNull(_wheels);
            ArgumentNullException.ThrowIfNull(_dashboard);

            return new Manual
            {
                Type = _type.Value,
                Seats = _seats.Value,
                Engine = _engine,
                Wheels = _wheels,
                Dashboard = _dashboard,
                IsConvertible = _isConvertible,
                GpsNavigator = _gps
            };
        }
    }

    // DIRECTOR
    public class Director
    {
        public T ConstructSportsCar<T>(IBuilder<T> builder)
        {
            builder.Reset();
            builder.SetCarType(CarType.Sports)
                .SetSeats(2)
                .SetEngine(new Engine())
                .SetWheels(new Wheels(20))
                .SetDashboard(new Dashboard(true))
                .SetConvertible(true)
                .SetGPSNavigator(new GPSNavigator());
            return builder.Build();
        }

        public T ConstructSUV<T>(IBuilder<T> builder)
        {
            builder.Reset();
            builder.SetCarType(CarType.SUV)
                .SetSeats(5)
                .SetEngine(new Engine())
                .SetWheels(new Wheels(18))
                .SetDashboard(new Dashboard(false));
            return builder.Build();
        }
    }
}