using RandomMapGenerator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public static class MapGenerator
    {
        /// <summary>
        /// Generate random map of ints, where 0 - grass, 1 - road
        /// </summary>
        /// <param name="width">Width of generated map</param>
        /// <param name="length">Length of generated map</param>
        /// <returns>Generated map</returns>
        public static List<List<int>> GenerateRandomMap(int width, int length)
        {
            var res = new List<List<int>>();
            var rnd = new System.Random();

            var g = new Graph();

            // Init map
            for (int x = 0; x < length; x++)
            {
                res.Add(new List<int>());
                for (int y = 0; y < width; y++)
                {
                    g.AddVertex(new Vector2Int(x, y));
                    res[x].Add(0);
                }
            }

            // Generate graph
            for (int x = 0; x < res.Count; x++)
            {
                for (int y = 0; y < res[x].Count; y++)
                {
                    if (x + 1 < res.Count)
                        g.AddEdge(new Vector2Int(x, y), new Vector2Int(x + 1, y), 1);

                    if (y + 1 < res[x].Count)
                        g.AddEdge(new Vector2Int(x, y), new Vector2Int(x, y + 1), 1);
                }
            }

            var randomPoints = new List<Vector2Int>();
            var tries = 0;

            // Generate random points on map
            while (randomPoints.Count < Math.Sqrt(width * length) / 2 && tries < 100)
            {
                var randomX = rnd.Next(0, length);
                var randomY = rnd.Next(0, width);

                if (randomPoints.Exists(x => x.x == randomX || x.y == randomY
                || x.x - x.y == randomX - randomY || x.x + x.y == randomX + randomY))
                {
                    tries++;
                    continue;
                }

                res[randomX][randomY] = 1;
                randomPoints.Add(new Vector2Int(randomX, randomY));
            }

            // If only 1 point, don't search path
            if (randomPoints.Count == 1)
                return res;

            // Search path
            var dijkstra = new Dijkstra(g);
            for (int i = 0; i < randomPoints.Count; i++)
            {
                var curPoint = randomPoints[i];
                var nextPoint = randomPoints[i + 1 >= randomPoints.Count ? 0 : i + 1];

                var path = dijkstra.FindShortestPath(curPoint, nextPoint);

                if (path == null)
                    continue;

                for (int j = 0; j < path.Count - 1; j++)
                {
                    g.RemoveEdge(path[j], path[j + 1]);

                }

                foreach (var item in path)
                {
                    res[item.x][item.y] = 1;
                }
            }

            return res;
        }
    }
}
