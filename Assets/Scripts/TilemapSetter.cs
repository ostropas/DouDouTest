using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapSetter : MonoBehaviour
{
    public Camera Camera;

    public Tilemap TileMap;
    public Tile RoadTile;
    public Tile GrassTile;


    public Vector2Int Size;
    public Grid TilesGrid;


    public void Awake()
    {
        var map = MapGenerator.GenerateRandomMap(Size.x, Size.y);

        var center = new Vector3Int(Size.x / 2, Size.y / 2, 0);

        var pos = TilesGrid.CellToWorld(center);
        pos.z = -10f;
        Camera.transform.position = pos;

        Camera.orthographicSize = Mathf.Sqrt(Size.x * Size.y);


        for (int x = 0; x < map.Count; x++)
        {
            for (int y = 0; y < map[x].Count; y++)
            {
                if (map[x][y] == 1)
                {
                    TileMap.SetTile(new Vector3Int(x, y, 0), RoadTile);
                } else
                {
                    TileMap.SetTile(new Vector3Int(x, y, 0), GrassTile);
                }
            }
        }
    }
}
