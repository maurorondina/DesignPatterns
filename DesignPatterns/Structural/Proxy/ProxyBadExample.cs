public static class ProxyBadExample
{
    public static void Run()
    {
        var videoList = new VideoList();
        string[] videoIds = { "1234", "abcde", "javasc123" };

        // Adding videos triggers immediate downloads
        foreach (var id in videoIds)
        {
            videoList.Add(new YouTubeVideo(id)); // Downloads ALL videos upfront
        }

        // User watches only one video, but all were already downloaded
        videoList.Watch("abcde");
    }

    // Video Interface
    public interface Video
    {
        void Render();
        string GetVideoId();
    }

    // Third-Party YouTubeVideo Class (Cannot be modified)
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

    // Video List Manager
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