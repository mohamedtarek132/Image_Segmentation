using static ImageTemplate.MainForm;
using System;

namespace ImageTemplate
{
    public class GraphConstruction
    {
        public static byte[,] build_graph(Color color, RGBPixel[,] ImageMatrix)
        {
            // Get image dimensions from the 2D pixel matrix
            int height = ImageMatrix.GetLength(0);
            int width = ImageMatrix.GetLength(1);

            // Total vertices/nodes in graph (each pixel is a node)
            int V = width * height;

            // Initialize graph: 
            // 2D array where each row represents a pixel, and 4 columns represent 4-directional edges
            // Directions: Right, Down-Left, Down, Down-Right (see directions array below)
            byte[,] graph = new byte[V, 4];

            // Define neighbor directions as (delta_row, delta_col, position_index) tuples:
            // [0] Right       (0, 1)
            // [1] Down-Left  (1, -1)
            // [2] Down       (1, 0)
            // [3] Down-Right (1, 1)
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

                            // Convert 2D pixel position to 1D graph node index
                            int currentIndex = row * width + col;

                            // Store weight in graph: current node -> direction position
                            graph[currentIndex, pos] = weight;

                        }
                    }
                }
            }
            return graph;
        }
    }
    
}