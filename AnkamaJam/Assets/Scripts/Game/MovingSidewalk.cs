using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class MovingSidewalk : MonoBehaviour
{
    [SerializeField]
    private List<Direction> m_entrances;
    [SerializeField]
    private List<Direction> m_exits;
    [SerializeField]
    private Sprite m_sprite;

    public List<Direction> Entrances { get { return m_entrances; } }
    public List<Direction> Exits { get { return m_exits; } }
    public Sprite Sprite { get { return m_sprite; } }

    public Direction PickEntrance()
    {
        var index = Helper.random(m_entrances.Count);
        return m_entrances[index];
    }

    public Direction PickExit()
    {
        var index = Helper.random(m_exits.Count);
        return m_exits[index];
    }

    public static Vector2Int GetVector2IntFromDirection(Direction direction)
    {
        switch (direction)
        {
            case Direction.East:
                return Vector2Int.right;
            case Direction.West:
                return Vector2Int.left;
            case Direction.North:
                return Vector2Int.up;
            case Direction.South:
                return Vector2Int.down;
        }
        return Vector2Int.down;
    }
}
