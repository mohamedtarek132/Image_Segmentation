using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

namespace ImageTemplate
{
    public class seg
    {
        int height, width;
        double k;  // Threshold function parameter controlling region merging
        int v;     // Total number of pixels (height * width)
        RGBPixel[,] ImageMatrix;
        DisjointSet finalRegions;
        public seg(int h, int w, int k, RGBPixel[,] imageMatrix)
        {
            this.k = k;
            this.height = h;
            this.width = w;
            v = h * w;  // Calculate total number of vertices
            ImageMatrix = imageMatrix;
        }

        // Main segmentation entry point
        public (RGBPixel[,], int, int[]) segmentImage()
        {

            // Initialize disjoint sets for each color channel
            DisjointSet RedComponents = new DisjointSet(v);
            DisjointSet GreenComponents = new DisjointSet(v);
            DisjointSet BlueComponents = new DisjointSet(v);

            Edge[] Edges = null;

            Parallel.Invoke(
                () => Edges = ProcessColorChannel(Color.Red, ref RedComponents),
                () => ProcessColorChannel(Color.Green, ref GreenComponents),
                () => ProcessColorChannel(Color.Blue, ref BlueComponents)
            );

            // Combine results from all three channels using intersection
            finalRegions = buildRegions(RedComponents, GreenComponents, BlueComponents, Edges);

            // Generate color-coded visualization of the regions
            RGBPixel[,] segmentedImage = visualizeRegions(finalRegions);

            // Diagnostic output for region counts
            //HashSet<int> red = new HashSet<int>();
            //HashSet<int> green = new HashSet<int>();
            //HashSet<int> blue = new HashSet<int>();
            HashSet<int> finalRegionSet = new HashSet<int>();
            Dictionary<int, int> pixelPerRegionCounter = new Dictionary<int, int>();

            for (int i = 0; i < v; i++)
            {
                //red.Add(RedComponents.Find(i));
                //green.Add(GreenComponents.Find(i));
                //blue.Add(BlueComponents.Find(i));
                int parent = finalRegions.Find(i);
                finalRegionSet.Add(parent);
                if (pixelPerRegionCounter.ContainsKey(parent))
                {
                    pixelPerRegionCounter[parent] += 1;
                }
                else
                {
                    pixelPerRegionCounter.Add(parent, 1);
                }
            }

            //Console.WriteLine(red.Count);
            //Console.WriteLine(green.Count);
            //Console.WriteLine(blue.Count);
            //Console.WriteLine(finalRegionSet.Count);
            int[] pixelPerRegionCount = pixelPerRegionCounter.Values.ToArray();
            Array.Sort(pixelPerRegionCount);
            Array.Reverse(pixelPerRegionCount);

            return (segmentedImage, finalRegionSet.Count, pixelPerRegionCount);
        }

        private Edge[] ProcessColorChannel(Color channel, ref DisjointSet components)
        {
            byte[,] channelGraph = GraphConstruction.build_graph(channel, ImageMatrix);
            Edge[] edges = buildEdgeArray(channelGraph);
            components = mergeComponents(components, edges);
            if (channel == Color.Red)
            {
                return edges;
            }
            return null;
        }

        // Construct sorted edge list for a specific color channel
        private Edge[] buildEdgeArray(byte[,] graph)
        {
            
            List<Edge> edgesList = new List<Edge>();

            // Iterate through all pixels
            for (int i = 0; i < v; i++)    //O(V)
            {
                (int, int, byte)[] directions = {
                                         (0, 1, 0),
                (1, -1, 1),  (1, 0, 2),  (1, 1, 3)
                };
                int x = i / width;
                int y = i % width;

                foreach (var dir in directions)    //O(1)
                {
                    int nx = x + dir.Item1;
                    int ny = y + dir.Item2;
                    if (nx >= 0 && nx < height && ny >= 0 && ny < width)
                    {
                        int neighborIndex = nx * width + ny;

                        edgesList.Add(new Edge { V1 = i, V2 = neighborIndex, weight = graph[i, dir.Item3] });   //O(1)
                    }

                }

            }

            // Sort edges by ascending weight for Kruskal-like merging
            Edge[] edges = edgesList.ToArray();     //O(V)
            Helper_func.coutingSort(edges);    //O(V)

            return edges;
        }

