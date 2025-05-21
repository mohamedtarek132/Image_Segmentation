using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace ImageTemplate
{
    public partial class MainForm : Form
    {
        
        public MainForm()
        {
            InitializeComponent();
        }


        RGBPixel[,] ImageMatrix;
        int regionCount;
        int[] pixelPerRegionCount;
        string OpenedFilePath;
        seg s;

        List<(int,int)> points;

        private void btnOpen_Click(object sender, EventArgs e)
        {
            points = new List<(int, int)>();

            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Open the browsed image and display it
                OpenedFilePath = openFileDialog1.FileName;
                ImageMatrix = ImageOperations.OpenImage(OpenedFilePath);
                ImageOperations.DisplayImage(ImageMatrix, pictureBox1);
            }
            txtWidth.Text = ImageOperations.GetWidth(ImageMatrix).ToString();
            txtHeight.Text = ImageOperations.GetHeight(ImageMatrix).ToString();
        }

        private async void btnGaussSmooth_Click(object sender, EventArgs e)
        {
            double sigma = double.Parse(txtGaussSigma.Text);
            int maskSize = (int)nudMaskSize.Value ;
            int k = int.Parse(textBox1.Text);
            ImageMatrix = ImageOperations.GaussianFilter1D(ImageMatrix, maskSize, sigma);

            Stopwatch timer = Stopwatch.StartNew();

            s = new seg(ImageMatrix.GetLength(0), ImageMatrix.GetLength(1),k, ImageMatrix);
            
            await Task.Run(() => (ImageMatrix, regionCount, pixelPerRegionCount) = s.ProccessImage());
            
            timer.Stop();
            long time = timer.ElapsedMilliseconds;
            
            textBox2.Text = (time/1000).ToString();
            ImageOperations.DisplayImage(ImageMatrix, pictureBox2);

            Helper_func.writeFile(OpenedFilePath, regionCount, pixelPerRegionCount);
            Helper_func.saveImage(pictureBox2, OpenedFilePath);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void nudMaskSize_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_MouseClick(object sender, MouseEventArgs e)
        {
            if (pictureBox2.Image == null)
                return;
            
            points.Add((e.Y,e.X));
            MessageBox.Show($"x= {e.X}, y= {e.Y}");
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            RGBPixel[,] mergedImage = s.MergeRegions(points);
            points = new List<(int, int)>();
            ImageOperations.DisplayImage(mergedImage, pictureBox3);
        }
    }
}