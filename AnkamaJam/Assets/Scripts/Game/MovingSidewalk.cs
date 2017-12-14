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
        var index = (int)Mathf.Floor(Helper.random(m_entrances.Count + 1));
        return m_entrances[index];
    }

    public Direction PickExit()
    {
        var index = (int)Mathf.Floor(Helper.random(m_entrances.Count + 1));
        return m_entrances[index];
    }
}
