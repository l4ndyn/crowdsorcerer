using System;
using System.Windows.Forms;
using LibVLCSharp.Shared;

namespace Crowdsorcerer.Projector
{
    public partial class VideoWindow : Form
    {
        LibVLC libVLC;
        MediaPlayer videoPlayer;
        //MediaTimelineController

        public VideoWindow()
        {
            if (!DesignMode)
            {
                Core.Initialize();
            }

            InitializeComponent();

            libVLC = new LibVLC();
            videoPlayer = new MediaPlayer(libVLC);

            videoView.MediaPlayer = videoPlayer;
        }

        public void Play(string videoPath) => PlayOn(videoPlayer, new(libVLC, videoPath));
        public void Play(Uri videoUri) => PlayOn(videoPlayer, new(libVLC, videoUri));

        void PlayOn(MediaPlayer player, Media media)
        {
            player.Play(media);
            media.Dispose();
        }

        private void VideoWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            videoPlayer.Stop();
            videoPlayer.Dispose();
            libVLC.Dispose();
        }
    }
}