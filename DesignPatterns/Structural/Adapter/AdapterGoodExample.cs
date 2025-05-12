public static class AdapterGoodExample
{
    public static void Run()
    {
        // 4. Usage
        var video = new Video();  
        var editor = new VideoEditor(video);  

        // Existing color (no changes needed)  
        editor.ApplyColor(new BlackAndWhiteColor());

        // New 3rd-party color via adapter  
        var rainbowAdapter = new RainbowColor(new Rainbow());  
        editor.ApplyColor(rainbowAdapter);

        // Outputs:  
        // Applying black and white filter.  
        // Initializing rainbow filter settings...  
        // Applying rainbow filter to video.  
    }

    // 1. Existing Code (Unchanged)  
    public class Video { /* Video properties/methods */ }

    public interface Color                       // CLIENT INTERFACE
    {
        void Apply(Video video);
    }

    public class BlackAndWhiteColor : Color
    {
        public void Apply(Video video) =>
            Console.WriteLine("Applying black and white filter.");
    }

    public class VideoEditor(Video video)        // CLIENT
    {
        public void ApplyColor(Color color) => color.Apply(video);
    }

    // 2. 3rd-Party Library Code (Unchanged)  
    public class Rainbow                         // SERVICE
    {
        public void Setup() =>
            Console.WriteLine("Initializing rainbow filter settings...");

        public void Update(Video video) =>
            Console.WriteLine("Applying rainbow filter to video.");
    }

    // 3. Adapter Class (New Code)  
    public class RainbowColor : Color            // ADAPTER
    {
        private readonly Rainbow _rainbow;

        public RainbowColor(Rainbow rainbow) => _rainbow = rainbow;

        public void Apply(Video video)
        {
            _rainbow.Setup();        // Call 3rd-party setup logic  
            _rainbow.Update(video);  // Call 3rd-party apply logic  
        }
    }
}