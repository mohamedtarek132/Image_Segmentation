using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ImageTemplate
{
    public partial class MainForm : Form
    {
        
        public MainForm()
        {
            InitializeComponent();
        }

        public struct RGBWeight
        {
            public byte red, green, blue;
        }

        RGBPixel[,] ImageMatrix;
        public RGBWeight[,] build_graph()
        {
            int Width = ImageMatrix.GetLength(1);
            int height = ImageMatrix.GetLength(0);
            int v = Width * height;

            RGBWeight[,] graph = new RGBWeight[v, 8];
            int index = 0;
            for (int i = 0;i<height-1; i++)
            {
                for (int j = 0;j<Width; j++)
                {
                    //S
                    graph[index, 6].blue = Convert.ToByte(Math.Abs(ImageMatrix[i, j].blue - ImageMatrix[i + 1, j].blue));
                    graph[index, 6].green = Convert.ToByte(Math.Abs(ImageMatrix[i, j].green - ImageMatrix[i + 1, j].green));
                    graph[index, 6].red = Convert.ToByte(Math.Abs(ImageMatrix[i, j].red - ImageMatrix[i + 1, j].red));
                    graph[index + Width, 1].blue = graph[index, 6].blue;
                    graph[index + Width, 1].green = graph[index, 6].green;
                    graph[index + Width, 1].red = graph[index, 6].red;
                    if (j == 0)
                    {
                        //0 1 2  NW N NE
                        //3   4  W    E
                        //5 6 7  SW S SE
                        //E
                        graph[index,4].blue = Convert.ToByte(Math.Abs(ImageMatrix[i,j].blue - ImageMatrix[i,j+1].blue));
                        graph[index, 4].green = Convert.ToByte(Math.Abs(ImageMatrix[i, j].green - ImageMatrix[i, j + 1].green));
                        graph[index, 4].red = Convert.ToByte(Math.Abs(ImageMatrix[i, j].red - ImageMatrix[i, j + 1].red));
                        graph[index + 1, 3].blue = graph[index, 4].blue;
                        graph[index + 1, 3].green = graph[index, 4].green;
                        graph[index + 1, 3].red = graph[index, 4].red;
                        
                        //SE
                        graph[index, 7].blue = Convert.ToByte(Math.Abs(ImageMatrix[i, j].blue - ImageMatrix[i + 1, j+1].blue));
                        graph[index, 7].green = Convert.ToByte(Math.Abs(ImageMatrix[i, j].green - ImageMatrix[i + 1, j+1].green));
                        graph[index, 7].red = Convert.ToByte(Math.Abs(ImageMatrix[i, j].red - ImageMatrix[i + 1, j+1].red));
                        graph[index + Width+1, 0].blue = graph[index, 7].blue;
                        graph[index + Width+1, 0].green = graph[index, 7].green;
                        graph[index + Width+1, 0].red = graph[index, 7].red;

                    }
                    else if(j == Width - 1)
                    {
                        //SW
                        graph[index, 5].blue = Convert.ToByte(Math.Abs(ImageMatrix[i, j].blue - ImageMatrix[i + 1, j-1].blue));
                        graph[index, 5].green = Convert.ToByte(Math.Abs(ImageMatrix[i, j].green - ImageMatrix[i + 1, j-1].green));
                        graph[index, 5].red = Convert.ToByte(Math.Abs(ImageMatrix[i, j].red - ImageMatrix[i + 1, j - 1].red));
                        graph[index + Width-1, 2].blue = graph[index, 5].blue;
                        graph[index + Width-1, 2].green = graph[index, 5].green;
                        graph[index + Width-1, 2].red = graph[index, 5].red;
                    }
                    else
                    {
                        //E
                        graph[index, 4].blue = Convert.ToByte(Math.Abs(ImageMatrix[i, j].blue - ImageMatrix[i, j + 1].blue));
                        graph[index, 4].green = Convert.ToByte(Math.Abs(ImageMatrix[i, j].green - ImageMatrix[i, j + 1].green));
                        graph[index, 4].red = Convert.ToByte(Math.Abs(ImageMatrix[i, j].red - ImageMatrix[i, j + 1].red));
                        graph[index + 1, 3].blue = graph[index, 4].blue;
                        graph[index + 1, 3].green = graph[index, 4].green;
                        graph[index + 1, 3].red = graph[index, 4].red;
                        //SE
                        graph[index, 7].blue = Convert.ToByte(Math.Abs(ImageMatrix[i, j].blue - ImageMatrix[i + 1, j + 1].blue));
                        graph[index, 7].green = Convert.ToByte(Math.Abs(ImageMatrix[i, j].green - ImageMatrix[i + 1, j + 1].green));
                        graph[index, 7].red = Convert.ToByte(Math.Abs(ImageMatrix[i, j].red - ImageMatrix[i + 1, j + 1].red));
                        graph[index + Width + 1, 0].blue = graph[index, 7].blue;
                        graph[index + Width + 1, 0].green = graph[index, 7].green;
                        graph[index + Width + 1, 0].red = graph[index, 7].red;
                        //SW
                        graph[index, 5].blue = Convert.ToByte(Math.Abs(ImageMatrix[i, j].blue - ImageMatrix[i + 1, j - 1].blue));
                        graph[index, 5].green = Convert.ToByte(Math.Abs(ImageMatrix[i, j].green - ImageMatrix[i + 1, j - 1].green));
                        graph[index, 5].red = Convert.ToByte(Math.Abs(ImageMatrix[i, j].red - ImageMatrix[i + 1, j - 1].red));
                        graph[index + Width - 1, 2].blue = graph[index, 5].blue;
                        graph[index + Width - 1, 2].green = graph[index, 5].green;
                        graph[index + Width - 1, 2].red = graph[index, 5].red;
                    }
                    index++;
                }
            }
            
            for (int j = 0; j < Width-1; j++)
            {
                //ES
                graph[index, 4].blue = Convert.ToByte(Math.Abs(ImageMatrix[height-1, j].blue - ImageMatrix[height - 1, j + 1].blue));
                graph[index, 4].green = Convert.ToByte(Math.Abs(ImageMatrix[height - 1, j].green - ImageMatrix[height - 1, j + 1].green));
                graph[index, 4].red = Convert.ToByte(Math.Abs(ImageMatrix[height - 1, j].red - ImageMatrix[height - 1, j + 1].red));
                graph[index + 1, 3].blue = graph[index, 4].blue;
                graph[index + 1, 3].green = graph[index, 4].green;
                graph[index + 1, 3].red = graph[index, 4].red;
            }


            return graph;
        }
        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Open the browsed image and display it
                string OpenedFilePath = openFileDialog1.FileName;
                ImageMatrix = ImageOperations.OpenImage(OpenedFilePath);
                ImageOperations.DisplayImage(ImageMatrix, pictureBox1);
            }
            txtWidth.Text = ImageOperations.GetWidth(ImageMatrix).ToString();
            txtHeight.Text = ImageOperations.GetHeight(ImageMatrix).ToString();
        }

        private void btnGaussSmooth_Click(object sender, EventArgs e)
        {
            double sigma = double.Parse(txtGaussSigma.Text);
            int maskSize = (int)nudMaskSize.Value ;
            ImageMatrix = ImageOperations.GaussianFilter1D(ImageMatrix, maskSize, sigma);
            ImageOperations.DisplayImage(ImageMatrix, pictureBox2);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }
    }
}