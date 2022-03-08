using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Crowdsorcerer.Youtube;

namespace Crowdsorcerer.Projectors
{
    public class Projector
    {
        VideoWindow videoWindow;
        YoutubeProviderLibvideoFast youtubeProvider;

        public Projector()
        {
            youtubeProvider = new();

            Task.Run(() =>
            {
                videoWindow = new();

                //videoWindow.Shown += (_, _) => ProjectYoutube("https://www.youtube.com/watch?v=ZmxF0vxpXa0&ab_channel=JeaneyCollects");
                //videoWindow.VideoFinished += () =>
                //    ProjectYoutube("https://www.youtube.com/watch?v=Ql2zKS_bdfE&ab_channel=CaptainMayo");
                Application.Run(videoWindow);
            });
        }

        public void ProjectYoutube(string url)
        {
            Task.Run(async () =>
            {
                var (audioUri, videoUri) = await youtubeProvider.GetUris(url);

                //ProjectMuxed(videoUri, audioUri);
                ProjectSynced(videoUri, audioUri);

            });

            //doesnt neccessarily work
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