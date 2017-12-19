using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName ="WaveEnd", menuName ="WaveEnd")]
public class WaveEnd : WaveElement
{

    public override IEnumerator Action(Wave config)
    {
        yield return new WaitForSeconds(Duration);
        GameSingleton.Instance.WaveManager.EndWave();
    }
}
