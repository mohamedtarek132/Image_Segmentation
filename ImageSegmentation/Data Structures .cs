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
            int[] parentLocal = parent; // Cache to local variable
                                        // Find the root of x
            int root = x;
            while (parentLocal[root] != root)
            {
                root = parentLocal[root];
            }
            // Compress the path from x to root
            while (parentLocal[x] != root)
            {
                int next = parentLocal[x];
                parentLocal[x] = root;
                x = next;
            }
            return root;
        }

        // Merges two components using union-by-size optimization
        public void Union(int x, int y, byte weight = 0)
        {
            int[] parentLocal = parent; // Cache local references
            int[] sizeLocal = size;
            byte[] internalDiffLocal = InternalDifference;

            int xRoot = Find(x);
            int yRoot = Find(y);

            if (xRoot == yRoot) return;

            // Ensure xRoot is the larger tree
            if (sizeLocal[xRoot] < sizeLocal[yRoot])
            {
                (xRoot, yRoot) = (yRoot, xRoot);
            }

            parentLocal[yRoot] = xRoot;
            sizeLocal[xRoot] += sizeLocal[yRoot];
            internalDiffLocal[xRoot] = Math.Max(internalDiffLocal[xRoot],
                Math.Max(weight, internalDiffLocal[yRoot]));
        }


    }
}