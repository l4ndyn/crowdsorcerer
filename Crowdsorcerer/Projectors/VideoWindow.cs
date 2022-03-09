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

        bool isBusy;
        public bool IsPlaying => videoPlayer.IsPlaying || isBusy;

        bool isFullscreen;

        public VideoWindow()
        {
            if (!DesignMode)
            {
                Core.Initialize();
            }

            InitializeComponent();
            KeyPreview = true;

            libVLC = new LibVLC();
            videoPlayer = new MediaPlayer(libVLC);

            videoView.MediaPlayer = videoPlayer;
            CheckForIllegalCrossThreadCalls = false;

            videoPlayer.Opening += (_, _) =>
            {
                videoView.Visible = true;
            };
            videoPlayer.EndReached += (_, _) =>
            {
                isBusy = false;
                videoView.Visible = false;
                VideoFinished?.Invoke();
            };
            videoPlayer.EncounteredError += (sender, e) =>
            {
                
            };
        }

        public void Play(string videoPath) => PlayOn(videoPlayer, new(libVLC, videoPath));
        public void Play(Uri videoUri) => PlayOn(videoPlayer, new(libVLC, videoUri));
        public void Play(Uri videoUri, Uri audioUri) => PlayOn(videoPlayer, new(libVLC, videoUri), audioUri.ToString());

        void PlayOn(MediaPlayer player, Media media)
        {
            isBusy = true;
            player.Stop();

            player.Media = media;
            player.Play();

            media.Dispose();
        }
        void PlayOn(MediaPlayer player, Media video, string audioUri)
        {
            isBusy = true;
            player.Stop();

            player.Media = video;
            player.AddSlave(MediaSlaveType.Audio, audioUri, false);

            player.Play();

            video.Dispose();
        }

        private void VideoWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            isBusy = false;

            videoPlayer.Stop();
            videoPlayer.Dispose();
            libVLC.Dispose();
        }

        private void VideoWindow_DoubleClick(object sender, EventArgs e)
        {
            GoFullscreen();
        }


        void GoFullscreen()
        {
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;

            isFullscreen = true;
        }
        void LeaveFullscreen()
        {

            FormBorderStyle = FormBorderStyle.FixedSingle;
            WindowState = FormWindowState.Normal;

            isFullscreen = false;
        }

        private void VideoWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.F11)
            {
                if (!isFullscreen) GoFullscreen();
                else LeaveFullscreen();
            }
        }
    }
}