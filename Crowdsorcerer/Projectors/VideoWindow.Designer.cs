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
            this.blackoutPanel = new System.Windows.Forms.Panel();
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
            // blackoutPanel
            // 
            this.blackoutPanel.BackColor = System.Drawing.Color.Black;
            this.blackoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.blackoutPanel.Location = new System.Drawing.Point(0, 0);
            this.blackoutPanel.Name = "blackoutPanel";
            this.blackoutPanel.Size = new System.Drawing.Size(284, 261);
            this.blackoutPanel.TabIndex = 2;
            // 
            // VideoWindow
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.audioView);
            this.Controls.Add(this.videoView);
            this.Controls.Add(this.blackoutPanel);
            this.Name = "VideoWindow";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.VideoWindow_FormClosed);
            this.DoubleClick += new System.EventHandler(this.VideoWindow_DoubleClick);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.VideoWindow_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.videoView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.audioView)).EndInit();
            this.ResumeLayout(false);

        }

        private LibVLCSharp.WinForms.VideoView audioView;
        private System.ComponentModel.IContainer components;
        private Panel blackoutPanel;
    }
}
