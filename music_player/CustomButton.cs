using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace music_player
{
    [DefaultEvent("Click")]
    public partial class CustomButton : UserControl
    {
        

        private int wh = 20;
        private float gradient_angle = 30;
        private Color cl0 = Color.Blue, cl1 = Color.Orange;
        //private Timer t = new Timer();
        private string label_button = "New button...";
        //new Color ForeColor = Color.White;
       
        public CustomButton()
        {

            DoubleBuffered = true;
            //t.Interval = 60;
            //t.Start();
            //t.Tick += (s, e) => { Gradient_angle = Gradient_angle % 360 + 1; };
            InitializeComponent();
        }

        private const int CS_DROPSHADOW = 0x20000;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= CS_DROPSHADOW;
                return cp;
            }
        }

        //public string 

        public int BorderRadius
        {
            get { return wh; }
            set { wh = value; Invalidate(); }
        }


        public Color Color_1
        {
            get { return cl0; }
            set { cl0 = value; Invalidate(); }
        }

        public Color Color_2
        {
            get { return cl1; }
            set { cl1 = value; Invalidate(); }
        }

        public float Gradient_angle 
        {
            get { return gradient_angle; }
            set { gradient_angle = value; Invalidate(); }
        }

        public string Label_button {
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
            gp.AddArc(new Rectangle(0, Height-wh, wh, wh), 90, 90);

            //e.Graphics.FillPath(new SolidBrush(Color.Teal), gp);
            e.Graphics.FillPath(new LinearGradientBrush(ClientRectangle,cl0,cl1, gradient_angle), gp);
            e.Graphics.DrawString(Label_button, Font, new SolidBrush(ForeColor), ClientRectangle, new StringFormat() { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center });
            base.OnPaint(e);
        }

    }
}
