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

            byte[,] graph = new byte[V, 8];

            (int, int, byte)[] directions = {
                                         (0, 1, 4),
                (1, -1, 5),  (1, 0, 6),  (1, 1, 7)
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

                            int neighborIndex = neighborRow * width + neighborCol;
                            int mirroredDirection = 7 - pos;
                            graph[neighborIndex, mirroredDirection] = weight;

                        }
                    }
                }
            }
            return graph;
        }
        //public static RGBWeight[,] build_graph2(RGBPixel[,] ImageMatrix)
        //{
        //    // Extract image dimensions from 2D array
        //    // height = number of rows (vertical dimension)
        //    // width = number of columns (horizontal dimension)
        //    int width = ImageMatrix.GetLength(1);
        //    int height = ImageMatrix.GetLength(0);
        //    int V = width * height;

        //    // Create adjacency matrix for 8-directional graph
        //    // Directions: [0]NW, [1]N, [2]NE, [3]W, [4]E, [5]SW, [6]S, [7]SE
        //    RGBWeight[,] graph = new RGBWeight[V, 8];

        //    int index = 0; // Linear index tracking current pixel (row-major order)

        //    // Process all rows except last one (avoid south boundary overflow)
        //    for (int row = 0; row < height - 1; row++)
        //    {
        //        for (int col = 0; col < width; col++)
        //        {
        //            // SOUTH CONNECTION (↓) - Always available except in last row
        //            // Current pixel: [row, col], South neighbor: [row+1, col]
        //            var southWeight = new RGBWeight
        //            {
        //                red = (byte)Math.Abs(ImageMatrix[row, col].red - ImageMatrix[row + 1, col].red),
        //                green = (byte)Math.Abs(ImageMatrix[row, col].green - ImageMatrix[row + 1, col].green),
        //                blue = (byte)Math.Abs(ImageMatrix[row, col].blue - ImageMatrix[row + 1, col].blue)
        //            };

        //            // Set south connection for current pixel (direction 6)
        //            graph[index, 6] = southWeight;
        //            // Set mirrored north connection for southern neighbor (direction 1)
        //            graph[index + width, 1] = southWeight;

        //            // Handle column-specific connections
        //            if (col == 0) // First column - no west neighbors
        //            {
        //                // EAST CONNECTION (→) to [row, col+1]
        //                var eastWeight = new RGBWeight
        //                {
        //                    red = (byte)Math.Abs(ImageMatrix[row, col].red - ImageMatrix[row, col + 1].red),
        //                    green = (byte)Math.Abs(ImageMatrix[row, col].green - ImageMatrix[row, col + 1].green),
        //                    blue = (byte)Math.Abs(ImageMatrix[row, col].blue - ImageMatrix[row, col + 1].blue)
        //                };
        //                graph[index, 4] = eastWeight;
        //                graph[index + 1, 3] = eastWeight; // Mirror west connection

        //                // SOUTH-EAST DIAGONAL (↘) to [row+1, col+1]
        //                var seWeight = new RGBWeight
        //                {
        //                    red = (byte)Math.Abs(ImageMatrix[row, col].red - ImageMatrix[row + 1, col + 1].red),
        //                    green = (byte)Math.Abs(ImageMatrix[row, col].green - ImageMatrix[row + 1, col + 1].green),
        //                    blue = (byte)Math.Abs(ImageMatrix[row, col].blue - ImageMatrix[row + 1, col + 1].blue)
        //                };
        //                graph[index, 7] = seWeight;
        //                graph[index + width + 1, 0] = seWeight; // Mirror NW connection
        //            }
        //            else if (col == width - 1) // Last column - no east neighbors
        //            {
        //                // SOUTH-WEST DIAGONAL (↙) to [row+1, col-1]
        //                var swWeight = new RGBWeight
        //                {
        //                    red = (byte)Math.Abs(ImageMatrix[row, col].red - ImageMatrix[row + 1, col - 1].red),
        //                    green = (byte)Math.Abs(ImageMatrix[row, col].green - ImageMatrix[row + 1, col - 1].green),
        //                    blue = (byte)Math.Abs(ImageMatrix[row, col].blue - ImageMatrix[row + 1, col - 1].blue)
        //                };
        //                graph[index, 5] = swWeight;
        //                graph[index + width - 1, 2] = swWeight; // Mirror NE connection
        //            }
        //            else // Middle columns - full east/west connectivity
        //            {
        //                // EAST CONNECTION (→)
        //                var eastWeight = new RGBWeight
        //                {
        //                    red = (byte)Math.Abs(ImageMatrix[row, col].red - ImageMatrix[row, col + 1].red),
        //                    green = (byte)Math.Abs(ImageMatrix[row, col].green - ImageMatrix[row, col + 1].green),
        //                    blue = (byte)Math.Abs(ImageMatrix[row, col].blue - ImageMatrix[row, col + 1].blue)
        //                };
        //                graph[index, 4] = eastWeight;
        //                graph[index + 1, 3] = eastWeight;

        //                // SOUTH-EAST DIAGONAL (↘)
        //                var seWeight = new RGBWeight
        //                {
        //                    red = (byte)Math.Abs(ImageMatrix[row, col].red - ImageMatrix[row + 1, col + 1].red),
        //                    green = (byte)Math.Abs(ImageMatrix[row, col].green - ImageMatrix[row + 1, col + 1].green),
        //                    blue = (byte)Math.Abs(ImageMatrix[row, col].blue - ImageMatrix[row + 1, col + 1].blue)
        //                };
        //                graph[index, 7] = seWeight;
        //                graph[index + width + 1, 0] = seWeight;

        //                // SOUTH-WEST DIAGONAL (↙)
        //                var swWeight = new RGBWeight
        //                {
        //                    red = (byte)Math.Abs(ImageMatrix[row, col].red - ImageMatrix[row + 1, col - 1].red),
        //                    green = (byte)Math.Abs(ImageMatrix[row, col].green - ImageMatrix[row + 1, col - 1].green),
        //                    blue = (byte)Math.Abs(ImageMatrix[row, col].blue - ImageMatrix[row + 1, col - 1].blue)
        //                };
        //                graph[index, 5] = swWeight;
        //                graph[index + width - 1, 2] = swWeight;
        //            }
        //            index++;
        //        }
        //    }

        //    // Process final row horizontal connections (no southern neighbors)
        //    int lastRow = height - 1;
        //    for (int col = 0; col < width - 1; col++)
        //    {
        //        // EAST CONNECTION (→) in final row
        //        var eastWeight = new RGBWeight
        //        {
        //            red = (byte)Math.Abs(ImageMatrix[lastRow, col].red - ImageMatrix[lastRow, col + 1].red),
        //            green = (byte)Math.Abs(ImageMatrix[lastRow, col].green - ImageMatrix[lastRow, col + 1].green),
        //            blue = (byte)Math.Abs(ImageMatrix[lastRow, col].blue - ImageMatrix[lastRow, col + 1].blue)
        //        };
        //        graph[index, 4] = eastWeight;
        //        graph[index + 1, 3] = eastWeight; // Mirror west connection
        //        index++;
        //    }

        //    return graph;
        //}
    }
    
}