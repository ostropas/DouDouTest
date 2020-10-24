using System.Collections.Generic;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
 
[CustomEditor(typeof(TilemapSetter))]
public class UIEquipmentSlotInspector : Editor
{
    private bool _showFolds;

    private SerializedObject _tilemapSetter;
    private SerializedProperty _tiles;

    void OnEnable()
    {
        _tilemapSetter = new SerializedObject(target);
        _tiles = _tilemapSetter.FindProperty("Tiles");
        _showFolds = false;
    }

    private void DrawTiles(SerializedProperty property, string name)
    {
        _showFolds = EditorGUILayout.Foldout(_showFolds, name);

        if (_showFolds)
        {
            int newCount = Mathf.Max(0, EditorGUILayout.IntField("size", property.arraySize));
            EditorGUI.indentLevel++;
            for (int i = 0; i < property.arraySize; i++)
            {
                var propertyElement = property.GetArrayElementAtIndex(i);
                var value = propertyElement.FindPropertyRelative("RuleArrowMask").intValue;
                propertyElement.FindPropertyRelative("RuleArrowMask").intValue = EditorGUILayout.MaskField("Arrows", value, Enum.GetNames(typeof(TilemapSetter.RuleArrow)).ToArray());
                EditorGUILayout.PropertyField(propertyElement.FindPropertyRelative("Tile"));
            }
            property.arraySize = newCount;
            EditorGUI.indentLevel--;
        }
    }
 
    public override void OnInspectorGUI()
    {
 
        base.OnInspectorGUI();

        DrawTiles(_tiles, "Rules tiles list");

        _tilemapSetter.ApplyModifiedProperties();
 
    }
}