using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Mindworks.Tests
{
    [TestClass]
    public class GraphTests
    {
        internal bool HasPath(List<List<int>> all, params int[] path)
        {
            foreach (var p in all)
            {
                if (p.Count == path.Length)
                {
                    int i = 0;
                    var matches = true;
                    foreach (var s in p)
                    {
                        matches &= (s == path[i++]);
                    }
                    if (matches)
                        return true;
                }
            }
            return false;
        }

        [TestMethod]
        public void EmptyGraphTest()
        {
            // No paths in graph
            var graph = new List<Tuple<int, int>>();
            var paths = Program.ConnectingPaths(graph, 1, 4);
            Assert.IsTrue(paths.Count == 0, "No paths in empty graph");
        }
        [TestMethod]
        public void SimpleGraphTest()
        {
            // Simple graph
            var graph = new List<Tuple<int, int>>();
            graph.Add(new Tuple<int, int>(1, 2));
            var paths = Program.ConnectingPaths(graph, 1, 1);
            Assert.IsTrue(HasPath(paths, 1), "Check start=end");
        }
        [TestMethod]
        public void NoPathTest()
        {
            var graph = new List<Tuple<int, int>>();
            graph.Add(new Tuple<int, int>(1, 2));
            // Add more paths to graph
            graph.Add(new Tuple<int, int>(2, 3));
            graph.Add(new Tuple<int, int>(3, 4));
            graph.Add(new Tuple<int, int>(5, 6));
            var paths = Program.ConnectingPaths(graph, 1, 5);
            Assert.IsTrue(paths.Count == 0, "No paths in this graph from 1 to 5");
        }

        [TestMethod]
        public void CircleGraphTest()
        {
            var graph = new List<Tuple<int, int>>();
            graph.Add(new Tuple<int, int>(1, 2));
            graph.Add(new Tuple<int, int>(2, 3));
            graph.Add(new Tuple<int, int>(3, 1));
            graph.Add(new Tuple<int, int>(3, 4));
            var paths = Program.ConnectingPaths(graph, 1, 4);
            Assert.IsTrue(paths.Count == 1, $"Should find 1 path, but found {paths.Count}.");
            Assert.IsTrue(HasPath(paths, 1, 2, 3, 4), "Did not find 1,2,3,4");
            paths = Program.ConnectingPaths(graph, 1, 1);
            Assert.IsTrue(paths.Count == 1, $"Should find 1 path, but found {paths.Count}.");
            Assert.IsTrue(HasPath(paths, 1), "Check start=end ");
            Assert.IsTrue(!HasPath(paths, 1, 2, 3, 1), "Check start=end in circle");
        }

        [TestMethod]
        public void MiniGraphTest()
        {
            var graph = new List<Tuple<int, int>>();
            graph.Add(new Tuple<int, int>(1, 2));
            graph.Add(new Tuple<int, int>(1, 3));
            graph.Add(new Tuple<int, int>(1, 4));
            graph.Add(new Tuple<int, int>(2, 4));
            graph.Add(new Tuple<int, int>(3, 1));
            graph.Add(new Tuple<int, int>(3, 2));
            var paths = Program.ConnectingPaths(graph, 3, 4);
            Assert.IsTrue(paths.Count == 3, $"Should find 3 paths, but found {paths.Count}.");
            Assert.IsTrue(HasPath(paths, 3, 2, 4), "Did not find 3, 2, 4");
            Assert.IsTrue(HasPath(paths, 3, 1, 4), "Did not find 3, 1, 4");
            Assert.IsTrue(HasPath(paths, 3, 1,2, 4), "Did not find 3, 1, 2, 4");
        }


        [TestMethod]
        public void MindworksTest()
        {
            var graph = new List<Tuple<int, int>>();
            graph.Add(new Tuple<int, int>(1, 2));
            graph.Add(new Tuple<int, int>(1, 3));
            graph.Add(new Tuple<int, int>(2, 4));
            graph.Add(new Tuple<int, int>(3, 4));
            graph.Add(new Tuple<int, int>(4, 5));
            graph.Add(new Tuple<int, int>(4, 6));
            var paths = Program.ConnectingPaths(graph, 1, 4);
            Assert.IsTrue(paths.Count == 2, $"Should find 2 paths, but found {paths.Count}.");
            Assert.IsTrue(HasPath(paths, 1, 2, 4), "Did not find 1,2,4");
            Assert.IsTrue(HasPath(paths, 1, 3, 4), "Did not find 1,3,4");

            paths = Program.ConnectingPaths(graph, 1, 5);
            Assert.IsTrue(paths.Count == 2, $"Should find 2 paths, but found {paths.Count}.");
            Assert.IsTrue(HasPath(paths, 1, 2, 4, 5), "Did not find 1,2,4,5");
            Assert.IsTrue(HasPath(paths, 1, 3, 4, 5), "Did not find 1,3,4,5");
        }

        [TestMethod]
        public void AnotherTest()
        {
            var graph = new List<Tuple<int, int>>();
            graph.Add(new Tuple<int, int>(1, 2));
            graph.Add(new Tuple<int, int>(2, 3));
            graph.Add(new Tuple<int, int>(2, 4));
            graph.Add(new Tuple<int, int>(1, 3));
            graph.Add(new Tuple<int, int>(3, 1));
            graph.Add(new Tuple<int, int>(3, 4));
            graph.Add(new Tuple<int, int>(4, 4));
            var paths = Program.ConnectingPaths(graph, 3, 4);
            Assert.IsTrue(paths.Count == 2, $"Should find 2 paths, but found {paths.Count}.");
            Assert.IsTrue(HasPath(paths, 3, 4), "Did not find 3,4");
            Assert.IsTrue(HasPath(paths, 3, 1, 2, 4), "Did not find 3, 1, 2, 4");
        }

    }
}
