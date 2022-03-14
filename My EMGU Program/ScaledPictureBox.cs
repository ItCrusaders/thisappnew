using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace My_EMGU_Program
{
    public partial class ScaledPictureBox : PictureBox
    {
        public Matrix ScaleM { get; set; }

        float Zoom { get; set; }
        Size ImgSize { get; set; }

        public ScaledPictureBox()
        {
          //  InitializeComponent();
            ScaleM = new Matrix();
            SizeMode = PictureBoxSizeMode.StretchImage;
        }

        public void InitImage()
        {
            if (Image != null)
            {
                ImgSize = Image.Size;
                Size = ImgSize;
                SetZoom(100);
            }
        }

        public void SetZoom(float zoomfactor)
        {
            if (zoomfactor <= 0) throw new Exception("Zoom must be positive");
            float oldZoom = Zoom;
            Zoom = zoomfactor / 100f;
            ScaleM.Reset();
            ScaleM.Scale(Zoom, Zoom);
            if (ImgSize != Size.Empty) Size = new Size((int)(ImgSize.Width * Zoom),
                                                       (int)(ImgSize.Height * Zoom));

        }

        public Rectangle ScalePoint(Rectangle pt)
        {  pt.X = (int)(pt.X/ Zoom); pt.Y = (int)(pt.Y/ Zoom);
            pt.Width = (int)(pt.Width / Zoom);
            pt.Height = (int)(pt.Height / Zoom);
            return pt; }



    }
}
