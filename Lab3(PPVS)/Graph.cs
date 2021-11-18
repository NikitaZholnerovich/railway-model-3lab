using System;
using System.Collections.Generic;
using System.Text;

namespace Lab3_PPVS_
{
    public class Graph
    {

        public List<Node> Nodes = new List<Node>();
        public List<Edge> Edges = new List<Edge>();
        public List<Train> Trains = new List<Train>();

        public int NodeCount => Nodes.Count;
        public int EdgeCount => Edges.Count;

        public void AddNodes(IEnumerable<Node> nodes)
        {
            Nodes.AddRange(nodes);
        }

        public void AddEdge(Node fromStation, Node toStation, int weight)
        {
            var edge = new Edge(fromStation, toStation, weight);
            Edges.Add(edge);
        }

        

    }
}
