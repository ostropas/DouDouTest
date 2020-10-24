using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static SimpleTile;

public class TilemapSetter : MonoBehaviour
{
    public enum RuleArrow
    {
        Up,
        RightUp,
        Right,
        RightDown,
        Down,
        LeftDown,
        Left,
        LeftUp,
        Center
    }

    [Serializable]
    public class SettableTile
    {
        public SimpleTile Tile;
        public int RuleArrowMask;
    }

    public Camera Camera;

    public Tilemap TileMap;


    public Vector2Int Size;
    public Grid TilesGrid;

    [HideInInspector]
    public List<SettableTile> Tiles;

    private Dictionary<int, SettableTile> _tilesDict; 



    public void Awake()
    {
        _tilesDict = CalcTilesDictionary();

        var map = MapGenerator.GenerateRandomMap(Size.x, Size.y);

        var center = new Vector3Int(Size.x / 2, Size.y / 2, 0);

        var pos = TilesGrid.CellToWorld(center);
        pos.z = -10f;
        Camera.transform.position = pos;

        Camera.orthographicSize = Mathf.Sqrt(Size.x * Size.y);

        //Debug.Log(GetCurrentRuleMask(0, 0, map));
        //Debug.Log(Tiles[0].RuleArrowMask);

        var sb = new System.Text.StringBuilder();

        foreach (var item in map)
        {
            var line = string.Join(" ", item);
            sb.Append(line);
            sb.Append("\n");
        }

        Debug.Log(sb.ToString());


        for (int x = 0; x < map.Count; x++)
        {
            for (int y = 0; y < map[x].Count; y++)
            {
                var mask = GetCurrentRuleMask(x, y, map);

                if (_tilesDict.ContainsKey(mask))
                {
                    ApplyTilePrefab(_tilesDict[mask].Tile.GetTile(), new Vector3Int(x, y, 0), TileMap);
                }
                else
                {
                    ApplyTilePrefab(_tilesDict[0].Tile.GetTile(), new Vector3Int(x, y, 0), TileMap);
                }
            }
        }
    }

    private int GetCurrentRuleMask(int x, int y, List<List<int>> map)
    {
        var res = 0;

        if (map[x][y] == 0)
            return 0;

        // Up y - max
        // Down y - min
        // Left x - min
        // Right x - max

        if (map[x][1] == 1)
            res |= 1 << (int)RuleArrow.Center;

        if (y + 1 < map[x].Count && map[x][y + 1] == 1)
            res |= 1 << (int)RuleArrow.Up;

        if (x + 1 < map.Count && y + 1 < map[x].Count && map[x + 1][y + 1] == 1)
            res |= 1 << (int)RuleArrow.RightUp;

        if (x + 1 < map.Count && map[x + 1][y] == 1)
            res |= 1 << (int)RuleArrow.Right;

        if (x + 1 < map.Count && y > 0 && map[x + 1][y - 1] == 1)
            res |= 1 << (int)RuleArrow.RightDown;

        if (y > 0 && map[x][y - 1] == 1)
            res |= 1 << (int)RuleArrow.Down;

        if (x > 0 && y > 0 && map[x - 1][y - 1] == 1)
            res |= 1 << (int)RuleArrow.LeftDown;

        if (x > 0 && map[x - 1][y] == 1)
            res |= 1 << (int)RuleArrow.Left;

        if (x > 0 && y + 1 < map[x].Count && map[x - 1][y + 1] == 1)
            res |= 1 << (int)RuleArrow.LeftUp;

        var everything = 0;

        foreach (RuleArrow rule in (RuleArrow[])Enum.GetValues(typeof(RuleArrow)))
        {
            everything |= 1 << (int)rule;
        }

        if (res == everything)
            return -1;

        return res;
    }

    private void ApplyTilePrefab(TilePrefab tilePrefab, Vector3Int pos, Tilemap tilemap)
    {
        foreach (var preset in tilePrefab.Presets)
        {
            var adjPos = pos;
            adjPos.z += preset.ZPos;
            tilemap.SetTile(adjPos, preset.Tile);
        }
    }

    private Dictionary<int, SettableTile> CalcTilesDictionary()
    {
        var res = new Dictionary<int, SettableTile>();

        foreach (var item in Tiles)
        {
            res.Add(item.RuleArrowMask, item);
        }

        return res;
    }
}
