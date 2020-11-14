using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DAudio;
using TagLib;

namespace music_player
{
    

    public partial class Form : System.Windows.Forms.Form
    {
        #region Drag and DropShadow form

        
        private bool Drag;
        private int MouseX;
        private int MouseY;

        private const int WM_NCHITTEST = 0x84;
        private const int HTCLIENT = 0x1;
        private const int HTCAPTION = 0x2;

        private bool m_aeroEnabled;

        private const int CS_DROPSHADOW = 0x00020000;
        private const int WM_NCPAINT = 0x0085;
        private const int WM_ACTIVATEAPP = 0x001C;

        [System.Runtime.InteropServices.DllImport("dwmapi.dll")]
        public static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMarInset);
        [System.Runtime.InteropServices.DllImport("dwmapi.dll")]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);
        [System.Runtime.InteropServices.DllImport("dwmapi.dll")]

        public static extern int DwmIsCompositionEnabled(ref int pfEnabled);
        [System.Runtime.InteropServices.DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottomRect,
            int nWidthEllipse,
            int nHeightEllipse
            );

        public struct MARGINS
        {
            public int leftWidth;
            public int rightWidth;
            public int topHeight;
            public int bottomHeight;
        }
        protected override CreateParams CreateParams
        {
            get
            {
                m_aeroEnabled = CheckAeroEnabled();
                CreateParams cp = base.CreateParams;
                if (!m_aeroEnabled)
                    cp.ClassStyle |= CS_DROPSHADOW; return cp;
            }
        }
        private bool CheckAeroEnabled()
        {
            if (Environment.OSVersion.Version.Major >= 6)
            {
                int enabled = 0; DwmIsCompositionEnabled(ref enabled);
                return (enabled == 1) ? true : false;
            }
            return false;
        }
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_NCPAINT:
                    if (m_aeroEnabled)
                    {
                        var v = 2;
                        DwmSetWindowAttribute(this.Handle, 2, ref v, 4);
                        MARGINS margins = new MARGINS()
                        {
                            bottomHeight = 1,
                            leftWidth = 0,
                            rightWidth = 0,
                            topHeight = 0
                        }; DwmExtendFrameIntoClientArea(this.Handle, ref margins);
                    }
                    break;
                default: break;
            }
            base.WndProc(ref m);
            if (m.Msg == WM_NCHITTEST && (int)m.Result == HTCLIENT) m.Result = (IntPtr)HTCAPTION;
        }
        private void PanelMove_MouseDown(object sender, MouseEventArgs e)
        {
            Drag = true;
            MouseX = Cursor.Position.X - this.Left;
            MouseY = Cursor.Position.Y - this.Top;
        }
        private void PanelMove_MouseMove(object sender, MouseEventArgs e)
        {
            if (Drag)
            {
                this.Top = Cursor.Position.Y - MouseY;
                this.Left = Cursor.Position.X - MouseX;
            }
        }
        private void PanelMove_MouseUp(object sender, MouseEventArgs e) { Drag = false; }

        #endregion Drag and DropShadow form

        //private const int CS_DROPSHADOW = 0x00020000;
        

        private AudioPlayer Player;
        public Form()
        {
            InitializeComponent();
            //panel1.Paint += dropShadow;
            //SetStyle(ControlStyles.DoubleBuffer, true); UpdateStyles();
            Player = new AudioPlayer();
            // подписываемся на событие изменения статуса
            Player.PlayingStatusChanged += (s, e) => button_Play.Text = e == Status.Playing ? "Pause" : "Play";

            Player.AudioSelected += (s, e) =>
            {
                //приравниваем максимум трекбара к продолжительности трека
                trackBar_CurentPosition.Maximum = (int)e.Duration;
                //выводим название трека в верхний лэбел 
                label_TrackName.Text = e.fileTag.Tag.Title;
                //выводим длину трека в правый лэбел
                label_TrakDuration.Text = e.DurationTime.ToString(@"mm\:ss");
                listBox_Playlist.SelectedItem = e.Name;
            };

            Player.ProgressChanged += (s, e) =>
            {
                trackBar_CurentPosition.Value = (int)e;
                label_CurentDuration.Text = ((AudioPlayer)s).PositionTime.ToString(@"mm\:ss");
            };
        }

        

        private void dropShadow(object sender, PaintEventArgs e)
        {
            Panel panel = (Panel)sender;
            Color[] shadow = new Color[3];
            shadow[0] = Color.FromArgb(181, 181, 181);
            shadow[1] = Color.FromArgb(195, 195, 195);
            shadow[2] = Color.FromArgb(211, 211, 211);
            Pen pen = new Pen(shadow[0]);
            using (pen)
            {
                foreach (Panel p in panel.Controls.OfType<Panel>())
                {
                    Point pt = p.Location;
                    pt.Y += p.Height;
                    for (var sp = 0; sp < 3; sp++)
                    {
                        pen.Color = shadow[sp];
                        e.Graphics.DrawLine(pen, p.Right + sp, p.Top + sp, p.Right + sp, p.Bottom + sp);
                        pt.Y++;
                    }
                }
            }
        }

        private void button_Open_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog() { Multiselect = true, Filter = "Audio files|*.wav;*.aac;*.mp4;*.mp3;" })
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    Player.LoadAudio(dialog.FileNames);
                    listBox_Playlist.Items.Clear();
                    listBox_Playlist.Items.AddRange(Player.Playlist);
                }
            } 
        }

        private void listBox_Playlist_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (((ListBox)sender).SelectedItem == null) return;

            Player.SelectAudio(((ListBox)sender).SelectedIndex);

            string new_str = Player.CurrentAudio.SourceUrl;
            var tfile = TagLib.File.Create(Player.CurrentAudio.SourceUrl);
            label_Album.Text = tfile.Tag.Album;
            var bin = (byte[])(tfile.Tag.Pictures[0].Data.Data);
            pictureBox_Cover.Image = Image.FromStream(new MemoryStream(bin));
            //pictureBox_Cover.Image.;
        }

        private void button_Play_Click(object sender, EventArgs e)
        {
            if (((Button)sender).Text == "Play") Player.Play();

            else if (((Button)sender).Text == "Pause") Player.Pause();
        }

        private void trackBar_CurentPosition_Scroll(object sender, EventArgs e) => Player.Position = ((TrackBar)sender).Value;

        private void trackBar_VolumeLavel_Scroll(object sender, EventArgs e) => Player.Volume = ((TrackBar)sender).Value;

        private void button_ClearPlaylist_Click(object sender, EventArgs e)
        {
            Player.Stop();
            listBox_Playlist.Items.Clear();
            Player.ClearPlaylist();
        }

        private void button_Next_Click(object sender, EventArgs e)
        {
            if(listBox_Playlist.SelectedIndex < listBox_Playlist.Items.Count)
            {
                Player.SelectAudio(listBox_Playlist.SelectedIndex + 1);
            }
            else
            {
                Player.SelectAudio(0);
            }
        }

        private void button_Prev_Click(object sender, EventArgs e)
        {
            if (listBox_Playlist.SelectedIndex < listBox_Playlist.Items.Count)
            {
                Player.SelectAudio(listBox_Playlist.SelectedIndex - 1);
            }
            else
            {
                Player.SelectAudio(0);
            }
        }

       

       

        private void customButton1_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
