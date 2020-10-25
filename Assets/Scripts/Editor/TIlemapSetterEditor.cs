using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TilemapSetter), true)]
public class TilemapSetterEditor : Editor
{
    private TilemapSetter _tilemapSetter;

    private void OnEnable()
    {
        _tilemapSetter = target as TilemapSetter;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (Application.isPlaying)
            if (GUILayout.Button("Regenerate"))
                _tilemapSetter.GenerateLevel();
    }
}
