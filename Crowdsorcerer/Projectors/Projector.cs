using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Crowdsorcerer.Sorcerer;
using Crowdsorcerer.Youtube;
using YouTubeSearch;
using Action = System.Action;
using Log = Serilog.Log;

namespace Crowdsorcerer.Projectors
{
    public class Projector
    {
        VideoWindow videoWindow;
        YoutubeProvider youtubeProvider;

        public event Action VideoFinished;
        public bool IsVideoPlaying => videoWindow.IsPlaying;

        public Projector()
        {
            youtubeProvider = new YoutubeProviderLibvideoFast();

            Task.Run(() =>
            {
                videoWindow = new();
                videoWindow.VideoFinished += VideoFinished;

                Application.Run(videoWindow);
            });
        }

        public async Task ProjectYoutube(Url url)
        {
            var videoInfo = await youtubeProvider.GetUris(url.url);
            Log.Debug($"[Projector] Projecting video with info: {videoInfo}");

            if (videoInfo.length != -1 && videoInfo.length < 60)
                ProjectMuxed(videoInfo.videoUri, videoInfo.audioUri);
            else
                ProjectSynced(videoInfo.videoUri, videoInfo.audioUri);

            //TODO: is this unstable?
            void ProjectMuxed(Uri videoUri, Uri audioUri)
            {
                var outputStreamUri = VideoMerger.GetUniqueOutputUri();
                VideoMerger.Merge(audioUri, videoUri, outputStreamUri);

                videoWindow.Play(outputStreamUri);
            }

            void ProjectSynced(Uri videoUri, Uri audioUri)
            {
                videoWindow.Play(videoUri, audioUri);
            }
        }
        public async Task ProjectYoutube(Text title)
        {
            VideoSearch search = new();
            var videos = await search.GetVideos(title.text, 1);

            Log.Debug($"[Projector] Fetching video url from title: {title.text}");

            await ProjectYoutube(new Url { url = videos.First().getUrl() });
        }

        public async Task ProjectYoutube(Sorcerer.Message source)
        {
            if (source is Url url) await ProjectYoutube(url);
            if (source is Text title) await ProjectYoutube(title);
            else throw new ArgumentException();
        }
    }
}