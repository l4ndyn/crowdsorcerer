using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DotNetTools.SharpGrabber;
using DotNetTools.SharpGrabber.Converter;
using DotNetTools.SharpGrabber.Grabbed;

namespace Crowdsorcerer.Youtube
{
    public class YoutubeProviderSharpGrabber : YoutubeProvider
    {
        private readonly HttpClient client;

        IMultiGrabber grabber;

        public YoutubeProviderSharpGrabber()
        {
            client = new();
            client.Timeout = TimeSpan.FromMinutes(10);

            grabber = GrabberBuilder.New()
                .UseDefaultServices()
                .AddYouTube()
                .Build();
		}

		public override async Task<string> Get(string url)
        {
            var result = await grabber.GrabAsync(new Uri(url, UriKind.Absolute));

			var audioStream = GetBestAudio(result);
            if (audioStream == null)
                throw new InvalidOperationException("No audio stream detected.");

            var videoStream = GetBestVideo(result);
            if (videoStream == null)
				throw new InvalidOperationException("No video stream detected.");

			try
            {
                var audioPath = await DownloadMedia(audioStream, result);
				var videoPath = await DownloadMedia(videoStream, result);

                var outputPath = Unique.FileName("mp4");
				await VideoMerger.Merge(audioPath, videoPath, outputPath);

                return outputPath;
            }
			finally
            {
                TempFiles.Clear();
            }
		}

        public override Task<YoutubeVideoInfo> GetUris(string url)
        {
            throw new NotImplementedException();
        }

        GrabbedMedia GetBestAudio(GrabResult result)
        {
            return result.Resources<GrabbedMedia>().FirstOrDefault(m => m.Channels == MediaChannels.Audio);
        }

        GrabbedMedia GetBestVideo(GrabResult result)
        {
            var videos = result.Resources<GrabbedMedia>()
                .Where(m => m.Channels == MediaChannels.Video).ToList();
            videos.Sort((x, y) => GetSize(y.Resolution).CompareTo(GetSize(x.Resolution)));

            return videos.FirstOrDefault(v => GetSize(v.Resolution) <= 144/* && v.Format.Extension == "mp4"*/);

            int GetSize(string size) => int.Parse(size.Substring(0, size.Length - 1));
        }

        async Task<string> DownloadMedia(GrabbedMedia media, IGrabResult grabResult)
		{
			Console.WriteLine("Downloading {0}...", media.Title ?? media.FormatTitle ?? media.Resolution);

			using var response = await client.GetAsync(media.ResourceUri);
			response.EnsureSuccessStatusCode();

            using var downloadStream = await response.Content.ReadAsStreamAsync();
            using var resourceStream = await grabResult.WrapStreamAsync(downloadStream);

			var path = TempFiles.New();
            using var fileStream = new FileStream(path, FileMode.Create);

            await resourceStream.CopyToAsync(fileStream);

			return path;
		}
    }
}