public static class FacadeBadExample
{
    public static async Task RunAsync()
    {
        var player = new BluRayPlayer();
        var audio = new SurroundSoundSystem();
        var projector = new LaserProjector();

        // Complex startup sequence
        await player.TurnOnAsync();
        await audio.PowerOnAsync();
        await projector.ActivateAsync();
        await projector.SwitchInputAsync("HDMI");
        await audio.SetVolumeAsync(35);
        await player.PlayAsync("Jurassic Park");

        await Task.Delay(2000); // Watch movie

        // Incomplete shutdown!
        await player.TurnOffAsync();
        // Forgot to power off audio!
        // Forgot to deactivate projector!
    }

    #region SUBSYSTEMS
    public class BluRayPlayer
    {
        public async Task TurnOnAsync()
        {
            Console.WriteLine("[BluRay] Booting...");
            await Task.Delay(1200);
        }

        public async Task PlayAsync(string movie)
        {
            Console.WriteLine($"[BluRay] Playing '{movie}'");
            await Task.Delay(300);
        }

        public async Task TurnOffAsync()
        {
            Console.WriteLine("[BluRay] Shutting down...");
            await Task.Delay(800);
        }
    }

    public class SurroundSoundSystem
    {
        public async Task PowerOnAsync()
        {
            Console.WriteLine("[Audio] Initializing...");
            await Task.Delay(1500);
        }

        public async Task SetVolumeAsync(int level)
        {
            Console.WriteLine($"[Audio] Volume {level}%");
            await Task.Delay(100);
        }
    }

    public class LaserProjector
    {
        public async Task ActivateAsync()
        {
            Console.WriteLine("[Projector] Warming up...");
            await Task.Delay(2000);
        }

        public async Task SwitchInputAsync(string input)
        {
            Console.WriteLine($"[Projector] Input: {input}");
            await Task.Delay(300);
        }
    }
    #endregion SUBSYSTEMS
}