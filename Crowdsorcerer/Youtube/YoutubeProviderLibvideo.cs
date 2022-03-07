using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VideoLibrary;

namespace Crowdsorcerer.Youtube
{
    public class YoutubeProviderLibvideo : YoutubeProvider
    {
        public override async Task<string> Get(string url)
        {
            var (audio, video) = await GetBestMedia(url);

            try
            {
                var audioPath = await DownloadVideo(audio);
                var videoPath = await DownloadVideo(video);
                //var audioPath = @"C:\Users\alpar\AppData\Local\Temp\tmpF2C8.mp3";
                //var videoPath = @"C:\Users\alpar\AppData\Local\Temp\tmp9EF7.mp4";

                var outputPath = UniqueFileNames.New("mp4");
                await VideoMerger.Merge(videoPath, audioPath, outputPath);

                return outputPath;
            }
            finally
            {
                TempFiles.Clear();
            }
        }

        public async Task<(Uri audioUri, Uri videoUri)> GetUris(string url)
        {
            var (audio, video) = await GetBestMedia(url);
            return (new(audio.Uri), new(video.Uri));
        }

        async Task<(YouTubeVideo audio, YouTubeVideo video)> GetBestMedia(string url)
        {
            var videoInfos = await GetAllVideos(url);

            var audio = GetBestAudio(videoInfos);
            if (audio == null)
                throw new InvalidOperationException("No audio stream detected.");

            var video = GetBestVideo(videoInfos);
            if (video == null)
                throw new InvalidOperationException("No video stream detected.");

            return (audio, video);
        }

        protected virtual async Task<IEnumerable<YouTubeVideo>> GetAllVideos(string url) =>
            await Client.For(YouTube.Default).GetAllVideosAsync(url);

        YouTubeVideo GetBestVideo(IEnumerable<YouTubeVideo> resources)
        {
            var videos = resources.Where(r => r.AdaptiveKind == AdaptiveKind.Video && r.AudioFormat == AudioFormat.Unknown).ToList();
            videos.Sort((x, y) => y.Resolution.CompareTo(x.Resolution));

            return videos.FirstOrDefault(v => v.Resolution <= 1080);
        }
        YouTubeVideo GetBestAudio(IEnumerable<YouTubeVideo> resources)
        {
            return resources.FirstOrDefault(r => r.AdaptiveKind == AdaptiveKind.Audio && r.AudioFormat != AudioFormat.Unknown);
        }

        async Task<string> DownloadVideo(YouTubeVideo video)
        {
            Console.WriteLine($"Downloading {video.Title} [{video.Format}]...");

            var path = TempFiles.New();
            await SaveVideo(video, path);

            Console.WriteLine("Downloaded.");

            return path;
        }

        protected virtual async Task SaveVideo(YouTubeVideo video, string path) =>
            File.WriteAllBytes(path, await video.GetBytesAsync());
    }
}