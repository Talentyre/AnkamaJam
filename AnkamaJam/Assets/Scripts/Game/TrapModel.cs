using UnityEngine;
using System;

public abstract class TrapModel : MonoBehaviour
{
	[SerializeField]
	private int m_cooldown;
    [SerializeField]
    private TrapModel m_evolution;

    public int Cooldown { get { return m_cooldown; } }

	public abstract void Activate(CharacterBehaviour c);
}

