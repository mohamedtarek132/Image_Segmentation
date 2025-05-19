using System.Collections.Generic;
using System;

namespace ImageTemplate
{
    public struct RGBWeight
    {
        public byte red, green, blue;
    }

    public struct Edge
    {
        public int V1, V2;
        public byte weight;
    }

    
    enum Color
    {
        Red,
        Green,
        Blue
    }

    

    public class DisjointSet
    {
        // Stores parent relationships for each element
        // parent[i] = index of i's parent element
        public int[] parent;

        // Stores size of each component tree
        // size[i] = number of elements in tree rooted at i
        public int[] size;

        public byte[] InternalDifference;

        // Initializes a Disjoint Set Union (DSU) structure for n elements
        public DisjointSet(int n)
        {
            parent = new int[n];
            size = new int[n];
            InternalDifference = new byte[n];

            for (int i = 0; i < n; i++)
            {
                parent[i] = i;  // Each element starts as its own root
                size[i] = 1;    // Each component starts with size 1
                InternalDifference[i] = 0;
            }
        }

        // Finds root of x's component with path compression optimization
        public int Find(int x)
        {
            while (parent[x] != x)
            {
                // Path compression: make x's parent point to grandparent
                parent[x] = parent[parent[x]];  // Flattens tree structure
                x = parent[x];  // Move up the tree
            }
            
            return x;  // Root element of x's component
        }

        // Merges two components using union-by-size optimization
        public void Union(int x, int y,byte weight = 0)
        {
            int xRoot = Find(x);  // Root of x's component
            int yRoot = Find(y);  // Root of y's component

            if (xRoot == yRoot) return;  // Already in same component

            // Ensure xRoot is larger component (swap if necessary)
            if (size[xRoot] < size[yRoot])
            {
                // Swap roots to maintain size hierarchy
                (xRoot, yRoot) = (yRoot, xRoot);  // Tuple swap
            }

            // Attach smaller tree (yRoot) to larger tree (xRoot)
            parent[yRoot] = xRoot;         // Merge components
            size[xRoot] += size[yRoot];    // Update component size
            
            InternalDifference[xRoot] = Math.Max(InternalDifference[xRoot], Math.Max(weight, InternalDifference[yRoot]));
            
            

            // Note: size[yRoot] is no longer maintained after merge
        }

        
    }
}