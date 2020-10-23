using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RandomMapGenerator
{
    public class GraphEdge
    {
        public GraphVertex ConnectedVertex { get; }

        public int EdgeWeight { get; set; }

        public GraphEdge(GraphVertex connectedVertex, int weight)
        {
            ConnectedVertex = connectedVertex;
            EdgeWeight = weight;
        }
    }

    public class GraphVertex
    {
        public Vector2Int Value { get; }

        public List<GraphEdge> Edges { get; }

        public GraphVertex(Vector2Int value)
        {
            Value = value;
            Edges = new List<GraphEdge>();
        }

        public void AddEdge(GraphEdge newEdge)
        {
            Edges.Add(newEdge);
        }

        public void RemoveEdge(GraphVertex vertex)
        {
            Edges.RemoveAll(x => x.ConnectedVertex == vertex);
        }

        public void AddEdge(GraphVertex vertex, int edgeWeight)
        {
            AddEdge(new GraphEdge(vertex, edgeWeight));
        }
    }

    public class Graph
    {
        public List<GraphVertex> Vertices { get; }

        public Graph()
        {
            Vertices = new List<GraphVertex>();
        }

        public void AddVertex(Vector2Int value)
        {
            Vertices.Add(new GraphVertex(value));
        }

        public GraphVertex FindVertex(Vector2Int value)
        {
            foreach (var v in Vertices)
            {
                if (v.Value.Equals(value))
                {
                    return v;
                }
            }

            return null;
        }

        public void AddEdge(Vector2Int firstValue, Vector2Int secondValue, int weight)
        {
            var v1 = FindVertex(firstValue);
            var v2 = FindVertex(secondValue);
            if (v2 != null && v1 != null)
            {
                v1.AddEdge(v2, weight);
                v2.AddEdge(v1, weight);
            }
        }

        public void RemoveEdge(Vector2Int firstValue, Vector2Int secondValue)
        {
            var v1 = FindVertex(firstValue);
            var v2 = FindVertex(secondValue);
            if (v2 != null && v1 != null)
            {
                v1.RemoveEdge(v2);
                v2.RemoveEdge(v1);
            }
        }
    }

    public class GraphVertexInfo
    {
        public GraphVertex Vertex { get; set; }

        public bool IsUnvisited { get; set; }

        public int EdgesWeightSum { get; set; }

        public GraphVertex PreviousVertex { get; set; }

        public GraphVertexInfo(GraphVertex vertex)
        {
            Vertex = vertex;
            IsUnvisited = true;
            EdgesWeightSum = int.MaxValue;
            PreviousVertex = null;
        }
    }

    public class Dijkstra
    {
        Graph graph;

        List<GraphVertexInfo> infos;

        public Dijkstra(Graph graph)
        {
            this.graph = graph;
        }

        void InitInfo()
        {
            infos = new List<GraphVertexInfo>();
            foreach (var v in graph.Vertices)
            {
                infos.Add(new GraphVertexInfo(v));
            }
        }

        GraphVertexInfo GetVertexInfo(GraphVertex v)
        {
            foreach (var i in infos)
            {
                if (i.Vertex.Equals(v))
                {
                    return i;
                }
            }

            return null;
        }

        public GraphVertexInfo FindUnvisitedVertexWithMinSum()
        {
            var minValue = int.MaxValue;
            GraphVertexInfo minVertexInfo = null;
            foreach (var i in infos)
            {
                if (i.IsUnvisited && i.EdgesWeightSum < minValue)
                {
                    minVertexInfo = i;
                    minValue = i.EdgesWeightSum;
                }
            }

            return minVertexInfo;
        }

        public List<Vector2Int> FindShortestPath(Vector2Int startName, Vector2Int finishName)
        {
            return FindShortestPath(graph.FindVertex(startName), graph.FindVertex(finishName));
        }

        public List<Vector2Int> FindShortestPath(GraphVertex startVertex, GraphVertex finishVertex)
        {
            InitInfo();
            var first = GetVertexInfo(startVertex);
            first.EdgesWeightSum = 0;
            while (true)
            {
                var current = FindUnvisitedVertexWithMinSum();
                if (current == null)
                {
                    break;
                }

                SetSumToNextVertex(current);
            }

            return GetPath(startVertex, finishVertex);
        }

        void SetSumToNextVertex(GraphVertexInfo info)
        {
            info.IsUnvisited = false;
            foreach (var e in info.Vertex.Edges)
            {
                var nextInfo = GetVertexInfo(e.ConnectedVertex);
                var sum = info.EdgesWeightSum + e.EdgeWeight;
                if (sum < nextInfo.EdgesWeightSum)
                {
                    nextInfo.EdgesWeightSum = sum;
                    nextInfo.PreviousVertex = info.Vertex;
                }
            }
        }

        List<Vector2Int> GetPath(GraphVertex startVertex, GraphVertex endVertex)
        {
            var path = new List<Vector2Int>();
            path.Add(endVertex.Value);
            while (startVertex != endVertex)
            {
                endVertex = GetVertexInfo(endVertex).PreviousVertex;

                if (endVertex == null)
                    return null;

                path.Add(endVertex.Value);
            }

            return path;
        }
    }
}