        // Merge components based on sorted edges and region predicate
        private DisjointSet mergeComponents(DisjointSet comp, Edge[] edges)    //O(V)
        {
            double MInt = 0;

            foreach (var edge in edges)     //O(V)
            {
                int root1 = comp.Find(edge.V1); //O(1)?????????????????
                int root2 = comp.Find(edge.V2);

                if (root1 == root2) continue;  // Already in same component

                // Calculate minimum internal difference threshold
                double v1_InternalDiff = comp.InternalDifference[root1] + (k / comp.size[root1]);
                double v2_InternalDiff = comp.InternalDifference[root2] + (k / comp.size[root2]);
                MInt = Math.Min(v1_InternalDiff, v2_InternalDiff);

                // Merge if edge weight is below adaptive threshold
                if (edge.weight < MInt)
                {
                    comp.Union(root1, root2, edge.weight);  //O(1)
                }
            }
            return comp;
        }

        // Combine results from three color channels through intersection
        private DisjointSet buildRegions(DisjointSet RedComp, DisjointSet GreenComp, DisjointSet BlueComp, Edge[] edges)    //O(V)
        {
            DisjointSet regions = new DisjointSet(v);

            foreach (var edge in edges)     //O(V)
            {
                int root1 = regions.Find(edge.V1);
                int root2 = regions.Find(edge.V2);

                if (root1 == root2) continue;  // Already in same component

                int v1 = edge.V1;
                int v2 = edge.V2;
                var key1 = (RedComp.Find(v1), GreenComp.Find(v1), BlueComp.Find(v1));
                var key2 = (RedComp.Find(v2), GreenComp.Find(v2), BlueComp.Find(v2));

                // Merge if edge weight is below adaptive threshold
                if (key1.Item1 == key2.Item1 && key1.Item2 == key2.Item2 && key1.Item3 == key2.Item3)
                {
                    regions.Union(root1, root2);    //O(1)
                }
            }


            return regions;
        }

        // Generate color-coded visualization of regions
        private RGBPixel[,] visualizeRegions(DisjointSet regions)   //O(V)
        {
            RGBPixel[,] output = new RGBPixel[height, width];   //O(V)
            Dictionary<int, RGBPixel> regionColors = new Dictionary<int, RGBPixel>();
            Random rand = new Random();

            for (int i = 0; i < v; i++) //O(V)
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

        public RGBPixel[,] MergeRegions(List<Point> points)
        {
            RGBPixel[,] output = new RGBPixel[height, width];
            int index1, index2;
            int root;
            int xCoordinate, yCoordinate;
            DisjointSet regions = finalRegions;

            for (int i = 0; i < points.Count - 1; i++) 
            {
                index1 = points[i].y * width + points[i].x;
                index2 = points[i + 1].y * width + points[i + 1].x;

                regions.Union(index1,index2);
            }

            int index = index1 = points[0].y * width + points[0].x;
            int regionRoot = regions.Find(index);

            for (int i = 0; i < v; i++)
            {
                root = regions.Find(i);
                xCoordinate = i / width;
                yCoordinate = i % width;

                if (root == regionRoot)
                {
                    output[xCoordinate, yCoordinate] = new RGBPixel()
                    {
                        red = ImageMatrix[xCoordinate, yCoordinate].red,
                        green = ImageMatrix[xCoordinate, yCoordinate].green,
                        blue = ImageMatrix[xCoordinate, yCoordinate].blue
                    };
                }
                else
                {
                    output[xCoordinate, yCoordinate] = new RGBPixel()
                    {
                        red = 255,
                        green = 255,
                        blue = 255,
                    };
                }
            }

            return output;
            
        }
    }
}