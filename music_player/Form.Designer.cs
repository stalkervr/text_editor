namespace music_player
{
    partial class Form
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.label_TrackName = new System.Windows.Forms.Label();
            this.button_Play = new System.Windows.Forms.Button();
            this.button_Open = new System.Windows.Forms.Button();
            this.listBox_Playlist = new System.Windows.Forms.ListBox();
            this.trackBar_CurentPosition = new System.Windows.Forms.TrackBar();
            this.trackBar_VolumeLavel = new System.Windows.Forms.TrackBar();
            this.label_CurentDuration = new System.Windows.Forms.Label();
            this.label_TrakDuration = new System.Windows.Forms.Label();
            this.button_ClearPlaylist = new System.Windows.Forms.Button();
            this.button_Next = new System.Windows.Forms.Button();
            this.button_Prev = new System.Windows.Forms.Button();
            this.pictureBox_Cover = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label_Album = new System.Windows.Forms.Label();
            this.label_Year = new System.Windows.Forms.Label();
            this.label_Autor = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_CurentPosition)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_VolumeLavel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Cover)).BeginInit();
            this.SuspendLayout();
            // 
            // label_TrackName
            // 
            this.label_TrackName.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_TrackName.Location = new System.Drawing.Point(345, 13);
            this.label_TrackName.Name = "label_TrackName";
            this.label_TrackName.Size = new System.Drawing.Size(377, 38);
            this.label_TrackName.TabIndex = 0;
            this.label_TrackName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button_Play
            // 
            this.button_Play.Location = new System.Drawing.Point(336, 184);
            this.button_Play.Name = "button_Play";
            this.button_Play.Size = new System.Drawing.Size(194, 23);
            this.button_Play.TabIndex = 1;
            this.button_Play.Text = "Play";
            this.button_Play.UseVisualStyleBackColor = true;
            this.button_Play.Click += new System.EventHandler(this.button_Play_Click);
            // 
            // button_Open
            // 
            this.button_Open.Location = new System.Drawing.Point(531, 184);
            this.button_Open.Name = "button_Open";
            this.button_Open.Size = new System.Drawing.Size(194, 23);
            this.button_Open.TabIndex = 2;
            this.button_Open.Text = "Open";
            this.button_Open.UseVisualStyleBackColor = true;
            this.button_Open.Click += new System.EventHandler(this.button_Open_Click);
            // 
            // listBox_Playlist
            // 
            this.listBox_Playlist.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listBox_Playlist.FormattingEnabled = true;
            this.listBox_Playlist.Location = new System.Drawing.Point(742, 13);
            this.listBox_Playlist.Name = "listBox_Playlist";
            this.listBox_Playlist.Size = new System.Drawing.Size(230, 301);
            this.listBox_Playlist.TabIndex = 3;
            this.listBox_Playlist.SelectedIndexChanged += new System.EventHandler(this.listBox_Playlist_SelectedIndexChanged);
            // 
            // trackBar_CurentPosition
            // 
            this.trackBar_CurentPosition.AutoSize = false;
            this.trackBar_CurentPosition.Location = new System.Drawing.Point(60, 289);
            this.trackBar_CurentPosition.Name = "trackBar_CurentPosition";
            this.trackBar_CurentPosition.Size = new System.Drawing.Size(605, 27);
            this.trackBar_CurentPosition.TabIndex = 4;
            this.trackBar_CurentPosition.Scroll += new System.EventHandler(this.trackBar_CurentPosition_Scroll);
            // 
            // trackBar_VolumeLavel
            // 
            this.trackBar_VolumeLavel.AutoSize = false;
            this.trackBar_VolumeLavel.Location = new System.Drawing.Point(274, 12);
            this.trackBar_VolumeLavel.Maximum = 100;
            this.trackBar_VolumeLavel.Name = "trackBar_VolumeLavel";
            this.trackBar_VolumeLavel.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBar_VolumeLavel.Size = new System.Drawing.Size(32, 244);
            this.trackBar_VolumeLavel.TabIndex = 5;
            this.trackBar_VolumeLavel.Value = 50;
            this.trackBar_VolumeLavel.Scroll += new System.EventHandler(this.trackBar_VolumeLavel_Scroll);
            // 
            // label_CurentDuration
            // 
            this.label_CurentDuration.AutoSize = true;
            this.label_CurentDuration.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_CurentDuration.Location = new System.Drawing.Point(15, 295);
            this.label_CurentDuration.Name = "label_CurentDuration";
            this.label_CurentDuration.Size = new System.Drawing.Size(39, 13);
            this.label_CurentDuration.TabIndex = 6;
            this.label_CurentDuration.Text = "00.00";
            // 
            // label_TrakDuration
            // 
            this.label_TrakDuration.AutoSize = true;
            this.label_TrakDuration.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_TrakDuration.Location = new System.Drawing.Point(682, 295);
            this.label_TrakDuration.Name = "label_TrakDuration";
            this.label_TrakDuration.Size = new System.Drawing.Size(39, 13);
            this.label_TrakDuration.TabIndex = 7;
            this.label_TrakDuration.Text = "00.00";
            this.label_TrakDuration.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // button_ClearPlaylist
            // 
            this.button_ClearPlaylist.Location = new System.Drawing.Point(531, 233);
            this.button_ClearPlaylist.Name = "button_ClearPlaylist";
            this.button_ClearPlaylist.Size = new System.Drawing.Size(194, 23);
            this.button_ClearPlaylist.TabIndex = 8;
            this.button_ClearPlaylist.Text = "Clear Playlist";
            this.button_ClearPlaylist.UseVisualStyleBackColor = true;
            this.button_ClearPlaylist.Click += new System.EventHandler(this.button_ClearPlaylist_Click);
            // 
            // button_Next
            // 
            this.button_Next.Location = new System.Drawing.Point(531, 208);
            this.button_Next.Name = "button_Next";
            this.button_Next.Size = new System.Drawing.Size(194, 23);
            this.button_Next.TabIndex = 9;
            this.button_Next.Text = "Next";
            this.button_Next.UseVisualStyleBackColor = true;
            this.button_Next.Click += new System.EventHandler(this.button_Next_Click);
            // 
            // button_Prev
            // 
            this.button_Prev.Location = new System.Drawing.Point(336, 208);
            this.button_Prev.Name = "button_Prev";
            this.button_Prev.Size = new System.Drawing.Size(194, 23);
            this.button_Prev.TabIndex = 10;
            this.button_Prev.Text = "Prev";
            this.button_Prev.UseVisualStyleBackColor = true;
            this.button_Prev.Click += new System.EventHandler(this.button_Prev_Click);
            // 
            // pictureBox_Cover
            // 
            this.pictureBox_Cover.Location = new System.Drawing.Point(12, 12);
            this.pictureBox_Cover.Name = "pictureBox_Cover";
            this.pictureBox_Cover.Size = new System.Drawing.Size(256, 244);
            this.pictureBox_Cover.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_Cover.TabIndex = 11;
            this.pictureBox_Cover.TabStop = false;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(341, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(246, 18);
            this.label1.TabIndex = 12;
            this.label1.Text = "label_Title";
            // 
            // label_Album
            // 
            this.label_Album.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_Album.Location = new System.Drawing.Point(341, 84);
            this.label_Album.Name = "label_Album";
            this.label_Album.Size = new System.Drawing.Size(189, 18);
            this.label_Album.TabIndex = 13;
            this.label_Album.Text = "label_Album";
            // 
            // label_Year
            // 
            this.label_Year.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_Year.Location = new System.Drawing.Point(341, 113);
            this.label_Year.Name = "label_Year";
            this.label_Year.Size = new System.Drawing.Size(189, 18);
            this.label_Year.TabIndex = 14;
            this.label_Year.Text = "label_Year";
            // 
            // label_Autor
            // 
            this.label_Autor.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_Autor.Location = new System.Drawing.Point(341, 141);
            this.label_Autor.Name = "label_Autor";
            this.label_Autor.Size = new System.Drawing.Size(163, 18);
            this.label_Autor.TabIndex = 15;
            this.label_Autor.Text = "label_Autor";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(336, 234);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(194, 23);
            this.button1.TabIndex = 16;
            this.button1.Text = "Clear Playlist";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Maroon;
            this.panel1.Location = new System.Drawing.Point(547, 56);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(101, 106);
            this.panel1.TabIndex = 17;
            // 
            // Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(984, 333);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label_Autor);
            this.Controls.Add(this.label_Year);
            this.Controls.Add(this.label_Album);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox_Cover);
            this.Controls.Add(this.button_Prev);
            this.Controls.Add(this.button_Next);
            this.Controls.Add(this.button_ClearPlaylist);
            this.Controls.Add(this.label_TrakDuration);
            this.Controls.Add(this.label_CurentDuration);
            this.Controls.Add(this.trackBar_VolumeLavel);
            this.Controls.Add(this.trackBar_CurentPosition);
            this.Controls.Add(this.listBox_Playlist);
            this.Controls.Add(this.button_Open);
            this.Controls.Add(this.button_Play);
            this.Controls.Add(this.label_TrackName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Music player";
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_CurentPosition)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_VolumeLavel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Cover)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label_TrackName;
        private System.Windows.Forms.Button button_Play;
        private System.Windows.Forms.Button button_Open;
        private System.Windows.Forms.ListBox listBox_Playlist;
        private System.Windows.Forms.TrackBar trackBar_CurentPosition;
        private System.Windows.Forms.TrackBar trackBar_VolumeLavel;
        private System.Windows.Forms.Label label_CurentDuration;
        private System.Windows.Forms.Label label_TrakDuration;
        private System.Windows.Forms.Button button_ClearPlaylist;
        private System.Windows.Forms.Button button_Next;
        private System.Windows.Forms.Button button_Prev;
        private System.Windows.Forms.PictureBox pictureBox_Cover;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label_Album;
        private System.Windows.Forms.Label label_Year;
        private System.Windows.Forms.Label label_Autor;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel1;
    }
}

