using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName ="Multiple tile", menuName = "Tile sets/Multiple Tile", order = 0)]
public class MultipleTIle : SimpleTile
{
    [SerializeField]
    private List<TilePrefab> _tiles;
    public override TilePrefab GetTile() => _tiles[Random.Range(0, _tiles.Count)];
}
