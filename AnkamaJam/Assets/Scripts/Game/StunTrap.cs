using UnityEngine;

public class StunTrap : TrapModel
{
    [SerializeField] public float m_duration;

    public override void Activate(CharacterBehaviour character, bool evolved, Vector2Int position, Vector2Int targetPosition)
    {
        character.OnStun(evolved ? ((StunTrap)Evolution).m_duration : m_duration);
    }
}