using UnityEngine;

public class StunTrap : TrapModel
{
    [SerializeField]
    private float m_duration;

    public override void Activate(CharacterBehaviour character)
    {
        character.OnStun(m_duration);
    }
}