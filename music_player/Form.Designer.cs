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
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_CurentPosition)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_VolumeLavel)).BeginInit();
            this.SuspendLayout();
            // 
            // label_TrackName
            // 
            this.label_TrackName.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label_TrackName.Location = new System.Drawing.Point(13, 13);
            this.label_TrackName.Name = "label_TrackName";
            this.label_TrackName.Size = new System.Drawing.Size(633, 38);
            this.label_TrackName.TabIndex = 0;
            this.label_TrackName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button_Play
            // 
            this.button_Play.Location = new System.Drawing.Point(18, 70);
            this.button_Play.Name = "button_Play";
            this.button_Play.Size = new System.Drawing.Size(194, 23);
            this.button_Play.TabIndex = 1;
            this.button_Play.Text = "Play";
            this.button_Play.UseVisualStyleBackColor = true;
            this.button_Play.Click += new System.EventHandler(this.button_Play_Click);
            // 
            // button_Open
            // 
            this.button_Open.Location = new System.Drawing.Point(452, 70);
            this.button_Open.Name = "button_Open";
            this.button_Open.Size = new System.Drawing.Size(194, 23);
            this.button_Open.TabIndex = 2;
            this.button_Open.Text = "Open";
            this.button_Open.UseVisualStyleBackColor = true;
            this.button_Open.Click += new System.EventHandler(this.button_Open_Click);
            // 
            // listBox_Playlist
            // 
            this.listBox_Playlist.FormattingEnabled = true;
            this.listBox_Playlist.Location = new System.Drawing.Point(18, 217);
            this.listBox_Playlist.Name = "listBox_Playlist";
            this.listBox_Playlist.Size = new System.Drawing.Size(546, 212);
            this.listBox_Playlist.TabIndex = 3;
            this.listBox_Playlist.SelectedIndexChanged += new System.EventHandler(this.listBox_Playlist_SelectedIndexChanged);
            // 
            // trackBar_CurentPosition
            // 
            this.trackBar_CurentPosition.Location = new System.Drawing.Point(18, 149);
            this.trackBar_CurentPosition.Name = "trackBar_CurentPosition";
            this.trackBar_CurentPosition.Size = new System.Drawing.Size(546, 45);
            this.trackBar_CurentPosition.TabIndex = 4;
            this.trackBar_CurentPosition.Scroll += new System.EventHandler(this.trackBar_CurentPosition_Scroll);
            // 
            // trackBar_VolumeLavel
            // 
            this.trackBar_VolumeLavel.Location = new System.Drawing.Point(601, 149);
            this.trackBar_VolumeLavel.Maximum = 100;
            this.trackBar_VolumeLavel.Name = "trackBar_VolumeLavel";
            this.trackBar_VolumeLavel.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trackBar_VolumeLavel.Size = new System.Drawing.Size(45, 280);
            this.trackBar_VolumeLavel.TabIndex = 5;
            this.trackBar_VolumeLavel.Value = 50;
            this.trackBar_VolumeLavel.Scroll += new System.EventHandler(this.trackBar_VolumeLavel_Scroll);
            // 
            // label_CurentDuration
            // 
            this.label_CurentDuration.AutoSize = true;
            this.label_CurentDuration.Location = new System.Drawing.Point(18, 111);
            this.label_CurentDuration.Name = "label_CurentDuration";
            this.label_CurentDuration.Size = new System.Drawing.Size(0, 13);
            this.label_CurentDuration.TabIndex = 6;
            // 
            // label_TrakDuration
            // 
            this.label_TrakDuration.AutoSize = true;
            this.label_TrakDuration.Location = new System.Drawing.Point(610, 111);
            this.label_TrakDuration.Name = "label_TrakDuration";
            this.label_TrakDuration.Size = new System.Drawing.Size(0, 13);
            this.label_TrakDuration.TabIndex = 7;
            this.label_TrakDuration.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // button_ClearPlaylist
            // 
            this.button_ClearPlaylist.Location = new System.Drawing.Point(18, 449);
            this.button_ClearPlaylist.Name = "button_ClearPlaylist";
            this.button_ClearPlaylist.Size = new System.Drawing.Size(75, 23);
            this.button_ClearPlaylist.TabIndex = 8;
            this.button_ClearPlaylist.Text = "Clear Playlist";
            this.button_ClearPlaylist.UseVisualStyleBackColor = true;
            this.button_ClearPlaylist.Click += new System.EventHandler(this.button_ClearPlaylist_Click);
            // 
            // button_Next
            // 
            this.button_Next.Location = new System.Drawing.Point(359, 99);
            this.button_Next.Name = "button_Next";
            this.button_Next.Size = new System.Drawing.Size(287, 23);
            this.button_Next.TabIndex = 9;
            this.button_Next.Text = "Next";
            this.button_Next.UseVisualStyleBackColor = true;
            this.button_Next.Click += new System.EventHandler(this.button_Next_Click);
            // 
            // button_Prev
            // 
            this.button_Prev.Location = new System.Drawing.Point(18, 99);
            this.button_Prev.Name = "button_Prev";
            this.button_Prev.Size = new System.Drawing.Size(271, 23);
            this.button_Prev.TabIndex = 10;
            this.button_Prev.Text = "Prev";
            this.button_Prev.UseVisualStyleBackColor = true;
            this.button_Prev.Click += new System.EventHandler(this.button_Prev_Click);
            // 
            // Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(679, 677);
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
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Music player";
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_CurentPosition)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_VolumeLavel)).EndInit();
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
    }
}

