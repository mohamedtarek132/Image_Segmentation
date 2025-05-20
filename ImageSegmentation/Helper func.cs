using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageTemplate
{
    public class Point
    {
        public int x;
        public int y;

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
    }

    internal class Helper_func
    {
        int height, width;

        public Helper_func(int height, int width)
        {
            this.height = height;
            this.width = width;
        }

        static public void writeFile(string fullPath, int regionCount, int[] pixelPerRegionCount)
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

        public (int, byte)[] getneighbors(int V_indx)
        {
            int x = V_indx / width;
            int y = V_indx % width;
            List<(int, byte)> neighbors = new List<(int, byte)>();

            // Check all 8 directions (dx, dy, direction index)
            (int, int, byte)[] directions = {
                                         (0, 1, 0),
                (1, -1, 1),  (1, 0, 2),  (1, 1, 3)
            };

            foreach (var dir in directions)
            {
                int nx = x + dir.Item1;
                int ny = y + dir.Item2;
                if (nx >= 0 && nx < height && ny >= 0 && ny < width)
                    neighbors.Add((nx * width + ny, dir.Item3));
            }
            return neighbors.ToArray();
        }
    }
}
