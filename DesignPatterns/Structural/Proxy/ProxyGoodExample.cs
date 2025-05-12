public static class ProxyGoodExample
{
    public static void Run()
    {
        var videoList = new VideoList();
        string[] videoIds = { "1234", "abcde", "javasc123" };

        // Add proxy objects instead of real videos
        foreach (var id in videoIds)
        {
            videoList.Add(new YouTubeVideoProxy(id)); // No downloads yet!
        }

        // Only "abcde" is downloaded when needed
        videoList.Watch("abcde");
    }

    // PROXY
    public class YouTubeVideoProxy : Video
    {
        private string _videoId;
        private YouTubeVideo _realVideo; // Deferred until needed

        public YouTubeVideoProxy(string videoId)
        {
            _videoId = videoId; // No download here!
        }

        public void Render()
        {
            if (_realVideo == null)
            {
                _realVideo = new YouTubeVideo(_videoId); // Download on-demand
            }
            _realVideo.Render();
        }

        public string GetVideoId() => _videoId;
    }

    // SERVICE INTERFACE - Video Interface
    public interface Video
    {
        void Render();
        string GetVideoId();
    }

    // SERVICE - Third-Party YouTubeVideo Class (Cannot be modified)
    public class YouTubeVideo : Video
    {
        private readonly string _videoId;

        public YouTubeVideo(string videoId)
        {
            _videoId = videoId;
            Download(); // Problem: Downloads immediately on creation
        }

        private void Download() =>
            Console.WriteLine($"Downloading video {_videoId} from YouTube API");

        public void Render() =>
            Console.WriteLine($"Rendering video {_videoId}");

        public string GetVideoId() => _videoId;
    }

    // CLIENT Video List Manager
    public class VideoList
    {
        private Dictionary<string, Video> _videos = new Dictionary<string, Video>();

        public void Add(Video video) =>
            _videos.Add(video.GetVideoId(), video);

        public void Watch(string videoId)
        {
            var video = _videos[videoId];
            video.Render(); // Just renders; video was already downloaded
        }
    }
}