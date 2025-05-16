using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageTemplate
{
    internal class Helper_func
    {
        int height, width;

        public Helper_func(int height, int width)
        {
            this.height = height;
            this.width = width;
        }

        public (int, byte)[] getneighbors(int V_indx)
        {
            int x = V_indx / width;
            int y = V_indx % width;
            List<(int, byte)> neighbors = new List<(int, byte)>();

            // Check all 8 directions (dx, dy, direction index)
            (int, int, byte)[] directions = {
        (-1, -1, 0), (-1, 0, 1), (-1, 1, 2),
        (0, -1, 3),          (0, 1, 4),
        (1, -1, 5),  (1, 0, 6),  (1, 1, 7)
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
