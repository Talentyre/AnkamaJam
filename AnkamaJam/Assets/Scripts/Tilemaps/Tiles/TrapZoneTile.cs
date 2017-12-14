using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TrapZoneTile : TileBase {
#if UNITY_EDITOR
    [MenuItem("Assets/Create/CustomAssets/TrapZoneTile",false,1)]
    private static void CreateStartTile()
    {
        string fName2 = "TrapZoneTile";
        TrapZoneTile myAT = new TrapZoneTile();

        myAT.name = fName2 + ".asset";
        AssetDatabase.CreateAsset(myAT, "Assets/Tilemap/Tiles/" +myAT.name + "");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
#endif

    public Sprite TrapZoneSprite;
    
    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
	{
		
        tileData.sprite = TrapZoneSprite;
        tileData.flags = TileFlags.LockAll;
	}
}
