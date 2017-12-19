using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[CreateAssetMenu]
public class Wave : ScriptableObject
{
    [SerializeField]
    private Vector3Int m_position;

    [SerializeField]
    private int m_waveIndex;

    [SerializeField]
    private List<WaveElement> m_waves;

    public Vector3Int Position { get { return m_position; } }

    public List<WaveElement> Waves { get { return m_waves; } }

    public int Index { get { return m_waveIndex; } }

    public IEnumerator Action()
    {
        for (int i = 0, size = m_waves.Count; i < size; ++i)
        {
            yield return m_waves[i].Action(this);
        }
    }
}
