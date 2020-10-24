using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class SimpleTile : ScriptableObject
{
    [Serializable]
    public class TilePreset
    {
        public Tile Tile;
        public int ZPos;
    }

    [Serializable]
    public class TilePrefab
    {
        public List<TilePreset> Presets; 
    }

    public abstract TilePrefab GetTile();
}
