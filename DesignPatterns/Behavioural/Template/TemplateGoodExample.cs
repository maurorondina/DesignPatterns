public static class TemplateGoodExample
{
    public static void Run()
    {
        Console.WriteLine("CSV Import:");
        var csvImporter = new CsvImporter();
        csvImporter.ExecuteImport();

        Console.WriteLine("\nJSON Import:");
        var jsonImporter = new JsonImporter();
        jsonImporter.ExecuteImport();
    }

    public record Person(string Name, int Age);

    // TEMPLATE METHOD: Data Import Framework
    public abstract class PeopleDataImporter
    {
        // Template method defining the import process
        public void ExecuteImport()
        {
            Connect();                                      // Step 1
            try
            {
                var data = ExtractData();                   // Step 2
                var transformed = TransformData(data);      // Step 3
                LoadData(transformed);                      // Step 4
            }
            finally
            {
                Disconnect();                               // Step 5   
            }
        }

        protected virtual void Connect() => 
            Console.WriteLine("Establishing default connection...");

        protected abstract IEnumerable<string> ExtractData();
        
        protected abstract List<Person> TransformData(IEnumerable<string> rawData);

        private void LoadData(List<Person> processedData)   // this step is common for all
        {
            // Simulate loading data into a database or other storage
            foreach (var person in processedData)
                Console.WriteLine($"Loading {person.Name}, Age: {person.Age}"); 
        }

        protected virtual void Disconnect() => 
            Console.WriteLine("Closing default connection...");
    }

    // CONCRETE CSV Importer
    public class CsvImporter : PeopleDataImporter
    {
        protected override IEnumerable<string> ExtractData()
        {
            Console.WriteLine("Reading CSV file lines");
            return ["Alice,25", "Bob,30", "Charlie,35"];
        }

        protected override List<Person> TransformData(IEnumerable<string> rawData)
        {
            Console.WriteLine("Transforming CSV data");
            var transformed = new List<Person>();
            foreach (var line in rawData)
            {
                var parts = line.Split(',');
                transformed.Add(new Person(parts[0], int.Parse(parts[1])));
            }
            return transformed;
        }
    }

    // CONCRETE JSON Importer
    public class JsonImporter : PeopleDataImporter
    {
        protected override IEnumerable<string> ExtractData()
        {
            Console.WriteLine("Reading JSON file content");
            return ["{\"Name\":\"Jessica\",\"Age\":40}", "{\"Name\":\"Martin\",\"Age\":50}"];
        }

        protected override List<Person> TransformData(IEnumerable<string> rawData)
        {
            Console.WriteLine("Transforming JSON data");
            var transformed = new List<Person>();
            foreach (var json in rawData)
            {
                var person = System.Text.Json.JsonSerializer.Deserialize<Person>(json);
                transformed.Add(person);
            }
            return transformed;
        }
    }
}