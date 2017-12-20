using UnityEngine;

public class FearTrap : TrapModel
{
    [SerializeField] private int m_power;

    public override void Activate(CharacterBehaviour character, bool evolved, Vector2Int position, Vector2Int targetPosition)
    {
        character.OnFear(evolved ? ((FearTrap)Evolution).m_power : m_power);
    }
}