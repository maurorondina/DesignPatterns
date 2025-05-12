public static class FacadeGoodExample
{
    public static async Task RunAsync()
    {
        await using HomeTheaterFacade theater = new(new BluRayPlayer(), new SurroundSoundSystem(), new LaserProjector());
        await theater.InitializeAsync();
        await theater.PlayAsync("Jurassic Park");
        await Task.Delay(1000);
        await theater.StopAsync();
    }

    #region SUBSYSTEMS
    // SUBSYSTEM INTERFACES
    public interface IDvdPlayer
    {
        Task TurnOnAsync();
        Task PlayAsync(string movie);
        Task StopAsync();
        Task TurnOffAsync();
    }

    public interface IAudioSystem
    {
        Task PowerOnAsync();
        Task SetVolumeAsync(int level);
        Task PowerOffAsync();
    }

    public interface IProjector
    {
        Task ActivateAsync();
        Task SwitchInputAsync(string input);
        Task DeactivateAsync();
    }

    // SUBSYSTEM IMPLEMENTATIONS
    public class BluRayPlayer : IDvdPlayer
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

        public async Task StopAsync()
        {
            Console.WriteLine($"[BluRay] Stopping movie");
            await Task.Delay(100);
        }
        public async Task TurnOffAsync()
        {
            Console.WriteLine("[BluRay] Shutting down...");
            await Task.Delay(800);
        }
    }

    public class SurroundSoundSystem : IAudioSystem
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

        public async Task PowerOffAsync()
        {
            Console.WriteLine("[Audio] Powering off...");
            await Task.Delay(1500);
        }
    }

    public class LaserProjector : IProjector
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

        public async Task DeactivateAsync()
        {
            Console.WriteLine("[Projector] Cooling down ...");
            await Task.Delay(2000);
        }
    }
    #endregion SUBSYSTEMS

    // FACADE
    public sealed class HomeTheaterFacade(IDvdPlayer player, IAudioSystem audio, IProjector projector) : IAsyncDisposable
    {
        public async Task InitializeAsync()
        {
            Console.WriteLine("--- Initializing Home Theater System...");
            // Turn on all components
            await Task.WhenAll(
                player.TurnOnAsync(),
                audio.PowerOnAsync(),
                projector.ActivateAsync()
            );
            // Set up the projector and audio system
            await projector.SwitchInputAsync("HDMI ARC");
            await audio.SetVolumeAsync(35);
        }

        public async Task PlayAsync(string movie)
        {
            Console.WriteLine("--- Enjoy the movie!");
            await player.PlayAsync(movie);
        }

        public async Task StopAsync()
        {
            await player.StopAsync();
            Console.WriteLine("--- Movie stopped.");
        }

        public async ValueTask DisposeAsync()
        {
            await Task.WhenAll(
                player.TurnOffAsync(),
                audio.PowerOffAsync(),
                projector.DeactivateAsync()
            );
            Console.WriteLine("--- Theater system OFF");
        }
    }
}