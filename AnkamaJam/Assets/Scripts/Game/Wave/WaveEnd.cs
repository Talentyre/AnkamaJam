using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "WavePause", menuName = "WavePause")]
public class WavePause : WaveElement
{
    public override IEnumerator Action(Wave config)
    {
        yield return new WaitForSeconds(Duration);
    }
}
