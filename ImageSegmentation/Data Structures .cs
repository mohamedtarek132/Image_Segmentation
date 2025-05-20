using System.Collections.Generic;
using System;

namespace ImageTemplate
{
    public struct Edge
    {
        public int V1, V2;
        public byte weight;
    }

    
    public enum Color
    {
        Red,
        Green,
        Blue
    }

    

    public class DisjointSet
    {
        // Tracks parent-child relationships in the disjoint set
        // parent[i] = index of the parent node of element i
        public int[] parent;

        // Tracks component sizes for union-by-size optimization
        // size[i] = number of elements in the tree rooted at i
        public int[] size;

        // Stores maximum internal difference for segmentation algorithms
        // InternalDifference[i] = maximum edge weight within component rooted at i
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
                InternalDifference[i] = 0;  // No internal difference in single-element components
            }
        }

        // Finds root of x's component with path compression optimization
        public int Find(int x)
        {
            int[] parentLocal = parent; // Local reference for faster access

            // First pass: find root ancestor
            int root = x;
            while (parentLocal[root] != root)
            {
                root = parentLocal[root];
            }

            // Second pass: path compression
            while (parentLocal[x] != root)
            {
                int next = parentLocal[x];  // Store original parent
                parentLocal[x] = root;      // Point directly to root
                x = next;                   // Move up the tree
            }
            return root;
        }

        // Merges two components using union-by-size and tracks maximum internal difference
        public void Union(int x, int y, byte weight = 0)
        {
            // Local references for faster access in tight loop
            int[] parentLocal = parent; 
            int[] sizeLocal = size;
            byte[] internalDiffLocal = InternalDifference;

            int xRoot = Find(x);
            int yRoot = Find(y);

            if (xRoot == yRoot) return;

            // Maintain invariant: xRoot is root of larger component
            if (sizeLocal[xRoot] < sizeLocal[yRoot])
            {
                (xRoot, yRoot) = (yRoot, xRoot);
            }

            // Merge smaller component into larger
            parentLocal[yRoot] = xRoot;
            sizeLocal[xRoot] += sizeLocal[yRoot];

            // InternalDiff(new) = max(InternalDiff(A), InternalDiff(B), weight)
            internalDiffLocal[xRoot] = Math.Max(internalDiffLocal[xRoot],
                Math.Max(weight, internalDiffLocal[yRoot]));
        }


    }
}