using System;
using System.Windows.Forms;
using LibVLCSharp.Shared;

namespace Crowdsorcerer.Projectors
{
    public partial class VideoWindow : Form
    {
        LibVLC libVLC;
        MediaPlayer videoPlayer;

        public event Action VideoFinished;

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

            videoPlayer.EndReached += (_, _) => VideoFinished?.Invoke();
        }

        public void Play(string videoPath) => PlayOn(videoPlayer, new(libVLC, videoPath));
        public void Play(Uri videoUri) => PlayOn(videoPlayer, new(libVLC, videoUri));
        public void Play(Uri videoUri, Uri audioUri) => PlayOn(videoPlayer, new(libVLC, videoUri), audioUri.ToString());

        void PlayOn(MediaPlayer player, Media media)
        {
            player.Stop();

            player.Media = media;
            player.Play();

            media.Dispose();
        }
        void PlayOn(MediaPlayer player, Media video, string audioUri)
        {
            player.Stop();

            player.Media = video;
            player.AddSlave(MediaSlaveType.Audio, audioUri, false);

            player.Play();

            video.Dispose();
        }

        private void VideoWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            videoPlayer.Stop();
            videoPlayer.Dispose();
            libVLC.Dispose();
        }
    }
}