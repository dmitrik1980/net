using System;
using System.Collections.Generic;

namespace Mindworks
{
    public static class Program
    {
        static void Main(string[] args)
        {
            string input;
            Tuple<int, int> edge;
            var graph = new List<Tuple<int, int>>();
            List<List<int>> paths;

            Console.WriteLine("Initialize graph:\n Enter edges as integers 'from' and 'to' delimited by whitespace and press enter.\n Enter empty line once finished.");
            while ((input = Console.ReadLine()) != "")
            {
                edge = ParseTuple(input);
                if (null != edge)
                {
                    graph.Add(edge);
                    Console.WriteLine($"Added edge '{edge.Item1} -> {edge.Item2}'.");
                }
                else
                {
                    Console.WriteLine($"Cannot parse input '{input}', line ignored.");
                }
            }
            Console.WriteLine("Enter 'start' and 'destination'.");
            do
            {
                input = Console.ReadLine();
                edge = ParseTuple(input);
                if (null != edge)
                {
                    Console.WriteLine($"Searching for paths from '{edge.Item1}' to '{edge.Item2}'.");
                }
                else
                {
                    Console.WriteLine($"Cannot parse input '{input}', line ignored.");
                }
            } while (null == edge);
            // Call the method.
            paths = ConnectingPaths(graph, edge.Item1, edge.Item2);
            // Display paths
            Console.WriteLine(
                0 == paths.Count
                ? "No paths found"
                : $"Found {paths.Count} paths:");
            foreach (var p in paths)
            {
                Console.WriteLine("Path: " + string.Join(" -> ", p));
            }
            Console.ReadLine();
        }

        private static Tuple<int, int> ParseTuple(string input)
        {
            var edge = input.Split(' ', ',');
            if (edge.Length == 2
                && int.TryParse(edge[0], out var @from)
                && int.TryParse(edge[1], out var to)
                )
            {
                return new Tuple<int, int>(from, to);
            }
            return null;
        }

        /// <summary></summary>
        /// <param name="graph">Graph as paths</param>
        /// <param name="node1">start point</param>
        /// <param name="node2">end point</param>
        /// <returns></returns>
        public static List<List<int>> ConnectingPaths(List<Tuple<int, int>> graph, int node1, int node2)
        {
            // Create graph
            var g = new Graph();
            graph.ForEach(p => g.AddEdge(p.Item1, p.Item2));
            return g.GetAllPaths(node1, node2);
        }

        private class Graph
        {
            /// <summary>Store as look-up per Vertice all possible neighbours</summary>
            private readonly IDictionary<int, HashSet<int>> _vertices = new Dictionary<int, HashSet<int>>();

            /// <summary>Add path from one vertice to another</summary>
            public void AddEdge(int from, int to)
            {
                HashSet<int> neighbours;
                if (!_vertices.TryGetValue(from, out neighbours))
                {
                    neighbours = new HashSet<int>();
                    _vertices.Add(from, neighbours);
                }
                if (!neighbours.Contains(to))
                {
                    neighbours.Add(to);
                }
//                else
//                {
//                    // Non-fatal error - path already defined before, do no add.
//                }
            }

            public List<List<int>> GetAllPaths(int from, int to)
            {
                var result = new List<List<int>>();
                GetAllPaths(from, to, new List<int>(new[] { from }), result);
                return result;
            }

            private void GetAllPaths(int from, int to, List<int> currentPath, List<List<int>> allPaths)
            {
                if (from == to)
                {
                    allPaths.Add(new List<int>(currentPath));
                    return; // Found
                }
                // See, if can go any further
                if (!_vertices.TryGetValue(from, out var neighbours))
                {
                    return; // Dead-end
                }
                // Ok, search for each, but verify, the neighbour has not yet been visited.
                foreach (var neigh in neighbours)
                {
                    // Only search, if neighbour not already in path
                    if (!currentPath.Contains(neigh))
                    {
                        currentPath.Add(neigh);
                        GetAllPaths(neigh, to, currentPath, allPaths);
                        currentPath.Remove(neigh);
                    }
                }
            }
        }
    }
}


