public static class TemplateBadExample
{
    public static void Run()
    {
        Console.WriteLine("CSV Import:");
        var csvProcessor = new CsvProcessor();
        csvProcessor.Process();

        Console.WriteLine("\nJSON Import:");
        var jsonProcessor = new JsonProcessor();
        jsonProcessor.Process();
    }

    public record Person(string Name, int Age);

    // CSV-specific implementation
    public class CsvProcessor {
        public void Process() {
            // Step 1: simulate establishing a connection to a data source or file
            Console.WriteLine("Establishing default connection...");
            // Step 2: simulate reading lines from a CSV file
            Console.WriteLine("Reading CSV file lines");
            IEnumerable<string> lines = ["Alice,25", "Bob,30", "Charlie,35"];
            // Step 3: simulate transforming CSV data into a list of Person objects
            var people = new List<Person>();
            foreach(var line in lines) {
                var parts = line.Split(',');
                people.Add(new Person(parts[0], int.Parse(parts[1])));
            }
            // Step 4: simulate loading data into the system
            Console.WriteLine("Loading data into the system...");
            foreach (var person in people) {
                Console.WriteLine($"Loading {person.Name}, Age: {person.Age}");
            }
            // Step 5: simulate closing the connection
            Console.WriteLine("Closing default connection...");
        }
    }

    // JSON-specific implementation
    public class JsonProcessor {
        public void Process() {
            // Step 1: simulate establishing a connection to a data source or file
            Console.WriteLine("Establishing default connection...");
            // Step 2: simulate reading lines from a JSON file
            Console.WriteLine("Reading JSON file content");
            IEnumerable<string> jsonLines = ["{\"Name\":\"Jessica\",\"Age\":40}", "{\"Name\":\"Martin\",\"Age\":50}"];
            // Step 3: simulate transforming JSON data into a list of Person objects
            Console.WriteLine("Transforming JSON data");
            var people = new List<Person>();
            foreach (var json in jsonLines)
            {
                var person = System.Text.Json.JsonSerializer.Deserialize<Person>(json);
                people.Add(person);
            }
            // Step 4: simulate loading data into the system
            Console.WriteLine("Loading data into the system...");
            foreach (var person in people) {
                Console.WriteLine($"Loading {person.Name}, Age: {person.Age}");
            }
            // Step 5: simulate closing the connection
            Console.WriteLine("Closing default connection...");
        }
    }
}