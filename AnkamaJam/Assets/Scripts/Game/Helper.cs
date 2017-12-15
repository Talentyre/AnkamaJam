﻿using System.Collections.Generic;
using UnityEngine;

public class Helper
{
    private static System.Random m_random = new System.Random();

    public static int random(int max)
    {
        return m_random.Next(max);
    }

    public static Vector2Int ToVector2Int(Vector3Int pos)
    {
        return new Vector2Int(pos.x, pos.y);
    }
    
    public static Vector3Int ToVector3Int(Vector2Int pos)
    {
        return new Vector3Int(pos.x, pos.y, 0);
    }


    public static List<Vector2Int> ToVector2Int(List<Vector3Int> positions)
    {
        var list = new List<Vector2Int>();
        positions.ForEach((p) => list.Add(ToVector2Int(p)));
        return list;
    }

    public static List<Vector3Int> ToVector3Int(List<Vector2Int> positions)
    {
        var list = new List<Vector3Int>();
        positions.ForEach((p) => list.Add(ToVector3Int(p)));
        return list;
    }
}
