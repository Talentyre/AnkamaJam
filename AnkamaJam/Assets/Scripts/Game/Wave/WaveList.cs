using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName ="WaveList", menuName ="WaveList")]
public class WaveList : WaveElement
{
    [SerializeField] private List<WaveElement> m_elements;

    public override IEnumerator Action(Wave config)
    {
        for (int i = 0, size = m_elements.Count; i < size; ++i)
        {
            yield return m_elements[i].Action(config);
        }
    }
}
