using My_EMGU_Program.helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace My_EMGU_Program
{
    public partial class TypeSelector : Form
    {
        public TypeSelector()
        {
            InitializeComponent();
        }

        public void showDialog()
        {

        }

        private void colorP_Click(object sender, EventArgs e)
        {
            colorDialog1.AllowFullOpen = true;
            DialogResult dr  = colorDialog1.ShowDialog();
            if(dr == DialogResult.OK)
            {
                this.rr = colorDialog1.Color;//.ToString();
                label2.ForeColor = rr;
//                MessageBox.Show(r);
            }

        }
        Color rr;

        public ItemC ds = new ItemC();
        public ItemC returnEvent()
        {
            ds.name = tb_name.Text;
            this.ds.color = this.rr;
            Console.WriteLine(rr.ToString()+" ss "+this.ds.color.ToString());
            return ds;
        }

    }
}
