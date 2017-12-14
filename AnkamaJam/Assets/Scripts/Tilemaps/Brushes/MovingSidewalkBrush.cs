using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

[CustomGridBrush(false, true, false, "MovingSidewalkBrush")]
public class MovingSidewalkBrush : GridBrushBase
{
#if UNITY_EDITOR
    [MenuItem("Assets/Create/CustomAssets/MovingSidewalkBrush", false, 0)]
    //This Function is called when you click the menu entry
    private static void CreateStartBrush()
    {
        string fileName = "MovingSidewalkBrush";
        MovingSidewalkBrush mytb = new MovingSidewalkBrush();
        mytb.name = fileName + ".asset";
        AssetDatabase.CreateAsset(mytb, "Assets/Tilemap/Brushes/" + mytb.name + "");
    }

    public MovingSidewalkTile m_tile;

    public override void Paint(GridLayout grid, GameObject layer, Vector3Int position)
    {
        Tilemap map = GetTilemap();
        if (map != null)
        {
            var gridInformation = BrushUtility.GetRootGridInformation(true, map.layoutGrid);
            gridInformation.ErasePositionProperty(position, TilemapProperty.MovingSidewalkProperty);
            gridInformation.SetPositionProperty(position, TilemapProperty.MovingSidewalkProperty, (Object) m_tile.m_movingSideWalk.gameObject);

            PaintInternal(position, map);
        }
    }

    private void PaintInternal(Vector3Int position, Tilemap map)
    {
        map.SetTile(position, m_tile);
    }

    public static Tilemap GetTilemap()
    {
        GameObject go = Selection.activeGameObject;
        return go != null ? go.GetComponent<Tilemap>() : null;
    }

#endif
}