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

    public const string m_startLayerName = "HorrorHouseStart";
    public TileBase m_start;

    //Paint internal macht wahrscheinlich selber noch kein Tile an die gewünschte Position
    public override void Paint(GridLayout grid, GameObject layer, Vector3Int position)
    {
        Debug.Log("PAIN !");
        var gridInformation = BrushUtility.GetRootGridInformation(true);
        Tilemap start = GetStart();

        if (start != null)
        {
            gridInformation.ErasePositionProperty(position, TilemapProperty.StartProperty);
            gridInformation.SetPositionProperty(position, TilemapProperty.StartProperty, 1);
            PaintInternal(position, start);
        }
    }

    private void PaintInternal(Vector3Int position, Tilemap acid)
    {
        acid.SetTile(position, m_start);
    }

    public static Tilemap GetStart()
    {
        GameObject go = GameObject.Find(m_startLayerName);
        return go != null ? go.GetComponent<Tilemap>() : null;
    }
}