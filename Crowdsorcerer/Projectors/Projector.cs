using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Crowdsorcerer.Youtube;
using YouTubeSearch;
using Action = System.Action;

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

        public async Task ProjectYoutube(YoutubeUrl url)
        {
            var videoInfo = await youtubeProvider.GetUris(url.url);
            Console.WriteLine(videoInfo);

            if (videoInfo.length != -1 && videoInfo.length < 60)
                ProjectMuxed(videoInfo.videoUri, videoInfo.audioUri);
            else
                ProjectSynced(videoInfo.videoUri, videoInfo.audioUri);

            //}

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
        public async Task ProjectYoutube(YoutubeTitle title)
        {
            VideoSearch search = new();
            var videos = await search.GetVideos(title.title, 1);

            await ProjectYoutube(new YoutubeUrl { url = videos.First().getUrl() });
        }

        public async Task ProjectYoutube(YoutubeVideoSource source)
        {
            if (source is YoutubeUrl url) await ProjectYoutube(url);
            if (source is YoutubeTitle title) await ProjectYoutube(title);
            else throw new ArgumentException();
        }
    }
}