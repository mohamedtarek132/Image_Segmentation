using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageTemplate
{
    public partial class MergedRegionForm : Form
    {
        RGBPixel[,] image;
        public MergedRegionForm(RGBPixel[,] image)
        {
            InitializeComponent();
            this.image = image;
        }

        private void MergedRegionForm_Load(object sender, EventArgs e)
        {
            ImageOperations.DisplayImage(image, pictureBox1);
        }
    }
}
