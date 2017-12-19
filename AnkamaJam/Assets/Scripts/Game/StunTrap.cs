using UnityEngine;

public class StunTrap : TrapModel
{
    [SerializeField] public float m_duration;

    public override void Activate(CharacterBehaviour character, bool evolved)
    {
        character.OnStun(evolved ? ((StunTrap)m_evolution).m_duration : m_duration);
    }
}