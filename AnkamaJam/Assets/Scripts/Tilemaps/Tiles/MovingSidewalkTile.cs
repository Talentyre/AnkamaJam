using UnityEngine;
using UnityEngine.Tilemaps;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;

public class MovingSidewalkTile : TileBase
{
#if UNITY_EDITOR
    [MenuItem("Assets/Create/CustomAssets/MovingSidewalkTile", false, 1)]
    private static void CreateStartTile()
    {
        string fName2 = "MovingSidewalkTile";
        MovingSidewalkTile myAT = new MovingSidewalkTile();

        myAT.name = fName2 + ".asset";
        AssetDatabase.CreateAsset(myAT, "Assets/Tilemap/Tiles/MovingSidewalk/" + myAT.name + "");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
#endif

    public MovingSidewalk m_movingSideWalk;

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        tileData.sprite = m_movingSideWalk.Sprite;
        tileData.flags = TileFlags.LockAll;
    }
}
