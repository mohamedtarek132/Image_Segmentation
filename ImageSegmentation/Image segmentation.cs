using System.Collections.Generic;
using System.Windows.Forms;
using System;
using System.Runtime.CompilerServices;

namespace ImageTemplate
{
    public class seg
    {
        int height, width;
        double k;  // Threshold function parameter controlling region merging
        int v;     // Total number of pixels (height * width)

        public seg(int h, int w, int k)
        {
            this.k = k;
            this.height = h;
            this.width = w;
            v = h * w;  // Calculate total number of vertices
        }

        // Main segmentation entry point
        public RGBPixel[,] segmentImage(RGBWeight[,] graph)
        {
            // Initialize disjoint sets for each color channel
            DisjointSet RedComponents = new DisjointSet(v);
            DisjointSet GreenComponents = new DisjointSet(v);
            DisjointSet BlueComponents = new DisjointSet(v);

            // Build and sort edges for each color channel
            Edge[] RedEdges = buildEdgeArray(Color.Red, graph);
            Edge[] GreenEdges = buildEdgeArray(Color.Green, graph);
            Edge[] BlueEdges = buildEdgeArray(Color.Blue, graph);

            // Merge components for each color channel independently
            RedComponents = mergeComponents(RedComponents, RedEdges);
            GreenComponents = mergeComponents(GreenComponents, GreenEdges);
            BlueComponents = mergeComponents(BlueComponents, BlueEdges);

            // Combine results from all three channels using intersection
            DisjointSet finalRegions = buildRegions(RedComponents, GreenComponents, BlueComponents);

            // Generate color-coded visualization of the regions
            RGBPixel[,] segmentedImage = visualizeRegions(finalRegions);

            // Diagnostic output for region counts
            HashSet<int> red = new HashSet<int>();
            HashSet<int> green = new HashSet<int>();
            HashSet<int> blue = new HashSet<int>();
            HashSet<int> t = new HashSet<int>();

            for (int i = 0; i < v; i++)
            {
                red.Add(RedComponents.Find(i));
                green.Add(GreenComponents.Find(i));
                blue.Add(BlueComponents.Find(i));
                t.Add(finalRegions.Find(i));
            }

            Console.WriteLine(red.Count);
            Console.WriteLine(green.Count);
            Console.WriteLine(blue.Count);
            Console.WriteLine(t.Count);

            return segmentedImage;
        }

        // Construct sorted edge list for a specific color channel
        private Edge[] buildEdgeArray(Color color, RGBWeight[,] graph)
        {
            Helper_func help = new Helper_func(height, width);
            List<Edge> edgesList = new List<Edge>();

            // Iterate through all pixels
            for (int i = 0; i < v; i++)
            {
                // Get 8-connected neighbors for current pixel
                (int, byte)[] neighbors = help.getneighbors(i);
                foreach (var n in neighbors)
                {
                    int neighborIndex = n.Item1;
                    // Ensure each edge is only added once (i < neighborIndex)
                    if (i < neighborIndex)
                    {
                        byte w;
                        // Extract weight from appropriate color channel
                        switch (color)
                        {
                            case Color.Red:
                                w = graph[i, n.Item2].red;
                                break;
                            case Color.Green:
                                w = graph[i, n.Item2].green;
                                break;
                            case Color.Blue:
                                w = graph[i, n.Item2].blue;
                                break;
                            default:
                                w = 0;
                                break;
                        }
                        edgesList.Add(new Edge { V1 = i, V2 = neighborIndex, weight = w });
                    }
                }
            }

            // Sort edges by ascending weight for Kruskal-like merging
            Edge[] edges = edgesList.ToArray();
            Array.Sort(edges, (a, b) => a.weight.CompareTo(b.weight));
            return edges;
        }

        // Merge components based on sorted edges and region predicate
        private DisjointSet mergeComponents(DisjointSet comp, Edge[] edges)
        {
            double MInt = 0;

            foreach (var edge in edges)
            {
                int root1 = comp.Find(edge.V1);
                int root2 = comp.Find(edge.V2);

                if (root1 == root2) continue;  // Already in same component

                // Calculate minimum internal difference threshold
                double v1_InternalDiff = comp.InternalDifference[root1] + (k / comp.size[root1]);
                double v2_InternalDiff = comp.InternalDifference[root2] + (k / comp.size[root2]);
                MInt = Math.Min(v1_InternalDiff, v2_InternalDiff);

                // Merge if edge weight is below adaptive threshold
                if (edge.weight < MInt)
                {
                    comp.Union(edge.V1, edge.V2, edge.weight);
                }
            }
            return comp;
        }

        // Combine results from three color channels through intersection
        private DisjointSet buildRegions(DisjointSet RedComp, DisjointSet GreenComp, DisjointSet BlueComp)
        {
            DisjointSet regions = new DisjointSet(v);
            // Track unique combinations of color component memberships
            Dictionary<(int, int, int), int> componentMap = new Dictionary<(int, int, int), int>();

            // Precompute roots for better cache performance
            int[] redRoots = new int[v];
            int[] greenRoots = new int[v];
            int[] blueRoots = new int[v];
            for (int i = 0; i < v; i++)
            {
                redRoots[i] = RedComp.Find(i);
                greenRoots[i] = GreenComp.Find(i);
                blueRoots[i] = BlueComp.Find(i);
            }

            for (int i = 0; i < v; i++)
            {
                // Create composite key from three color components
                var key = (redRoots[i], greenRoots[i], blueRoots[i]);

                if (componentMap.TryGetValue(key, out int existingRoot))
                {
                    // Merge with existing region and update root
                    regions.Union(existingRoot, i, 0);
                    componentMap[key] = regions.Find(existingRoot);
                }
                else
                {
                    // Register new unique component combination
                    componentMap[key] = regions.Find(i);
                }
            }
            return regions;
        }

        // Generate color-coded visualization of regions
        private RGBPixel[,] visualizeRegions(DisjointSet regions)
        {
            RGBPixel[,] output = new RGBPixel[height, width];
            Dictionary<int, RGBPixel> regionColors = new Dictionary<int, RGBPixel>();
            Random rand = new Random();

            for (int i = 0; i < v; i++)
            {
                int root = regions.Find(i);
                if (!regionColors.ContainsKey(root))
                {
                    // Assign random color to new region
                    regionColors[root] = new RGBPixel()
                    {
                        red = (byte)rand.Next(256),
                        green = (byte)rand.Next(256),
                        blue = (byte)rand.Next(256)
                    };
                }

                // Convert linear index to 2D coordinates
                int x = i / width;
                int y = i % width;
                output[x, y] = regionColors[root];
            }

            return output;
        }
    }
}