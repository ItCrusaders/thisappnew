using Emgu.CV;
using Emgu.CV.Structure;
using My_EMGU_Program.helpers;
using My_EMGU_Program.models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace My_EMGU_Program
{
    public partial class Form2 : Form
    {
        Image<Bgr, Byte> My_Image;
        Image<Gray, Byte> gray_image;
        Image<Bgr, Byte> My_image_copy;

        bool gray_in_use = false;
        public Form2()
        {
            InitializeComponent();
                image_PCBX.MouseDown += new MouseEventHandler(picOriginal_MouseDown);
             image_PCBX.MouseMove += new MouseEventHandler(picOriginal_MouseMove);
         //  image_PCBX.MouseUp += new MouseEventHandler(picOriginal_MouseUp);
            image_PCBX.MouseWheel += new MouseEventHandler(zoom);
        }

        private Point getUnscaled()
        {
            Point p = image_PCBX.PointToClient(Cursor.Position);
            Point unscaled_p = new Point();

            // image and container dimensions
            int w_i = image_PCBX.Image.Width;
            int h_i = image_PCBX.Image.Height;
            int w_c = image_PCBX.Width;
            int h_c = image_PCBX.Height;

            float imageRatio = w_i / (float)h_i; // image W:H ratio
            float containerRatio = w_c / (float)h_c; // container W:H ratio

            if (imageRatio >= containerRatio)
            {
                // horizontal image
                float scaleFactor = w_c / (float)w_i;
                float scaledHeight = h_i * scaleFactor;
                // calculate gap between top of container and top of image
                float filler = Math.Abs(h_c - scaledHeight) / 2;
                unscaled_p.X = (int)(p.X / scaleFactor);
                unscaled_p.Y = (int)((p.Y - filler) / scaleFactor);
            }
            else
            {
                // vertical image
                float scaleFactor = h_c / (float)h_i;
                float scaledWidth = w_i * scaleFactor;
                float filler = Math.Abs(w_c - scaledWidth) / 2;
                unscaled_p.X = (int)((p.X - filler) / scaleFactor);
                unscaled_p.Y = (int)(p.Y / scaleFactor);
            }

            return unscaled_p;
        }


        Point prevp = new Point(0, 0);
        private void zoom(object sender, MouseEventArgs e)
        {
            HandledMouseEventArgs ee = (HandledMouseEventArgs)e;
            ee.Handled = true;

            prvPoint = (new Point(e.X, e.Y));

            float zm;
            if (e.Delta < 0)
            {

                zm = 80f / 100f;
                if (image_PCBX.Height <= original.Height * zm || image_PCBX.Width <= original.Width * zm)
                {
                    return;
                }
            }
            else
            {
                zm = 125f / 100f;

            }


            int y = (int)(prvPoint.Y * zm) - prvPoint.Y - image_PCBX.Top;
            int x = (int)(prvPoint.X * zm) - prvPoint.X - image_PCBX.Left;
            if (zm > 1)
            {
                if (flowLayoutPanel1.VerticalScroll.Visible != true && flowLayoutPanel1.HorizontalScroll.Visible != true)
                {

                    Cursor.Position = new Point(Cursor.Position.X, Cursor.Position.Y + y);
                    Cursor.Position = new Point(Cursor.Position.X + x, Cursor.Position.Y);
                }
            }

            image_PCBX.Width = (int)(image_PCBX.Width * zm);
            image_PCBX.Height = (int)(image_PCBX.Height * zm);




            var loc = new Point((int)(prvPoint.X * zm), (int)(prvPoint.Y * zm));
            loc -= new Size(flowLayoutPanel1.AutoScrollPosition);
            loc -= new Size(prvPoint.X, prvPoint.Y);
            flowLayoutPanel1.AutoScrollPosition = loc;


            if (zm < 1)
            {
                if (flowLayoutPanel1.VerticalScroll.Visible != true && flowLayoutPanel1.HorizontalScroll.Visible != true)
                {

                    Cursor.Position = new Point(Cursor.Position.X, Cursor.Position.Y + y);
                    Cursor.Position = new Point(Cursor.Position.X + x, Cursor.Position.Y);
                }
            }


        }
        Point prvPoint = new Point(0, 0);


        int hasStart = 0;
        public List<Point> somePoints = new List<Point>();

        
        public double calLength(double x, double y)
        {
            return Math.Sqrt(x*x + y*y);
        }

        private void Load_BTN_Click(object sender, EventArgs e)
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
                drawBitmap = new Bitmap(original);
               // ww = image_PCBX.Width;
               // hh = image_PCBX.Height;

                My_image_copy = My_Image.Copy();



            }
        }

        dimensions dms = new dimensions();

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
        private void Suppress(int spectrum)
        {

            for (int i = 0; i < My_Image.Height; i++)
            {
                for (int j = 0; j < My_Image.Width; j++)
                {
                    My_image_copy.Data[i, j, spectrum] = 0;
                }
            }

        }

        private void Un_Suppress(int spectrum)
        {
            for (int i = 0; i < My_Image.Height; i++)
            {
                for (int j = 0; j < My_Image.Width; j++)
                {
                    My_image_copy.Data[i, j, spectrum] = My_Image.Data[i, j, spectrum];
                }
            }
        }


        Bitmap original;
        bool isSelecting;
        float x0, y0, x1, y1;

        #region helpder methods

        // Start selecting the rectangle.
        private void picOriginal_MouseDown(object sender, MouseEventArgs e)

        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                // Right cliclk.
            }
            else {

                if (original != null)
                {
                  //  if(activeItem.Length>0)
                    if (isSelecting == false)
                    {
                       // if (activeItem.Length > 0)
                            isSelecting = true;

                        // Save the start point.
                        getBounds();
                        Point P = TranslateZoomMousePosition(new Point(e.X, e.Y));
                        x0 = P.X;// (e.X * P.X) - lb;// * ratioX;// g.X;// * scaledFactor.Width;// image_PCBX.PointToClient(Cursor.Position).X;
                        y0 = P.Y;// (e.Y*P.Y) - tb;// * ratioY;// g.Y;
                    }
                    else
                    {
                        picOriginal_MouseUp(sender, e);
                    }
                }

            }
        }


        int lb = 0;
        int rb = 0;

        int tb = 0;
        int bb = 0;

        private void getBounds()
        {
            lb = (image_PCBX.Width - original.Width) / 2;
            rb = lb;

            tb = (image_PCBX.Height - original.Height) / 2;
            bb = tb;
        }

     
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 mm = new Form1();
            mm.Show();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                checkBox1.Checked = false;
                checkBox2.Checked = false;
                ItemC cd = Prompt.ShowDialogNew("area");
                if(cd != null)
                {
                    listAreas.Items.Add(cd.name);
                    Object2d _2d = new Object2d();
                    _2d.type = 0;
                    _2d.color = cd.color;

                    areas.Add(cd.name, _2d);
                    selectedIndex = listAreas.Items.Count - 1;
                    activeItem = cd.name;
                    listAreas.Invalidate();
                }
            }
            else
            {
                if (!checkBox2.Checked)
                    checkBox2.Checked = true;
              //  checkBox2.Checked = false;
            }
        }
        Dictionary<String,Object2d> areas = new Dictionary<String, Object2d>();
       // Dictionary<String, Object2d> lines = new Dictionary<String, Object2d>();

        String activeItem = "";

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
          //      x0 = -1;
            //    y0=-1;
                ItemC cd = Prompt.ShowDialogNew("line length");
                linesList.Items.Add(cd.name);
                Object2d _2d = new Object2d();
                _2d.type = 0;
                _2d.color=cd.color;

                areas.Add(cd.name, _2d);
                activeItem = cd.name;
                selectedIndex = linesList.Items.Count - 1;
                linesList.Invalidate();

                checkBox1.Checked = false;
                if(checkBox3.Checked)
                checkBox3.Checked = false;
            }
            else
            {

                if (!checkBox3.Checked)
                    checkBox3.Checked= true;
                checkBox1.Checked= false;
            }
        }

        // Continue selecting.
        private void picOriginal_MouseMove(object sender, MouseEventArgs e)
        {
            if (original != null)
            {
                // Do nothing it we're not selecting an area.
                if (!isSelecting) return;

                
                Point P = TranslateZoomMousePosition(new Point(e.X, e.Y));
                x1 = P.X;// e.X - lb;// * ratioX;// - g.X;// * scaledFactor.Width;// image_PCBX.PointToClient(Cursor.Position).X;
                y1 = P.Y;// e.Y - tb;// * ratioY;// - g.Y;// * scaledFactor.Height;// image_PCBX.PointToClient(Cursor.Position).Y;

                // Make a Bitmap to display the selection rectangle.
                Bitmap bm = new Bitmap((Bitmap)drawBitmap);


                using (Graphics gr = Graphics.FromImage(bm))
                {

               //     gr.MultiplyTransform(image_PCBX.ScaleM);
                    float idth = Math.Abs(x0 - x1);
                    float eight = Math.Abs(y0 - y1);
                    Rectangle rrr = new Rectangle();//
                    rrr.X = (int)Math.Min(x0, x1);
                    rrr.Y = (int)Math.Min(y0, y1);//new Point(x0, y0), new Size(idth, eight));
                    rrr.Width = (int)idth;
                    rrr.Height = (int)eight;



                    //    this.Invalidate();

                    if (checkBox1.Checked)
                    {
                        Pen d = new Pen(Color.Red,2);
                        gr.DrawLine(d, new Point((int)x0, (int)y0), new Point((int)x1, (int)y1));
                    }
                    else if (checkBox3.Checked)
                    {
                        Pen d = new Pen(areas[activeItem].color,2);
                        gr.DrawLine(d, new Point((int)x0, (int)y0), new Point((int)x1, (int)y1));
                    }
                    else if (checkBox2.Checked)
                    {

                        Pen d = new Pen(areas[activeItem].color,2);
                        gr.DrawLine(d, new Point((int)x0, (int)y0), new Point((int)x1, (int)y1));
                    }
                    using (Brush b = new SolidBrush(Color.FromArgb(150, Color.White)))
                    {
                //        gr.FillRectangle(b, mHoverRectangle);
                    }
                }
                  //  this.Invalidate();
//                image_PCBX.Image = original;
                // Display the temporary bitmap.
                
                image_PCBX.Image = bm;
            }
        }


        Bitmap drawBitmap;

        private void drawline(Rectangle rt)
        {

        }

        int selectedIndex = -1;

        private void linesList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(linesList.SelectedIndex > -1)
            {
                activeItem = linesList.Items[linesList.SelectedIndex].ToString();
                selectedIndex = linesList.SelectedIndex;

            }
        }

        private void listAreas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (linesList.SelectedIndex > -1)
            {
                selectedIndex = listAreas.SelectedIndex;
                activeItem = linesList.Items[linesList.SelectedIndex].ToString();
            }
        }

        Object2d scalePoint = new Object2d();

        private void sx(object sender, EventArgs e)
        {

        }
        Boolean selc = false;
        private void stopMeasurement_Click(object sender, EventArgs e)
        {
            isSelecting =false;
            if (checkBox3.Checked)
            {
                Object2d _2d = areas[activeItem];
                double area = _2d.calcArea();
                float scalenoww = image_PCBX.Image.Width / dms.scalew;
                float scalenowh = image_PCBX.Image.Height / dms.scaleh;
                //   area = area * dms.userwidth * scalenoww / dms.iwidth;
                // float l = 0.0f;
                //l = (float)Math.Sqrt(
                //          (Math.Pow(Math.Abs(x1 - x0) * dms.userwidth * scalenoww / dms.iwidth, 2.0))
                //         +
                //          (Math.Pow(Math.Abs(y1 - y0) * dms.userwidth * scalenowh / dms.iwidth, 2.0)));

                MessageBox.Show("area = " + area);
                listAreas.Items[selectedIndex] +=" "+ area;
                using (var path = new GraphicsPath())
                {
                    path.AddPolygon(_2d.getAllPoints().ToArray());

                    using (Graphics gr = Graphics.FromImage(drawBitmap))
                    {
                        using (var brush = new SolidBrush(Color.Black))
                        {
                            gr.FillPath(brush, path);
                        }
                    }
                }
            }
            else if (checkBox2.Checked)
            {

                Object2d _2d = areas[activeItem];
                double l = 0.0f;
                l = _2d.getLength();

                linesList.Items[selectedIndex] += " " + l;
                MessageBox.Show("Length is : " + l);
            }
        }

        int activeline = 0;
        // Finish selecting the area.
        private void picOriginal_MouseUp(object sender, MouseEventArgs e)
        {
            if (original != null)
            {
                // Do nothing it we're not selecting an area.
                if (!isSelecting) return;
                isSelecting = false;

                int wid = (int)Math.Abs(x0 - x1);
                int hgt = (int)Math.Abs(y0 - y1);
                Rectangle r = new Rectangle();
                r.X = (int)x0;
                r.Y = (int)x0;

                r.Width = wid;
                r.Height = hgt;
              //  Bitmap bm= drawBitmap;
                using (Graphics gr = Graphics.FromImage(drawBitmap))
                {
                    int idth = (int)Math.Abs(x0 - x1);
                    int eight = (int)Math.Abs(y0 - y1);
                    Rectangle rrr = new Rectangle();//
                    rrr.X = (int)Math.Min(x0, x1);
                    rrr.Y = (int)Math.Min(y0, y1);//new Point(x0, y0), new Size(idth, eight));
                    rrr.Width = idth;
                    rrr.Height = eight;

                    if (checkBox1.Checked)
                    {
                        if (dms.scalew!=0.0)
                        {
                            Pen p = new Pen(Color.BlueViolet,2);//Pens.BlueViolet;
                          //  p.Width = 20;
                            gr.DrawLine(p, new Point((int)x0, (int)y0), new Point((int)x1, (int)y1));
                          
                            {
                                using (Brush b = new SolidBrush(Color.FromArgb(150, Color.BlueViolet)))
                                {
                                    float scalenoww = image_PCBX.Image.Width / dms.scalew;
                                    float scalenowh = image_PCBX.Image.Height / dms.scaleh;
                                    float l = 0.0f;
                                    l = (float)Math.Sqrt(
                                              (Math.Pow(Math.Abs(x1 - x0) * dms.userwidth * scalenoww / dms.iwidth, 2.0))
                                             +
                                              (Math.Pow(Math.Abs(y1 - y0) * dms.userwidth * scalenowh / dms.iwidth, 2.0)));


                                    int p1 = (int)Math.Abs(x1 + x0) / 2;
                                    int p2 = (int)Math.Abs(y1 + y0) / 2;

                                    gr.DrawString(l + "cm",
                                        new Font("Arial", 14 * scalenoww, FontStyle.Bold), 
                                        b,
                                        new Point(p1 - 20, p2 - 20));
                                }
                                // MessageBox.Show("iwidth " + area);
                            }
                        }

                        else
                        {
                           
                            string ll = Prompt.ShowDialog("Enter the Length", "Length");
                            if (ll.Length == 0)
                                return;
                            Pen p = new Pen(Color.Red, 2);
                            gr.DrawLine(p, new Point((int)x0, (int)y0), new Point((int)x1, (int)y1));
                            // string ll = Prompt.ShowDialog("Enter the Length", "Length");
                            if (ll != null || ll.Length > 0)
                            {
                                dms.iwidth = Math.Abs(x0 - x1);
                                dms.userwidth = float.Parse(ll);
                                dms.scalew = image_PCBX.Image.Width;
                                dms.scaleh = image_PCBX.Image.Height;
                                dms.iheight = Math.Abs(y0 - y1);// image_PCBX.Image.Height;// dms.userwidth / dms.iwidth;
                                using (Brush b = new SolidBrush(Color.FromArgb(150, Color.Red)))
                                {

                                    float scalenow = image_PCBX.Image.Width / dms.scalew;
                                    int p1 = (int)Math.Abs(x1 + x0) / 2;
                                    int p2 = (int)Math.Abs(y1 + y0) / 2;
                                    gr.DrawString(ll + "cm", new Font("Arial", 14 * scalenow, FontStyle.Bold), b,
                                        new Point(p1 - 20, p2 - 20));
                                }
                                // MessageBox.Show("iwidth " + area);
                            }
                        }
                       
                        
                    }
                    else //if (checkBox3.Checked)
                    {
                    //    gr.DrawRectangle(Pens.Red,
                    //    rrr);
                    //    float scalenow = image_PCBX.Image.Width / dms.scalew;
                    //    float hh1 = image_PCBX.Image.Height / dms.iheight;
                    //    float ww = rrr.Width * dms.userwidth * scalenow / dms.iwidth;
                    //    float hh = rrr.Height * dms.userwidth * hh1 / dms.iwidth;
                    //    float area = ww * hh;

                    //    MessageBox.Show("Area is " + area);
                    //}
                    //else if (checkBox2.Checked)
                    //{
                        Object2d _2d = areas[activeItem];
                        Point pp = new Point((int)x1, (int)y1);
                        if (_2d.lines.Count > 0)
                        {
                            Point pp2 = _2d.lines[0].getLines()[0];
                            if (pp.X == pp2.X && pp.Y == pp2.Y)
                            {
                                double area = _2d.calcArea();
                              //  MessageBox.Show("area = " + area);
                            }
                        }


                            float scalenoww = image_PCBX.Image.Width / dms.scalew;
                        float scalenowh = image_PCBX.Image.Height / dms.scaleh;

                        float xs= dms.userwidth* scalenoww / dms.iwidth;
                        float ys = dms.userwidth * scalenowh / dms.iwidth;

                        if (activeline == 0)
                        {
                            Lines la = new Lines();
                            la.setPoint1(new Point((int)x0, (int)y0),0,0);
                            la.setPoint2(new Point((int)x1, (int)y1), xs, ys);
                            _2d.addLine(la);
                            activeline = 0;

                            isSelecting = true;
                        }
                        else
                        {
                            //                  Lines la = _2d.getLines().Last();
                            //                List<Lines> ss = _2d.lines;
                            //              ss[ss.Count - 1] = la;
                            //            _2d.lines = ss;
                            //          x0 = x1;
                            //        y0 = y1;
                        }
            //            _2d.addPoints(new Point((int)x1, (int)y1));
                        gr.DrawLine(new Pen(_2d.color,2), new Point((int)x0, (int)y0), new Point((int)x1, (int)y1));
                       
                        x0 = x1;
                        y0 = y1;
                    }
                }

                image_PCBX.Image = drawBitmap;
                
                if ((wid < 1) || (hgt < 1)) return;

               
            }
        }

        private void showss(Bitmap aa)
        {
            Image<Bgr, byte> source = My_Image; // Image B
            Image<Bgr, byte> template = new Image<Bgr, byte>(aa); // Image A
            Image<Bgr, byte> imageToShow = source.Copy();

           

            using (Image<Gray, float> result = source.MatchTemplate(template,
                Emgu.CV.CvEnum.TM_TYPE.CV_TM_CCOEFF_NORMED))
            {
                double[] minValues, maxValues;
                Point[] minLocations, maxLocations;
                result.MinMax(out minValues, out maxValues, out minLocations, out maxLocations);
                result.Split();

                for (int i = 0; i < maxValues.Length; i++)
                {
                    Console.WriteLine("hahah " + maxValues[i]);
                    // You can try different values of the threshold. I guess somewhere between 0.75 and 0.95 would be good.
                    //  if (maxValues[i] > 0.9)
                    {
                        // This is a match. Do something with it, for example draw a rectangle around it.
                        Rectangle match = new Rectangle(maxLocations[i], template.Size);
                        imageToShow.Draw(match, new Bgr(Color.Red), 3);
                    }
                }
            }

            // Show imageToShow in an ImageBox (here assumed to be called imageBox1)
            image_PCBX.Image = imageToShow.ToBitmap();
        }

        #endregion

    }
}
