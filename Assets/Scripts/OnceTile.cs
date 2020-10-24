using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName ="Once tile", menuName ="Tile sets/Once Tile", order = 0)]
public class OnceTile : SimpleTile
{
    [SerializeField]
    private TilePrefab _tile;
    public override TilePrefab GetTile() => _tile;
}
