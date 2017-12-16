using UnityEngine;

public class FearTrap : TrapModel
{
    [SerializeField] private int m_power;

    public override void Activate(CharacterBehaviour character)
    {
        character.OnFear(m_power);
    }
}