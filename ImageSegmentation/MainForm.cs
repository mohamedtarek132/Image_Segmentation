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

        private void btnOpen_Click(object sender, EventArgs e)
        {
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

            seg s = new seg(ImageMatrix.GetLength(0), ImageMatrix.GetLength(1),k);
            
            await Task.Run(() => (ImageMatrix, regionCount, pixelPerRegionCount) = s.segmentImage(GraphConstruction.build_graph(ImageMatrix)));
            
            timer.Stop();
            long time = timer.ElapsedMilliseconds;
            
            Console.WriteLine("time : " + time);
            Console.WriteLine("number of finel regions : " + regionCount);
            ImageOperations.DisplayImage(ImageMatrix, pictureBox2);

            writeFile(OpenedFilePath, regionCount, pixelPerRegionCount);

        }

        private void writeFile(string fullPath, int regionCount, int[] pixelPerRegionCount)
        {
            string filePath = Path.GetDirectoryName(fullPath);
            filePath = Path.Combine(filePath, "our_output.txt");

            try
            {
                // Check if file exists before attempting to delete
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    Console.WriteLine($"File deleted successfully: {filePath}");
                }
                else
                {
                    Console.WriteLine($"File does not exist: {filePath}");
                }

                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    // Write the count as the first line
                    writer.WriteLine(regionCount);

                    // Write each array element on subsequent lines
                    foreach (var item in pixelPerRegionCount)
                    {
                        writer.WriteLine(item);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting file: {ex.Message}");
            }

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
    }
}