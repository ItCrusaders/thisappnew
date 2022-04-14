using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Emgu.CV;
using Emgu.Util;
using Emgu.CV.Structure;
using EQS_2._0.repositories;

namespace My_EMGU_Program
{
    public partial class Form1oo : Form
    {
        Image<Bgr, Byte> My_Image;
        Image<Gray, Byte> gray_image;
        Image<Bgr, Byte> My_image_copy;

        bool gray_in_use = false;

        public Form1oo()
        {
            InitializeComponent();
            image_PCBX.MouseDown += new MouseEventHandler(picOriginal_MouseDown);
            image_PCBX.MouseMove += new MouseEventHandler(picOriginal_MouseMove);
            image_PCBX.MouseUp += new MouseEventHandler(picOriginal_MouseUp);
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

                My_image_copy = My_Image.Copy();

                //Set sepctrum choice
                Red_Spectrum_CHCK.Checked = true;
                Red_Spectrum_CHCK.Enabled = true;
                Green_Spectrum_CHCK.Checked = true;
                Green_Spectrum_CHCK.Enabled = true;
                Blue_Spectrum_CHCK.Checked = true;
                Blue_Spectrum_CHCK.Enabled = true;


            }
        }

        private void image_PCBX_MouseMove(object sender, MouseEventArgs e)
        {
            if (image_PCBX.Image != null)
            {
                X_pos_LBL.Text = "X: " + e.X.ToString();
                Y_pos_LBL.Text = "Y: " + e.Y.ToString();

                if (gray_in_use)
                {
                    Val_LBL.Text = "Value: " + gray_image[e.Y, e.X].ToString();
                }
                else
                {
                    Val_LBL.Text = "Value: " + My_Image[e.Y, e.X].ToString();
                }
                //It is much more stable with large images to access the image.Data propert directley than use code like bellow
                //Bitmap tmp_img = new Bitmap(image_PCBX.Image);
                //Val_LBL.Text = "Value: " + tmp_img.GetPixel(e.X, e.Y).ToString();
            }
        }

        private void Convert_btn_Click(object sender, EventArgs e)
        {
            if (My_Image != null)
            {
                if (gray_in_use)
                {
                    gray_in_use = false;

                    Bitmap mm = My_Image.ToBitmap();
                    //Display the Image
                    image_PCBX.Image = mm;
                    original = mm;
                    Convert_btn.Text = "Convert to Gray";

                    Red_Spectrum_CHCK.Checked = true;
                    Red_Spectrum_CHCK.Enabled = true;
                    Green_Spectrum_CHCK.Checked = true;
                    Green_Spectrum_CHCK.Enabled = true;
                    Blue_Spectrum_CHCK.Checked = true;
                    Blue_Spectrum_CHCK.Enabled = true;
                }
                else
                {
                    gray_image = My_Image.Convert<Gray, Byte>();
                    gray_in_use = true;

                    Bitmap mm = gray_image.ToBitmap();
                    //Display the Image
                    image_PCBX.Image = mm;
                    original = mm;
                   // image_PCBX.Image = gray_image.ToBitmap();
                    Convert_btn.Text = "Convert to Colour";

                    Red_Spectrum_CHCK.Enabled = false;
                    Green_Spectrum_CHCK.Enabled = false;
                    Blue_Spectrum_CHCK.Enabled = false;
                }
            }

        }

        private void Red_Spectrum_CHCK_CheckedChanged(object sender, EventArgs e)
        {
            if (!Red_Spectrum_CHCK.Checked)
            {
                //Remove Red Spectrum programatically
                Suppress(2);
            }
            else
            {
                //Add Red Spectrum programatically
                Un_Suppress(2);
            }
            image_PCBX.Image = My_image_copy.ToBitmap();
        }

        private void Green_Spectrum_CHCK_CheckedChanged(object sender, EventArgs e)
        {
            if (!Green_Spectrum_CHCK.Checked)
            {
                //Remove Green Spectrum programatically
                Suppress(1);
            }
            else
            {
                //Add Green Spectrum programatically
                Un_Suppress(1);
            }
            image_PCBX.Image = My_image_copy.ToBitmap();
        }

        private void Blue_Spectrum_CHCK_CheckedChanged(object sender, EventArgs e)
        {
            if (!Blue_Spectrum_CHCK.Checked)
            {
                //Remove Blue Spectrum programatically
                Suppress(0);
            }
            else
            {
                //Add Blue Spectrum programatically
                Un_Suppress(0);
            }
            image_PCBX.Image = My_image_copy.ToBitmap();
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
        int x0, y0, x1, y1;

        #region helpder methods

        // Start selecting the rectangle.
        private void picOriginal_MouseDown(object sender, MouseEventArgs e)
        {
            if (original != null)
            {
                isSelecting = true;
                // Save the start point.
                x0 = e.X;
                y0 = e.Y;
            }
        }

        // Continue selecting.
        private void picOriginal_MouseMove(object sender, MouseEventArgs e)
        {
            if (original != null)
            {
                // Do nothing it we're not selecting an area.
                if (!isSelecting) return;

                // Save the new point.
                x1 = e.X;
                y1 = e.Y;

                // Make a Bitmap to display the selection rectangle.
                Bitmap bm = new Bitmap(original);

                // Draw the rectangle.
                float zoomX = (float)original.Size.Width / image_PCBX.Width;
                float zoomY = (float)original.Size.Height / image_PCBX.Height;

                using (Graphics gr = Graphics.FromImage(bm))
                {
                    gr.DrawRectangle(Pens.Red,
                        Math.Min(x0, x1) * zoomX, Math.Min(y0, y1) * zoomY,
                        Math.Abs(x0 - x1) * zoomX, Math.Abs(y0 - y1) * zoomY
                        );
                }
                // Display the temporary bitmap.
                image_PCBX.Image = new Bitmap(bm, new Size(image_PCBX.Width, image_PCBX.Height));
            }
        }

        private void image_PCBX_Click(object sender, EventArgs e)
        {

        }

        // Finish selecting the area.
        private void picOriginal_MouseUp(object sender, MouseEventArgs e)
        {
            if (original != null)
            {
                // Do nothing it we're not selecting an area.
                if (!isSelecting) return;
                isSelecting = false;

                // Display the original image.
                image_PCBX.Image = original;

                // Copy the selected part of the image.
                int wid = Math.Abs(x0 - x1);
                int hgt = Math.Abs(y0 - y1);
                if ((wid < 1) || (hgt < 1)) return;

                Bitmap area = new Bitmap(wid, hgt);
                using (Graphics gr = Graphics.FromImage(area))
                {
                    Rectangle source_rectangle =
                        new Rectangle(Math.Min(x0, x1), Math.Min(y0, y1),
                            wid, hgt);
                    Rectangle dest_rectangle =
                        new Rectangle(0, 0, wid, hgt);
                    gr.DrawImage(original, dest_rectangle,
                        source_rectangle, GraphicsUnit.Pixel);
                }

                //pictureBox1.Image = area;
                showss(area);
                // Display the result.
                // pictureBox2.Image = area;
            }
        }

        private void showss(Bitmap aa)
        {
            Image<Bgr, byte> source =My_Image; // Image B
            Image<Bgr, byte> template = new Image<Bgr, byte>(aa); // Image A
            Image<Bgr, byte> imageToShow = source.Copy();

           // MCvMat mat = new MCvMat();

       //     Mat asa = new Mat();

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

        private void button1_Click(object sender, EventArgs e)
        {
            // My_Image.
        }
    }
}
