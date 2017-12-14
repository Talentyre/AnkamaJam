using UnityEngine;
using System;

public abstract class TrapModel : MonoBehaviour
{
	[SerializeField]
	private int m_cooldown;
    [SerializeField]
    private TrapModel m_evolution;
    [SerializeField]
    private Animator m_animator;

    public int Cooldown { get { return m_cooldown; } }

	public virtual void Activate(CharacterBehaviour c)
    {
        m_animator.SetTrigger("TriggerTrap");
    }

    public Animator Animator { get { return m_animator; } }
}

