using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
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

                var outputPath = Unique.FileName("mp4");
                await VideoMerger.Merge(videoPath, audioPath, outputPath);

                return outputPath;
            }
            finally
            {
                TempFiles.Clear();
            }
        }

        public override async Task<YoutubeVideoInfo> GetUris(string url)
        {
            var (audio, video) = await GetBestMedia(url);
            return new YoutubeVideoInfo(new(video.Uri), new(audio.Uri), video.Title, video.Info.LengthSeconds ?? -1);
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

            return videos.FirstOrDefault(v => v.Resolution <= 480);
        }
        YouTubeVideo GetBestAudio(IEnumerable<YouTubeVideo> resources)
        {
            return resources.FirstOrDefault(r => r.AdaptiveKind == AdaptiveKind.Audio && r.AudioFormat != AudioFormat.Unknown);
        }

        async Task<string> DownloadVideo(YouTubeVideo video)
        {
            Log.Information($"[YoutubeProvider] Downloading {video.Title} [{video.Format}]...");

            var path = TempFiles.New();
            await SaveVideo(video, path);

            Log.Information($"[YoutubeProvider] Successfully ownloaded {video.Title} [{video.Format}].");

            return path;
        }

        protected virtual async Task SaveVideo(YouTubeVideo video, string path) =>
            File.WriteAllBytes(path, await video.GetBytesAsync());
    }
}