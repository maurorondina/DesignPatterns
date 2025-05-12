public static class AdapterBadExample
{
    public static void Run()
    {
        // 3. Usage (Fails Compilation)  
        var video = new Video();
        var editor = new VideoEditor(video);

        editor.ApplyColor(new BlackAndWhiteColor()); // ✅ Works  
        // editor.ApplyColor(new Rainbow());         // ❌ Compiler Error:  
                                                     // "Cannot convert 'Rainbow' to 'Color'."  
    }

    // 1. Existing Code (Your Application)  
    public class Video { /* Video properties/methods */ }

    public interface Color
    {
        void Apply(Video video);
    }

    public class BlackAndWhiteColor : Color
    {
        public void Apply(Video video) =>
            Console.WriteLine("Applying black and white filter.");
    }

    public class VideoEditor(Video video)
    {
        public void ApplyColor(Color color) => color.Apply(video);
    }

    // 2. 3rd-Party Library Code (Incompatible)  
    public class Rainbow
    {
        public void Setup() =>
            Console.WriteLine("Initializing rainbow filter settings...");

        public void Update(Video video) =>
            Console.WriteLine("Applying rainbow filter to video.");
    }
}