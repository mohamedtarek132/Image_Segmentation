using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Security.Permissions;
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
        class Heap<K, V> // PriorityQueue
        {
            public K[] keys;
            public V[] values;
            public int count = 0;
            private Func<K, K, bool> predicate;

            // (a, b) => a < b min at top (minHeap) // (a, b) => a > b max at top (maxHeap)
            public Heap(int size, Func<K, K, bool> comparisonBase)
            {
                keys = new K[size + 1];
                values = new V[size + 1];
                predicate = comparisonBase; // assume happy path
            }

            private void HeapifyUp(int curIdx)
            {
                if (curIdx < 2)
                    return;

                if (predicate(keys[curIdx], keys[(curIdx / 2)]))
                {
                    K temp = keys[curIdx];
                    V temp2 = values[curIdx];
                    keys[curIdx] = keys[(curIdx / 2)];
                    values[curIdx] = values[(curIdx / 2)];
                    keys[(curIdx / 2)] = temp;
                    values[(curIdx / 2)] = temp2;
                    HeapifyUp(curIdx / 2);
                }
            }
            public void insert(K key, V value)
            {
                count++;

                if (count == keys.Length)
                    Expand();

                keys[count] = key;
                values[count] = value;
                HeapifyUp(count);
            }
            private void Expand()
            {
                var new_keys_arr = new K[keys.Length << 1];
                var new_values_arr = new V[values.Length << 1];
                for (int i = 0; i < keys.Length; i++)
                {
                    new_keys_arr[i] = keys[i];
                    new_values_arr[i] = values[i];

                }
                keys = new_keys_arr;
                values = new_values_arr;
            }

            private void HeapifyDown(int curIdx)
            {
                if ((curIdx * 2) > count)
                    return;

                if ((curIdx * 2) + 1 > count)
                {
                    if (predicate(keys[(curIdx * 2)], keys[curIdx]))
                    {
                        K temp = keys[curIdx];
                        V temp2 = values[curIdx];
                        keys[curIdx] = keys[(curIdx * 2)];
                        values[curIdx] = values[(curIdx * 2)];
                        keys[(curIdx * 2)] = temp;
                        values[(curIdx * 2)] = temp2;
                        // HeapifyDown(curIdx * 2); // index > count
                    }
                }
                else
                {
                    if (predicate(keys[(curIdx * 2)], keys[(curIdx * 2) + 1]) &&
                        predicate(keys[(curIdx * 2)], keys[curIdx]))
                    {
                        K temp = keys[curIdx];
                        V temp2 = values[curIdx];
                        keys[curIdx] = keys[(curIdx * 2)];
                        values[curIdx] = values[(curIdx * 2)];
                        keys[(curIdx * 2)] = temp;
                        values[(curIdx * 2)] = temp2;
                        HeapifyDown(curIdx * 2);
                    }
                    else if (predicate(keys[(curIdx * 2) + 1], keys[curIdx]))
                    {
                        K temp = keys[curIdx];
                        V temp2 = values[curIdx];
                        keys[curIdx] = keys[(curIdx * 2) + 1];
                        values[curIdx] = values[(curIdx * 2) + 1];
                        keys[(curIdx * 2) + 1] = temp;
                        values[(curIdx * 2) + 1] = temp2;
                        HeapifyDown((curIdx * 2) + 1);
                    }
                }
            }
            public void pop(out K k, out V v)
            {
                k = keys[1];
                v = values[1];
                keys[1] = keys[count];
                values[1] = values[count];
                // arr[count] = temp;
                count--;

                HeapifyDown(1);
            }
            public void top(out K k, out V v)
            {
                k = keys[1];
                v = values[1];
            }
        }
        public class Component
        {
            public int height;
            public int width;
            public int maxInternal;
            public int connectedNodesCount;
            public Dictionary<(int, int), int> neighborsComponents;
        }
        public struct Components
        {
            public Component[] component;
            public Components(int i, int j)
            {
                component = new Component[3] {
                        new Component{height = i, width = j, maxInternal = 0, connectedNodesCount = 1, neighborsComponents = new Dictionary<(int, int), int>()},
                        new Component{height = i, width = j, maxInternal = 0, connectedNodesCount = 1, neighborsComponents = new Dictionary<(int, int), int>()},
                        new Component{height = i, width = j, maxInternal = 0, connectedNodesCount = 1, neighborsComponents = new Dictionary<(int, int), int>()},
                    };
            }
        }
        enum Color
        {
            Red,
            Green, 
            Blue
        }
        Components[,] imageComponents; // DSU - UF
        private (int x, int y) rootParent(int x, int y, Color color)
        {
            Stack<(int, int)> stack = new Stack<(int, int)>(); 
            int colorIdx = (int)color;
            while (imageComponents[x, y].component[colorIdx].width != y &&
                imageComponents[x, y].component[colorIdx].height != x)
            {
                stack.Push((x, y));
                int new_x = imageComponents[x, y].component[colorIdx].height;
                int new_y = imageComponents[x, y].component[colorIdx].width;
                x = new_x; 
                y = new_y;
            }
            while(stack.Count > 0)
            {
                var temp = stack.Pop();
                imageComponents[x, y].component[colorIdx].height = temp.Item1;
                imageComponents[x, y].component[colorIdx].width = temp.Item2;
            }
            return (x, y);
        }
        private void MergeIfCanMerge(int x1, int y1, int x2, int y2, int weight, Color color)
        {
            decimal k = nudMaskSize.Value;
            int colorIdx = (int)color;
            var MinInternal = imageComponents[x1, y1].component[colorIdx].maxInternal + k / imageComponents[x1, y1].component[colorIdx].connectedNodesCount;
            MinInternal = Math.Min(MinInternal,
                imageComponents[x2, y2].component[colorIdx].maxInternal + k / imageComponents[x2, y2].component[colorIdx].connectedNodesCount);

            if (weight > MinInternal) // they should be seperated
            {
                if(!imageComponents[x1, y1].component[colorIdx].neighborsComponents.ContainsKey((x2,  y2)))
                {
                    imageComponents[x1, y1].component[colorIdx].neighborsComponents.Add((x2, y2), weight);
                    imageComponents[x2, y2].component[colorIdx].neighborsComponents.Add((x1, y1), weight);
                }
            }
            else // merge
            {
                if(imageComponents[x2, y2].component[colorIdx].connectedNodesCount < imageComponents[x1, y1].component[colorIdx].connectedNodesCount)
                {
                    imageComponents[x2, y2].component[colorIdx].width = imageComponents[x1, y1].component[colorIdx].width;
                    imageComponents[x2, y2].component[colorIdx].height = imageComponents[x1, y1].component[colorIdx].height;

                    imageComponents[x1, y1].component[colorIdx].maxInternal = Math.Max(
                        imageComponents[x1, y1].component[colorIdx].maxInternal,
                        Math.Max(weight,
                        imageComponents[x2, y2].component[colorIdx].maxInternal)
                        );
                    imageComponents[x1, y1].component[colorIdx].connectedNodesCount += imageComponents[x2, y2].component[colorIdx].connectedNodesCount;
                    foreach(var v in imageComponents[x2, y2].component[colorIdx].neighborsComponents)
                    {
                        if (imageComponents[x1, y1].component[colorIdx].neighborsComponents.ContainsKey(v.Key))
                            imageComponents[x1, y1].component[colorIdx].neighborsComponents[v.Key] =
                                Math.Min(imageComponents[x1, y1].component[colorIdx].neighborsComponents[v.Key],
                                v.Value);
                        else
                            imageComponents[x1, y1].component[colorIdx].neighborsComponents.Add(v.Key, v.Value);
                    }
                }
            }
        }
        Heap<int, (int, int, int)> heap; // = new Heap<int, (int, int)>(8,);
        private void getNeighborPosition(int x, int y, int neighborPosition, ref int x_neighbor, ref int y_neighbor)
        {
            //0 1 2
            //3   4
            //5 6 7
            switch (neighborPosition)
            {
                case 0:
                    x_neighbor = x - 1;
                    y_neighbor = y - 1;
                    break;
                case 1:
                    x_neighbor = x - 1;
                    y_neighbor = y;
                    break;
                case 2:
                    x_neighbor = x - 1;
                    y_neighbor = y + 1;
                    break;
                case 3:
                    x_neighbor = x;
                    y_neighbor = y - 1;
                    break;
                case 4:
                    x_neighbor = x;
                    y_neighbor = y + 1;
                    break;
                case 5:
                    x_neighbor = x + 1;
                    y_neighbor = y - 1;
                    break;
                case 6:
                    x_neighbor = x + 1;
                    y_neighbor = y;
                    break;
                case 7:
                    x_neighbor = x + 1;
                    y_neighbor = y + 1;
                    break;
            }
        }
        private void buildComponent(Color color)
        {
            while(heap.count > 0)
            {
                heap.pop(out int weight, out (int, int, int) p);
                (int x, int y, int neighborPosition) = p; //0 1 2  NW N NE
                                                            //3   4  W    E
                                                            //5 6 7  SW S SE

                // decide either to merge the two components or not to merge them
                int x_neighbor = 0;
                int y_neighbor = 0;
                getNeighborPosition(x, y, neighborPosition, ref x_neighbor, ref y_neighbor);
                (x, y) = rootParent(x, y, color);
                (x_neighbor, y_neighbor) = rootParent(x_neighbor, y_neighbor, color);
                MergeIfCanMerge(x, y, x_neighbor, y_neighbor, weight, color);
            }
        }
        private void buildComponents(RGBWeight[,] graph) 
        {
            int rows = ImageMatrix.GetLength(0); // height
            int columns = ImageMatrix.GetLength(1); // width
            int V = rows * columns;
            imageComponents = new Components[rows, columns];
            for(int i = 0; i < rows; i++)
            {
                for(int j = 0; j < columns; j++)
                {
                    imageComponents[i, j] = new Components(i, j);
                }
            }

            heap = new Heap<int, (int, int, int)>(V * 4 + 5, (a, b) => a < b);

            for(int i = 0; i < V; i++)
            {
                int row = i / columns;
                int col = i % columns;
                if (col > 0 && row + 1 < rows)
                    heap.insert(graph[i, 5].red, (row, col, 5));
                if(col + 1 < columns)
                    heap.insert(graph[i, 4].red, (row, col, 4));
                if(row + 1 < rows)
                    heap.insert(graph[i, 6].red, (row, col, 6));
                if(col + 1 < columns && row + 1 < rows)
                    heap.insert(graph[i, 7].red, (row, col, 7));
                // save memory and time as this ensures the edges aren't repeated at the heap
            }
            buildComponent(Color.Red);

            for (int i = 0; i < V; i++)
            {
                int row = i / columns;
                int col = i % columns;
                if (col > 0 && row + 1 < rows)
                    heap.insert(graph[i, 5].green, (row, col, 5));
                if (col + 1 < columns)
                    heap.insert(graph[i, 4].green, (row, col, 4));
                if (row + 1 < rows)
                    heap.insert(graph[i, 6].green, (row, col, 6));
                if (col + 1 < columns && row + 1 < rows)
                    heap.insert(graph[i, 7].green, (row, col, 7));
                // save memory and time as this ensures the edges aren't repeated at the heap
            }
            buildComponent(Color.Green);

            for (int i = 0; i < V; i++)
            {
                int row = i / columns;
                int col = i % columns;
                if (col > 0 && row + 1 < rows)
                    heap.insert(graph[i, 5].blue, (row, col, 5));
                if (col + 1 < columns)
                    heap.insert(graph[i, 4].blue, (row, col, 4));
                if (row + 1 < rows)
                    heap.insert(graph[i, 6].blue, (row, col, 6));
                if (col + 1 < columns && row + 1 < rows)
                    heap.insert(graph[i, 7].blue, (row, col, 7));
                // save memory and time as this ensures the edges aren't repeated at the heap
            }
            buildComponent(Color.Blue);

            var redRegionsRoots = new HashSet<(int, int)>();
            var greenRegionsRoots = new HashSet<(int, int)>();
            var blueRegionsRoots = new HashSet<(int, int)>();

            for(int i = 0; i < rows; i++)
            {
                for(int j = 0; j < columns; j++)
                {
                    var components = imageComponents[i, j];
                    
                    var row = components.component[0].height;
                    var col = components.component[0].width;
                    redRegionsRoots.Add(rootParent(row, col, Color.Red));

                    row = components.component[1].height;
                    col = components.component[1].width;
                    greenRegionsRoots.Add(rootParent(row, col, Color.Green));

                    row = components.component[2].height;
                    col = components.component[2].width;
                    blueRegionsRoots.Add(rootParent(row, col, Color.Blue));
                }
            }

            int redRegionsCount = redRegionsRoots.Count;
            int greenRegionsCount = greenRegionsRoots.Count;
            int blueRegionsCount = blueRegionsRoots.Count;
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

        private void nudMaskSize_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}