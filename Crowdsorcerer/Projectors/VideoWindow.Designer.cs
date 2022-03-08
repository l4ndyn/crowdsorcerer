using System.Windows.Forms;

namespace Crowdsorcerer.Projectors
{
    public partial class VideoWindow : Form
    {
        private LibVLCSharp.WinForms.VideoView videoView;

        private void InitializeComponent()
        {
            this.videoView = new LibVLCSharp.WinForms.VideoView();
            this.audioView = new LibVLCSharp.WinForms.VideoView();
            ((System.ComponentModel.ISupportInitialize)(this.videoView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.audioView)).BeginInit();
            this.SuspendLayout();
            // 
            // videoView
            // 
            this.videoView.BackColor = System.Drawing.Color.Black;
            this.videoView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.videoView.Location = new System.Drawing.Point(0, 0);
            this.videoView.MediaPlayer = null;
            this.videoView.Name = "videoView";
            this.videoView.Size = new System.Drawing.Size(284, 261);
            this.videoView.TabIndex = 0;
            this.videoView.Text = "videoView1";
            // 
            // audioView
            // 
            this.audioView.BackColor = System.Drawing.Color.Black;
            this.audioView.Location = new System.Drawing.Point(0, 0);
            this.audioView.MediaPlayer = null;
            this.audioView.Name = "audioView";
            this.audioView.Size = new System.Drawing.Size(75, 23);
            this.audioView.TabIndex = 1;
            this.audioView.Text = "videoView1";
            this.audioView.Visible = false;
            // 
            // VideoWindow
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.audioView);
            this.Controls.Add(this.videoView);
            this.Name = "VideoWindow";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.VideoWindow_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.videoView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.audioView)).EndInit();
            this.ResumeLayout(false);

        }

        private LibVLCSharp.WinForms.VideoView audioView;
        private System.ComponentModel.IContainer components;
    }
}
