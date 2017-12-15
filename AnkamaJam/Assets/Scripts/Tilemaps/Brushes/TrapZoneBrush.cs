using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

[CustomGridBrush(false, true, false, "TrapZoneBrush")]
public class TrapZoneBrush : GridBrushBase
{
#if UNITY_EDITOR
    [MenuItem("Assets/Create/CustomAssets/TrapZoneBrush", false, 0)]
    //This Function is called when you click the menu entry
    private static void CreateStartBrush()
    {
        string fileName = "TrapZoneBrush";
        TrapZoneBrush mytb = new TrapZoneBrush();
        mytb.name = fileName + ".asset";
        AssetDatabase.CreateAsset(mytb, "Assets/Tilemap/Brushes/" + mytb.name + "");
    }
#endif

    public string m_startLayerName = "HorrorHouseTraps";
    public TileBase m_trap;

    //Paint internal macht wahrscheinlich selber noch kein Tile an die gewünschte Position
    public override void Paint(GridLayout grid, GameObject layer, Vector3Int position)
    {
        var gridInformation = BrushUtility.GetRootGridInformation(true);
        Tilemap start = GetTilemap();

        if (start != null)
        {
            gridInformation.ErasePositionProperty(position, TilemapProperty.TrapProperty);
            gridInformation.SetPositionProperty(position, TilemapProperty.TrapProperty, 1);
            PaintInternal(position, start);
        }
    }

    private void PaintInternal(Vector3Int position, Tilemap map)
    {
        map.SetTile(position, m_trap);
    }

    public override void Erase(GridLayout grid, GameObject brushTarget, Vector3Int position)
    {
        Tilemap map = GetTilemap();
        if (map != null)
        {
            var gridInformation = BrushUtility.GetRootGridInformation(true, map.layoutGrid);
            gridInformation.ErasePositionProperty(position, TilemapProperty.TrapProperty);
            map.SetTile(position, null);
        }
    }

    private Tilemap GetTilemap()
    {
        GameObject go = GameObject.Find(m_startLayerName);
        return go != null ? go.GetComponent<Tilemap>() : null;
    }
}