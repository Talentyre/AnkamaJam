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

        var myPositions = gridInformation.GetAllPositions(TilemapProperty.MovingSidewalkProperty);
        foreach (var myPosition in myPositions)
        {
            Object positionProperty = gridInformation.GetPositionProperty(myPosition, TilemapProperty.MovingSidewalkProperty, (Object) null);
            Debug.Log("Start positions  = "+myPosition+" ??? "+ positionProperty );
        }
        
        
    }
}
