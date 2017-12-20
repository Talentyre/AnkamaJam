using UnityEngine;

public class FearTrap : TrapModel
{
    [SerializeField] private int m_power;

    public override void Activate(CharacterBehaviour character, bool evolved)
    {
        character.OnFear(evolved ? ((FearTrap)Evolution).m_power : m_power);
    }
}