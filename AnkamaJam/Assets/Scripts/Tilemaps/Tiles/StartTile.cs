using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class StartTile : TileBase {
#if UNITY_EDITOR
    [MenuItem("Assets/Create/CustomAssets/StartTile",false,1)]
    private static void CreateStartTile()
    {
//        Sprite[] myTextures = InitiateSlots(); 
//
//        if (myTextures != null)
//        { 
//            Debug.Log("Loaded mySPrites");
//            Debug.Log(myTextures.GetType() + "Length: " + myTextures.Length );
//            Debug.Log(myTextures[0].name);
//        }
//        else
//        {
//            Debug.Log("Texture not loaded");
//        }

        string fName2 = "StartTile";
        StartTile myAT = new StartTile();

        myAT.name = fName2 + ".asset";
        AssetDatabase.CreateAsset(myAT, "Assets/Tilemap/Tiles/" +myAT.name + "");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
#endif

    public Sprite StartSPrite;
    
    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
	{
		
        tileData.sprite = StartSPrite;
        tileData.flags = TileFlags.LockAll;
	}
}
