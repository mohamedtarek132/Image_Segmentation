using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageTemplate
{
    internal class Helper_func
    {
        
        static public void coutingSort(Edge[] edges)
        {
            int maxWeight = 255;
            int[] count = new int[maxWeight + 1];
            Edge[] sorted = new Edge[edges.Length];

            foreach (Edge edge in edges)
            {
                count[edge.weight]++;
            }

            int total = 0;
            for (int i = 0; i <= maxWeight; i++)
            {
                int oldCount = count[i];
                count[i] = total;
                total += oldCount;
            }

            foreach (Edge edge in edges)
            {
                int index = count[edge.weight];
                sorted[index] = edge;
                count[edge.weight]++;
            }

            Array.Copy(sorted, edges, edges.Length);
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

    }
}
