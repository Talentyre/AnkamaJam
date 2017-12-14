using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Object = UnityEngine.Object;

public class TestTileMap : MonoBehaviour
{
    public Tilemap StartMap;

    private void Start()
    {
        var gridInformation = BrushUtility.GetRootGridInformation(StartMap.layoutGrid);

        var myPositions = gridInformation.GetAllPositions(TilemapProperty.StartProperty);
        foreach (var myPosition in myPositions)
        {
            var positionProperty = gridInformation.GetPositionProperty(myPosition, TilemapProperty.StartProperty, 0);
            Debug.Log("Start positions  = "+myPosition+" ??? "+ positionProperty );
        }
        
        
    }
}
