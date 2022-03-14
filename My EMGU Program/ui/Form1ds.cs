using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace My_EMGU_Program.ui
{
    public partial class Form1ds : Form
    {
        public Form1ds()
        {
            InitializeComponent();
            image_PCBX.MouseWheel += new MouseEventHandler(zoom);
        }

        Image<Bgr, Byte> My_Image;

        Bitmap original;
        Image<Bgr, Byte> My_image_copy;
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog Openfile = new OpenFileDialog();
            if (Openfile.ShowDialog() == DialogResult.OK)
            {
                //  ImageRepo.ConvertToBitmap(Openfile.FileName);
                //Load the Image
                My_Image = new Image<Bgr, Byte>(Openfile.FileName);

                Bitmap mm = My_Image.ToBitmap();

                //Display the Image
                image_PCBX.Image = mm;
                original = mm;
                //  drawBitmap = new Bitmap(original);
                // ww = image_PCBX.Width;
                // hh = image_PCBX.Height;

        //        image_PCBX.Width = (int)(image_PCBX.Width * 2);// + zoomRatio / 100;
          //      image_PCBX.Height = (int)(image_PCBX.Height * 2);
                My_image_copy = My_Image.Copy();



            }
        }
        protected  void OnMouseWheel(MouseEventArgs ea)
        {
            //  flag = 1;
            // Override OnMouseWheel event, for zooming in/out with the scroll wheel
            if (image_PCBX.Image != null)
            {
                // If the mouse wheel is moved forward (Zoom in)
                if (ea.Delta > 0)
                {
                    // Check if the pictureBox dimensions are in range (15 is the minimum and maximum zoom level)
                    if ((image_PCBX.Width < (15 * this.Width)) && (image_PCBX.Height < (15 * this.Height)))
                    {
                        // Change the size of the picturebox, multiply it by the ZOOMFACTOR
                        image_PCBX.Width = (int)(image_PCBX.Width * 1.25);
                        image_PCBX.Height = (int)(image_PCBX.Height * 1.25);

                        // Formula to move the picturebox, to zoom in the point selected by the mouse cursor
                        image_PCBX.Top = (int)(ea.Y - 1.25 * (ea.Y - image_PCBX.Top));
                        image_PCBX.Left = (int)(ea.X - 1.25 * (ea.X - image_PCBX.Left));
                    }
                }
                else
                {
                    // Check if the pictureBox dimensions are in range (15 is the minimum and maximum zoom level)
                    if ((image_PCBX.Width > (original.Width)) && (image_PCBX.Height > (original.Height)))
                    {
                        // Change the size of the picturebox, divide it by the ZOOMFACTOR
                        image_PCBX.Width = (int)(image_PCBX.Width / 1.25);
                        image_PCBX.Height = (int)(image_PCBX.Height / 1.25);

                        // Formula to move the picturebox, to zoom in the point selected by the mouse cursor
                        image_PCBX.Top = (int)(ea.Y - 0.80 * (ea.Y - image_PCBX.Top));
                        image_PCBX.Left = (int)(ea.X - 0.80 * (ea.X - image_PCBX.Left));
                    }
                }
            }
        }
        Point prevp = new Point(0,0);
        private void zoom(object sender, MouseEventArgs e)
        {
            HandledMouseEventArgs ee = (HandledMouseEventArgs)e;
            ee.Handled = true;
            //    prvPoint.X = e.X;
            //      prvPoint.Y = e.Y;

            prvPoint = (new Point(e.X, e.Y));
            prevp = TranslateZoomMousePosition(prvPoint);

            float zm;
            if (e.Delta < 0)
            {

                zm = 80f/100f;
                if(image_PCBX.Height <= original.Height *zm || image_PCBX.Width <= original.Width*zm)
                {
                    return;
                }
            }
            else
            {
                zm = 125f/100f;
               
            }
            //    if(flowLayoutPanel1.Height > image_PCBX.Height * zm)


            int y = (int)(prvPoint.Y * zm) - prvPoint.Y - image_PCBX.Top;
            //    y *= image_PCBX.Width / image_PCBX.Height;
            int x = (int)(prvPoint.X * zm) - prvPoint.X - image_PCBX.Left;
            if (zm > 1)
            {
                if (flowLayoutPanel1.VerticalScroll.Visible != true && flowLayoutPanel1.HorizontalScroll.Visible != true)
                {

                    //         if (flowLayoutPanel1.VerticalScroll.Visible != true)
                    Cursor.Position = new Point(Cursor.Position.X, Cursor.Position.Y + y);
                    Cursor.Position = new Point(Cursor.Position.X + x, Cursor.Position.Y);
                }
            }
          
            //      if (flowLayoutPanel1.Width > image_PCBX.Width * zm)
            {

             //   if (flowLayoutPanel1.HorizontalScroll.Visible != true)
            //        Cursor.Position = new Point(Cursor.Position.X + x, Cursor.Position.Y);
            }
            image_PCBX.Width = (int)(image_PCBX.Width * zm );// + zoomRatio / 100;
            image_PCBX.Height = (int)(image_PCBX.Height * zm);

           

            //
            //           flowLayoutPanel1.AutoScrollPosition = new Point((int)(prvPoint.X + zm* (prvPoint.X)),
            //            (int)(prvPoint.Y + zm * (prvPoint.Y)));  
            //              image_PCBX.Top = (int)(prvPoint.Y - zm * (prvPoint.Y - image_PCBX.Top));
            //            image_PCBX.Left = (int)(prvPoint.X - zm * (prvPoint.X - image_PCBX.Left));
            Point xx = new Point(image_PCBX.Left,image_PCBX.Top);
            //   Console.WriteLine("prevp is " + prvPoint.ToString());
            //    Console.WriteLine("xx is "+xx.ToString());
            // image_PCBX.Top = prvPoint.Y;
            //image_PCBX.Left = prvPoint.X;
            //   flowLayoutPanel1.AutoScrollPosition = new Point(flowLayoutPanel1.Left + (int)(prvPoint.X * zm ),//+ (int)(e.X * zm / 100f),// * (e.X - image_PCBX.Top)), 
            //      flowLayoutPanel1.Top+ (int)(prvPoint.Y * zm));// + (int)(e.Y * zm / 100f));// * (e.Y - image_PCBX.Top)));


            //   prvPoint = (new Point(e.X, e.Y));
            //    Console.WriteLine("newp is " + prvPoint.ToString());
            //   Cursor.
            //hScrollBar1.sc

            var loc = new Point((int)(prvPoint.X*zm), (int)(prvPoint.Y*zm));
            loc -= new Size(flowLayoutPanel1.AutoScrollPosition);
            loc-= new Size (prvPoint.X,prvPoint.Y);
            flowLayoutPanel1.AutoScrollPosition = loc;

      
            if(zm < 1)
            {
                if (flowLayoutPanel1.VerticalScroll.Visible != true && flowLayoutPanel1.HorizontalScroll.Visible != true)
                {

                    //         if (flowLayoutPanel1.VerticalScroll.Visible != true)
                    Cursor.Position = new Point(Cursor.Position.X, Cursor.Position.Y + y);
                    Cursor.Position = new Point(Cursor.Position.X + x, Cursor.Position.Y);
                }
            }


            //          flowLayoutPanel1.VerticalScroll.Value = prvPoint.Y;


            //            flowLayoutPanel1.AutoScrollPosition
            // flowLayoutPanel1.VerticalScroll.SmallChange = (int)(prvPoint.Y * zm) - prvPoint.Y;
            //  flowLayoutPanel1.AutoScrollPosition = new Point((int)(prvPoint.X * zm) - prvPoint.X,
            //     (int)(prvPoint.Y * zm) - prvPoint.Y);// prvPoint.X, prvPoint.Y);
            //       image_PCBX.Top = image_PCBX.Top - (int)(prvPoint.Y * zm) - prvPoint.Y;// (int)(prvPoint.X * zm);
            //     image_PCBX.Left = image_PCBX.Left - (int)(prvPoint.X * zm) - prvPoint.X;// (int)(prvPoint.Y * zm);
            y = (int)(prvPoint.Y * zm) - prvPoint.Y - image_PCBX.Top;
            //    y *= image_PCBX.Width / image_PCBX.Height;
             x = (int)(prvPoint.X * zm) - prvPoint.X - image_PCBX.Left;

            

            Console.WriteLine(e.X+"  0000-  "+e.Y);
         //   Point relativeLoc = new Point(flowLayoutPanel1.X - form.Location.X, flowLayoutPanel1.Y - form.Location.Y);
            Point yyau = 
            flowLayoutPanel1.Location;//MousePosition;
          //  Point yyu = image_PCBX.PointT(yyau);
         //   Console.WriteLine(yyu.X  + "  mm-  " +
           //     (yyu.Y ));

            // Mouse
            //   flowLayoutPanel1.HorizontalScroll.Value = x;//// (int)(prvPoint.X - zm * (prvPoint.X - image_PCBX.Left));// * (e.X - image_PCBX.Left));
            //    Console.WriteLine("dd"+zm+ " ver " + flowLayoutPanel1.VerticalScroll.Value + " " + y + " max" + flowLayoutPanel1.VerticalScroll.Maximum);
            // flowLayoutPanel1.VerticalScroll.

            // flowLayoutPanel1.VerticalScroll.Value += y;// (int)(prvPoint.Y - zm * (prvPoint.Y - image_PCBX.Top));// * (e.Y - image_PCBX.Top));
            if (zm > 0)
            {
                y = -y;
                x=-x;
          //      flowLayoutPanel1.AutoScrollPosition += new Point(x, y);
               // image_PCBX.Location =new Point(x,y);
//                image_PCBX.Location.X= x;
            }
            else
            {
                y = (int)((prvPoint.Y) - prvPoint.Y/zm);
                //    y *= image_PCBX.Width / image_PCBX.Height;
                x = (int)((prvPoint.X) - prvPoint.X/zm);
                //   flowLayoutPanel1.HorizontalScroll.Value = 0;
                //     flowLayoutPanel1.PerformLayout();
                //   image_PCBX.Location= new Point(x,y);

          //      flowLayoutPanel1.AutoScrollPosition = new Point(x, y);
             //   image_PCBX.PerformLayout();
            }
      //      flowLayoutPanel1.AutoScrollPosition = new Point(x,y);
        //     flowLayoutPanel1.PerformLayout();
            //  MessageBox.Show(flowLayoutPanel1.VerticalScroll.ToString());
           //    flowLayoutPanel1.PerformLayout();
         //   Console.WriteLine(" ver " + flowLayoutPanel1.VerticalScroll.Value + " " + y+" max"+flowLayoutPanel1.VerticalScroll.Maximum);


        }
        Point prvPoint = new Point(0, 0);

        protected Point TranslateZoomMousePosition(Point coordinates)
        {
            // test to make sure our image is not null
            if (image_PCBX.Image == null) return coordinates;
            // Make sure our control width and height are not 0 and our 
            // image width and height are not 0
            if (image_PCBX.Width == 0 || image_PCBX.Height == 0 ||
                image_PCBX.Image.Width == 0 || image_PCBX.Image.Height == 0) return coordinates;
            // This is the one that gets a little tricky. Essentially, need to check 
            // the aspect ratio of the image to the aspect ratio of the control
            // to determine how it is being rendered
            float imageAspect = (float)image_PCBX.Image.Width / image_PCBX.Image.Height;
            float controlAspect = (float)image_PCBX.Width / image_PCBX.Height;
            float newX = coordinates.X;
            float newY = coordinates.Y;
            if (imageAspect > controlAspect)
            {
                // This means that we are limited by width, 
                // meaning the image fills up the entire control from left to right
                float ratioWidth = (float)image_PCBX.Image.Width / image_PCBX.Width;
                newX *= ratioWidth;
                float scale = (float)image_PCBX.Width / image_PCBX.Image.Width;
                float displayHeight = scale * image_PCBX.Image.Height;
                float diffHeight = image_PCBX.Height - displayHeight;
                diffHeight /= 2;
                newY -= diffHeight;
                newY /= scale;
            }
            else
            {
                // This means that we are limited by height, 
                // meaning the image fills up the entire control from top to bottom
                float ratioHeight = (float)image_PCBX.Image.Height / image_PCBX.Height;
                newY *= ratioHeight;
                float scale = (float)image_PCBX.Height / image_PCBX.Image.Height;
                float displayWidth = scale * image_PCBX.Image.Width;
                float diffWidth = image_PCBX.Width - displayWidth;
                diffWidth /= 2;
                newX -= diffWidth;
                newX /= scale;
            }
            return new Point((int)newX, (int)newY);
        }
        private void image_PCBX_MouseMove(object sender, MouseEventArgs e)
        {
        }

        private void flowLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void image_PCBX_MouseMove_1(object sender, MouseEventArgs e)
        {

            label3.Text = "[" + e.X + "," + e.Y + "]";
        }

        Point p = new Point();
        private void image_PCBX_Click(object sender, EventArgs e)
        {
            p = TranslateZoomMousePosition(new Point(MousePosition.X-image_PCBX.Location.X,
                MousePosition.Y-image_PCBX.Location.Y));
        }
    }
}
