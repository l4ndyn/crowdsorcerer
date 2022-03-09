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
    }
}