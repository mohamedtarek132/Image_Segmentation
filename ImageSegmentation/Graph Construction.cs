using static ImageTemplate.MainForm;
using System;

namespace ImageTemplate
{
    public class GraphConstruction
    {
        public static byte[,] build_graph(Color color, RGBPixel[,] ImageMatrix)
        {
            int width = ImageMatrix.GetLength(1);
            int height = ImageMatrix.GetLength(0);
            int V = width * height;

            byte[,] graph = new byte[V, 4];

            (int, int, byte)[] directions = {
                                         (0, 1, 0),
                (1, -1, 1),  (1, 0, 2),  (1, 1, 3)
            };


            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {

                    foreach (var dir in directions)
                    {
                        var (dr, dc,pos) = dir;
                        int neighborRow = row + dr;
                        int neighborCol = col + dc;

                        // Check if neighbor is within image bounds
                        if (neighborRow >= 0 && neighborRow < height && neighborCol >= 0 && neighborCol < width)
                        {
                            byte weight;
                            // Extract weight from appropriate color channel
                            switch (color)
                            {
                                case Color.Red:
                                    weight = (byte)Math.Abs(ImageMatrix[row, col].red - ImageMatrix[neighborRow, neighborCol].red);
                                    break;
                                case Color.Green:
                                    weight = (byte)Math.Abs(ImageMatrix[row, col].green - ImageMatrix[neighborRow, neighborCol].green);
                                    break;
                                case Color.Blue:
                                    weight = (byte)Math.Abs(ImageMatrix[row, col].blue - ImageMatrix[neighborRow, neighborCol].blue);
                                    break;
                                default:
                                    weight = 0;
                                    break;
                            }

                            int currentIndex = row * width + col;

                            graph[currentIndex, pos] = weight;

                        }
                    }
                }
            }
            return graph;
        }
    }
    
}