using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName ="CharacterWaveElement", menuName = "CharacterWaveElement")]
public class CharacterWaveElement : WaveElement
{

    [SerializeField]
    private CharacterModel m_character;

    public override IEnumerator Action(Wave config)
    {
        yield return new WaitForSeconds(Duration);

        GameSingleton.Instance.RequestCharacterSpawn(new CharacterSpawn(m_character, config.Position));
    }
}
