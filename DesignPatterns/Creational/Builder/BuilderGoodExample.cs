public static class BuilderGoodExample
{
    public static void Run()
    {
        // Build Cars
        var carBuilder = new CarBuilder();
        var director = new Director(carBuilder);
        director.Make(CarType.Sports);
        Car sportCar = carBuilder.GetResult();
        director.Make(CarType.SUV);
        Car suvCar = carBuilder.GetResult();

        // Build Manuals
        var manualBuilder = new ManualBuilder();
        director.ChangeBuilder(manualBuilder);
        director.Make(CarType.Sports);
        Manual manualSportCar = manualBuilder.GetResult();
        director.Make(CarType.SUV);
        Manual manualSuvCar = manualBuilder.GetResult();

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
    public interface IBuilder
    {
        void Reset();
        IBuilder SetCarType(CarType type);
        IBuilder SetSeats(int seats);
        IBuilder SetEngine(Engine engine);
        IBuilder SetWheels(Wheels wheels);
        IBuilder SetDashboard(Dashboard dashboard);
        IBuilder SetGPSNavigator(GPSNavigator? gps);
        IBuilder SetConvertible(bool convertible);
    }

    // CONCRETE BUILDERS
    public sealed class CarBuilder : IBuilder
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

        public IBuilder SetCarType(CarType type)
        {
            _type = type;
            return this;
        }

        public IBuilder SetSeats(int seats)
        {
            _seats = seats;
            return this;
        }

        public IBuilder SetEngine(Engine engine)
        {
            _engine = engine;
            return this;
        }

        public IBuilder SetWheels(Wheels wheels)
        {
            _wheels = wheels;
            return this;
        }

        public IBuilder SetDashboard(Dashboard dashboard)
        {
            _dashboard = dashboard;
            return this;
        }

        public IBuilder SetGPSNavigator(GPSNavigator? gps)
        {
            _gps = gps;
            return this;
        }

        public IBuilder SetConvertible(bool convertible)
        {
            _isConvertible = convertible;
            return this;
        }

        public Car GetResult()
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

    public sealed class ManualBuilder : IBuilder
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

        public IBuilder SetCarType(CarType type)
        {
            _type = type;
            return this;
        }

        public IBuilder SetSeats(int seats)
        {
            _seats = seats;
            return this;
        }

        public IBuilder SetEngine(Engine engine)
        {
            _engine = engine;
            return this;
        }

        public IBuilder SetWheels(Wheels wheels)
        {
            _wheels = wheels;
            return this;
        }

        public IBuilder SetDashboard(Dashboard dashboard)
        {
            _dashboard = dashboard;
            return this;
        }

        public IBuilder SetConvertible(bool convertible)
        {
            _isConvertible = convertible;
            return this;
        }

        public IBuilder SetGPSNavigator(GPSNavigator? gps)
        {
            _gps = gps;
            return this;
        }

        public Manual GetResult()
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
        private IBuilder _builder;

        public Director(IBuilder builder)
        {
            _builder = builder;
        }

        public void ChangeBuilder(IBuilder builder) => _builder = builder;

        public void Make(CarType type)
        {
            _builder.Reset();
            switch (type)
            {
                case CarType.Sports:
                    ConstructSportsCar();
                    break;
                case CarType.SUV:
                    ConstructSUV();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
        private void ConstructSportsCar()
        {
            _builder.SetCarType(CarType.Sports)
                .SetSeats(2)
                .SetEngine(new Engine())
                .SetWheels(new Wheels(20))
                .SetDashboard(new Dashboard(true))
                .SetConvertible(true)
                .SetGPSNavigator(new GPSNavigator());
        }

        private void ConstructSUV()
        {
            _builder.SetCarType(CarType.SUV)
                .SetSeats(5)
                .SetEngine(new Engine())
                .SetWheels(new Wheels(18))
                .SetDashboard(new Dashboard(false));
        }
    }
}