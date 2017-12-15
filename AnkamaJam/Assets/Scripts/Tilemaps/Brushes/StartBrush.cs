using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

[CustomGridBrush(false, true, false, "StartBrush")]
public class StartBrush : GridBrushBase
{
#if UNITY_EDITOR
    [MenuItem("Assets/Create/CustomAssets/StartBrush", false, 0)]
    //This Function is called when you click the menu entry
    private static void CreateStartBrush()
    {
        string fileName = "StartBrush";
        StartBrush mytb = new StartBrush();
        mytb.name = fileName + ".asset";
        AssetDatabase.CreateAsset(mytb, "Assets/Tilemap/Brushes/" + mytb.name + "");
    }
#endif

    public string m_startLayerName = "HorrorHouseStart";
    public TileBase m_start;
    public bool IsStart = true;

    private string CurrentProperty
    {
        get { return IsStart ? TilemapProperty.StartProperty : TilemapProperty.EndProperty; }
    }

    //Paint internal macht wahrscheinlich selber noch kein Tile an die gewünschte Position
    public override void Paint(GridLayout grid, GameObject layer, Vector3Int position)
    {
        var gridInformation = BrushUtility.GetRootGridInformation(true);
        Tilemap start = GetTilemap();

        if (start != null)
        {
            gridInformation.ErasePositionProperty(position, CurrentProperty);
            gridInformation.SetPositionProperty(position, CurrentProperty, 1);
            PaintInternal(position, start);
        }
    }

    private void PaintInternal(Vector3Int position, Tilemap map)
    {
        map.SetTile(position, m_start);
    }

    public override void Erase(GridLayout grid, GameObject brushTarget, Vector3Int position)
    {
        Tilemap map = GetTilemap();
        if (map != null)
        {
            var gridInformation = BrushUtility.GetRootGridInformation(true, map.layoutGrid);
            gridInformation.ErasePositionProperty(position, CurrentProperty);
            map.SetTile(position, null);
        }
    }

    private Tilemap GetTilemap()
    {
        GameObject go = GameObject.Find(m_startLayerName);
        return go != null ? go.GetComponent<Tilemap>() : null;
    }
}