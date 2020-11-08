using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DAudio;

namespace music_player
{
    

    public partial class Form : System.Windows.Forms.Form
    {

        private AudioPlayer Player;
        public Form()
        {
            InitializeComponent();
            Player = new AudioPlayer();
            // подписываемся на событие изменения статуса
            Player.PlayingStatusChanged += (s, e) => button_Play.Text = e == Status.Playing ? "Pause" : "Play";

            Player.AudioSelected += (s, e) =>
            {
                //приравниваем максимум трекбара к продолжительности трека
                trackBar_CurentPosition.Maximum = (int)e.Duration;
                //выводим название трека в верхний лэбел 
                label_TrackName.Text = e.Name;
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
    }
}
