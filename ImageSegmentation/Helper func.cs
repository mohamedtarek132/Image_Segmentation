using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageTemplate
{
    internal class Helper_func
    {

        static public void coutingSort(Edge[] edges)//O(4V)
        {
            // Maximum possible edge weight (255 for byte values)
            int maxWeight = 255;

            // Frequency array for weight distribution counting
            int[] count = new int[maxWeight + 1];

            // Output array for sorted edges (same size as input)
            Edge[] sorted = new Edge[edges.Length];//O(V)

            // Phase 1: Count frequency of each weight value
            foreach (Edge edge in edges)//O(V)
            {
                count[edge.weight]++;
            }

            // Phase 2: Convert counts to cumulative indexes
            int total = 0;
            for (int i = 0; i <= maxWeight; i++)
            {
                int oldCount = count[i];
                count[i] = total;   // Store start index for current weight
                total += oldCount;  // Update running total of elements
            }

            // Phase 3: Distribute edges to sorted positions
            foreach (Edge edge in edges)//O(V)
            {
                int index = count[edge.weight];
                sorted[index] = edge;
                count[edge.weight]++;   // Move to next position for same weight
            }

            // Phase 4: Copy sorted array back to original
            Array.Copy(sorted, edges, edges.Length);//O(V)
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

        static public void saveImage(PictureBox picBoxOfImgToBeSaved, string originalFilePath)
        {
            try
            {
                // Validate input file path
                if (string.IsNullOrWhiteSpace(originalFilePath))
                {
                    Console.WriteLine("Invalid original file path");
                    return;
                }

                // Get directory path safely
                string directory = Path.GetDirectoryName(originalFilePath);

                // Handle root directories and network paths
                if (string.IsNullOrEmpty(directory))
                {
                    // Use current directory if no path found
                    directory = Environment.CurrentDirectory;
                }

                // Create directory if it doesn't exist
                Directory.CreateDirectory(directory);

                string imagePath = Path.Combine(directory, "our_output.bmp");

                // Check if PictureBox has an image
                if (picBoxOfImgToBeSaved?.Image == null)
                {
                    Console.WriteLine("No image available to save");
                    return;
                }

                // Delete existing file if present
                if (File.Exists(imagePath))
                {
                    File.Delete(imagePath);
                    Console.WriteLine($"Existing image deleted: {imagePath}");
                }

                // Save the image
                picBoxOfImgToBeSaved.Image.Save(imagePath, ImageFormat.Bmp);
                Console.WriteLine($"Image saved successfully: {imagePath}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Invalid path format: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving image: {ex.Message}");
            }
        }
    }
}
