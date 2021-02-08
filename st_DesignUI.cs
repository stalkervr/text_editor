using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace st_DesignUI
{
    class st_Button : Button
    {
        private StringFormat SF = new StringFormat();

        private bool MouseEntered = false;
        private bool MousePressed = false;
        public st_Button()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
            DoubleBuffered = true;

            Size = new System.Drawing.Size(100, 30);

            BackColor = Color.Tomato;
            //BorderColor = BackColor;
            ForeColor = Color.White;

            SF.Alignment = StringAlignment.Center;
            SF.LineAlignment = StringAlignment.Center;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics graph = e.Graphics;
            graph.SmoothingMode = SmoothingMode.HighQuality;
            graph.Clear(Parent.BackColor);

            Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);
            graph.DrawRectangle(new Pen(BackColor), rect);
            graph.FillRectangle(new SolidBrush(BackColor), rect);

            if (MouseEntered)
            {
                graph.DrawRectangle(new Pen(Color.FromArgb(60, Color.White)), rect);
                graph.FillRectangle(new SolidBrush(Color.FromArgb(60, Color.White)), rect);
            }

            if (MousePressed)
            {
                graph.DrawRectangle(new Pen(Color.FromArgb(30, Color.Black)), rect);
                graph.FillRectangle(new SolidBrush(Color.FromArgb(30, Color.Black)), rect);
            }

            graph.DrawString(Text, Font, new SolidBrush(ForeColor), rect, SF);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            MouseEntered = true;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {

            base.OnMouseLeave(e);
            MouseEntered = false;
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            MousePressed = true;
            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            MousePressed = false;
            Invalidate();
        }
    }
    //***********************************************************************************************
    class st_ButtonCircle : Control
    {
        private StringFormat SF = new StringFormat();

        private bool MouseEntered = false;
        private bool MousePressed = false;

        private Image icon = null;


        public Image Icon
        {
            get { return icon; }
            set { icon = value; Invalidate(); }
        }

        public st_ButtonCircle()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
            DoubleBuffered = true;

            Size = new System.Drawing.Size(100, 30);

            BackColor = Color.Tomato;
            //BorderColor = BackColor;
            ForeColor = Color.White;

            SF.Alignment = StringAlignment.Center;
            SF.LineAlignment = StringAlignment.Center;
            //Icon = icon;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics graph = e.Graphics;
            graph.SmoothingMode = SmoothingMode.HighQuality;
            graph.Clear(Parent.BackColor);

            RectangleF rect = new RectangleF(0.0F, 0.0F, Width - 1, Height - 1);
            graph.DrawEllipse(new Pen(BackColor), rect);
            graph.FillEllipse(new SolidBrush(BackColor), rect);

            if (MouseEntered)
            {
                graph.DrawEllipse(new Pen(Color.FromArgb(60, Color.White)), rect);
                graph.FillEllipse(new SolidBrush(Color.FromArgb(60, Color.White)), rect);
            }

            if (MousePressed)
            {
                graph.DrawEllipse(new Pen(Color.FromArgb(30, Color.Black)), rect);
                graph.FillEllipse(new SolidBrush(Color.FromArgb(30, Color.Black)), rect);
            }

            graph.DrawString(Text, Font, new SolidBrush(ForeColor), rect, SF);
            //graph.DrawImage(icon, rect);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            MouseEntered = true;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {

            base.OnMouseLeave(e);
            MouseEntered = false;
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            MousePressed = true;
            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            MousePressed = false;
            Invalidate();
        }
    }
    //**********************************************************************

    class st_ButtonGradient : UserControl
    {
        private int wh = 20;
        private float gradient_angle = 30;
        private Color cl0 = Color.Blue, cl1 = Color.Orange;
        //private Timer t = new Timer();
        private string label_button = "New button...";
        //new Color ForeColor = Color.White;

        public st_ButtonGradient()
        {

            DoubleBuffered = true;
            BackColor = Color.Transparent;
            //t.Interval = 60;
            //t.Start();
            //t.Tick += (s, e) => { Gradient_angle = Gradient_angle % 360 + 1; };
            //InitializeComponent();
        }
        //public string 

        public int BorderRadius
        {
            get { return wh; }
            set { wh = value; Invalidate(); }
        }

        [Category("st_DesignUI Props")]
        public Color Color_1
        {
            get { return cl0; }
            set { cl0 = value; Invalidate(); }
        }
        [Category("st_DesignUI Props")]
        public Color Color_2
        {
            get { return cl1; }
            set { cl1 = value; Invalidate(); }
        }
        [Category("st_DesignUI Props")]
        public float Gradient_angle
        {
            get { return gradient_angle; }
            set { gradient_angle = value; Invalidate(); }
        }

        public string Label_button
        {
            get { return label_button; }
            set { label_button = value; Invalidate(); }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;

            GraphicsPath gp = new GraphicsPath();

            gp.AddArc(new Rectangle(0, 0, wh, wh), 180, 90);
            gp.AddArc(new Rectangle(Width - wh, 0, wh, wh), -90, 90);
            gp.AddArc(new Rectangle(Width - wh, Height - wh, wh, wh), 0, 90);
            gp.AddArc(new Rectangle(0, Height - wh, wh, wh), 90, 90);

            //e.Graphics.FillPath(new SolidBrush(Color.Teal), gp);
            e.Graphics.FillPath(new LinearGradientBrush(ClientRectangle, cl0, cl1, gradient_angle), gp);
            e.Graphics.DrawString(Label_button, Font, new SolidBrush(ForeColor), ClientRectangle, new StringFormat() { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center });
            base.OnPaint(e);
        }
    }

    //*****************************************************************************

    public partial class st_DragPanel : Component
    {
        private Control handleControl;

        public Control SelectControl
        {
            get
            {
                return this.handleControl;
            }
            set
            {
                this.handleControl = value;
                this.handleControl.MouseDown += new MouseEventHandler(this.DragForm_MouseDown);
            }
        }
        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr a, int msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        private void DragForm_MouseDown(object sender, MouseEventArgs e)
        {
            bool flag = e.Button == MouseButtons.Left;
            if (flag)
            {
                st_DragPanel.ReleaseCapture();
                st_DragPanel.SendMessage(this.SelectControl.FindForm().Handle, 161, 2, 0);
            }
        }
    }
}

