/*using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using YoutubeExtractor;

namespace Crowdsorcerer.Youtube
{
    public class YoutubeProviderExtractor : YoutubeProvider
    {
        public override Task<string> Get(string url)
        {
            IEnumerable<VideoInfo> videoInfos = DownloadUrlResolver.GetDownloadUrls(url);
            var videoPath = GetBestVideo(videoInfos);
            var audioPath = GetBestAudio(videoInfos);

            VideoMerger.Merge(audioPath, videoPath, "video_sharpgrabber.mp4");

            return Task.FromResult("video_extractor.mp4");
        }

        string GetBestVideo(IEnumerable<VideoInfo> videoInfos)
        {
            VideoInfo video = videoInfos.First(info => info.VideoType == VideoType.Mp4 && info.Resolution == 144);

            if (video.RequiresDecryption) DownloadUrlResolver.DecryptDownloadUrl(video);

            var videoDownloader = new VideoDownloader(video, "v_extractor.mp4");
            videoDownloader.Execute();

            return "v_extractor.mp4";
        }

        string GetBestAudio(IEnumerable<VideoInfo> videoInfos)
        {
            VideoInfo video = videoInfos
                .Where(info => info.CanExtractAudio)
                .OrderByDescending(info => info.AudioBitrate)
                .First();

            if (video.RequiresDecryption) DownloadUrlResolver.DecryptDownloadUrl(video);

            var audioDownloader = new AudioDownloader(video, "a_extractor.mp4");
            audioDownloader.Execute();

            return "a_extractor.mp4";
        }
    }
}*/