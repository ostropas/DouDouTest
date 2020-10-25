using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.Tilemaps;
using static SimpleTile;

public class TilemapSetter : MonoBehaviour
{
    public Camera Camera;

    public Tilemap TileMap;


    public Vector2Int Size;
    public Grid TilesGrid;

    public List<SimpleTile> Tiles;

    public void Awake()
    {
        GenerateLevel();
    }

    public void GenerateLevel()
    {
        TileMap.ClearAllTiles();

        var map = MapGenerator.GenerateRandomMap(Size.x, Size.y);

        var center = new Vector3Int(Size.y / 2, Size.y / 2, 0);

        var pos = TilesGrid.CellToWorld(center);
        pos.z = -10f;
        Camera.transform.position = pos;

        Camera.orthographicSize = Mathf.Sqrt(Size.x * Size.y);


        for (int x = 0; x < map.Count; x++)
        {
            for (int y = 0; y < map[x].Count; y++)
            {
                var prefabs = GetCurrentTilePrefabs(x, y, map);

                for (int i = 0; i < prefabs.Count; i++)
                {
                    ApplyTilePrefab(prefabs[i].GetTile(), new Vector3Int(x, y, 0), TileMap, i);

                }
            }
        }
    }

    bool _checkUp;
    bool _checkUpRight;
    bool _checkRight;
    bool _checkRightDown;
    bool _checkDown;
    bool _checkLeftDown;
    bool _checkLeft;
    bool _checkLeftUp;
    bool _checkCenter;

    private List<SimpleTile> GetCurrentTilePrefabs(int x, int y, List<List<int>> map)
    {
        var res = new List<SimpleTile>();

        _checkUp = CheckUp(x, y, map);
        _checkUpRight = CheckUpRight(x, y, map);
        _checkRight = CheckRight(x, y, map);
        _checkRightDown = CheckRightDown(x, y, map);
        _checkDown = CheckDown(x, y, map);
        _checkLeftDown = CheckLeftDown(x, y, map);
        _checkLeft = CheckLeft(x, y, map);
        _checkLeftUp = CheckLeftUp(x, y, map);
        _checkCenter = CheckCenter(x, y, map);


        if (!_checkLeftUp && !_checkUp && !_checkUpRight &&
            !_checkLeft && _checkCenter && !_checkRight &&
            !_checkLeftDown && !_checkDown && !_checkRightDown)
        {
            res.Add(Tiles[0]);
            res.Add(Tiles[18]);
        }
        else
        {
            if (_checkCenter)
                res.Add(Tiles[0]);

            AddOneWay(res);
        }

        if (res.Count == 0)
        {
            res.Add(Tiles[1]);
        }

        return res;
    }

    private void AddOneWay(List<SimpleTile> res)
    {
        bool addTwoWay = true;

        if (!_checkLeftUp && !_checkUp && !_checkUpRight &&
            !_checkLeft && _checkCenter && !_checkRight &&
            !_checkLeftDown && _checkDown && !_checkRightDown)
        {
            res.Add(Tiles[17]);
            addTwoWay = false;
        }

        if (!_checkLeftUp && !_checkUp && !_checkUpRight &&
            _checkLeft && _checkCenter && !_checkRight &&
            !_checkLeftDown && !_checkDown && !_checkRightDown)
        {
            res.Add(Tiles[16]);
            addTwoWay = false;
        }

        if (!_checkLeftUp && _checkUp && !_checkUpRight &&
            !_checkLeft && _checkCenter && !_checkRight &&
            !_checkLeftDown && !_checkDown && !_checkRightDown)
        {
            res.Add(Tiles[15]);
            addTwoWay = false;
        }

        if (!_checkLeftUp && !_checkUp && !_checkUpRight &&
            !_checkLeft && _checkCenter && _checkRight &&
            !_checkLeftDown && !_checkDown && !_checkRightDown)
        {
            res.Add(Tiles[14]);
            addTwoWay = false;
        }

        if (addTwoWay)
            AddTwoWay(res);
    }

    private void AddTwoWay(List<SimpleTile> res)
    {
        bool addThreeWay = true;

        if (_checkCenter && !_checkLeft && !_checkLeftDown && !_checkDown)
        {
            res.Add(Tiles[9]);
            addThreeWay = false;
        }

        if (_checkCenter && !_checkLeft && !_checkLeftUp && !_checkUp)
        {
            res.Add(Tiles[7]);
            addThreeWay = false;
        }

        if (_checkCenter && !_checkUp && !_checkUpRight && !_checkRight)
        {
            res.Add(Tiles[8]);
            addThreeWay = false;
        }

        if (_checkCenter && !_checkRight && !_checkRightDown && !_checkDown)
        {
            res.Add(Tiles[6]);
            addThreeWay = false;
        }

        if (addThreeWay)
        {
            AddThreeWay(res);
        } else
        {
            AddFourWay(res);
        }
    }

    private void AddThreeWay(List<SimpleTile> res)
    {
        if (_checkCenter && !_checkUp)
        {
            res.Add(Tiles[3]);
        }

        if (_checkCenter && !_checkLeft)
        {
            res.Add(Tiles[4]);
        }

        if (_checkCenter && !_checkRight)
        {
            res.Add(Tiles[5]);
        }

        if (_checkCenter && !_checkDown)
        {
            res.Add(Tiles[2]);
        }

        AddFourWay(res);
    }

    private void AddFourWay(List<SimpleTile> res)
    {
        if (_checkCenter && !_checkLeftUp && _checkUp && _checkLeft)
        {
            res.Add(Tiles[11]);
        }

        if (_checkCenter && !_checkLeftDown && _checkLeft && _checkDown)
        {
            res.Add(Tiles[12]);
        }

        if (_checkCenter && !_checkRightDown && _checkRight && _checkDown)
        {
            res.Add(Tiles[10]);
        }

        if (_checkCenter && !_checkUpRight && _checkUp && _checkRight)
        {
            res.Add(Tiles[13]);
        }
    }

    // Up y - max
    // Down y - min
    // Left x - min
    // Right x - max

    private bool CheckCenter(int x, int y, List<List<int>> map) => map[x][y] == 1;
    private bool CheckLeftUp(int x, int y, List<List<int>> map) => x > 0 && y + 1 < map[x].Count && map[x - 1][y + 1] == 1;
    private bool CheckUp(int x, int y, List<List<int>> map) => y + 1 < map[x].Count && map[x][y + 1] == 1;
    private bool CheckUpRight(int x, int y, List<List<int>> map) => x + 1 < map.Count && y + 1 < map[x].Count && map[x + 1][y + 1] == 1;
    private bool CheckRight(int x, int y, List<List<int>> map) => x + 1 < map.Count && map[x + 1][y] == 1;
    private bool CheckRightDown(int x, int y, List<List<int>> map) => x + 1 < map.Count && y > 0 && map[x + 1][y - 1] == 1;
    private bool CheckDown(int x, int y, List<List<int>> map) => y > 0 && map[x][y - 1] == 1;
    private bool CheckLeftDown(int x, int y, List<List<int>> map) => x > 0 && y > 0 && map[x - 1][y - 1] == 1;
    private bool CheckLeft(int x, int y, List<List<int>> map) => x > 0 && map[x - 1][y] == 1;


    private void ApplyTilePrefab(TilePrefab tilePrefab, Vector3Int pos, Tilemap tilemap, int zPos)
    {
        foreach (var preset in tilePrefab.Presets)
        {
            var adjPos = pos;
            adjPos.z += zPos;
            tilemap.SetTile(adjPos, preset.Tile);
        }
    }
}
