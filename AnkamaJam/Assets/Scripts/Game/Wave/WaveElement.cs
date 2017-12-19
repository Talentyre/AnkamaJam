using UnityEngine;
using System.Collections;

public abstract class WaveElement : ScriptableObject
{
    [SerializeField]
    private float m_duration;

    public float Duration { get { return m_duration; } }

    public abstract IEnumerator Action(Wave config);
}
