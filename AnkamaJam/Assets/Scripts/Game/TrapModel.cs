using UnityEngine;
using System;

public abstract class TrapModel : MonoBehaviour
{
	[SerializeField]
	private int m_cooldown;
    [SerializeField]
    private float m_delayMin;
    [SerializeField]
    private float m_delayMax;
    [SerializeField]
    private TrapModel m_evolution;
    [SerializeField]
    private Animator m_animator;

    public int Cooldown { get { return m_cooldown; } }

	public virtual void Activate(CharacterBehaviour c)
    {
    }

    public Animator Animator { get { return m_animator; } }

    public float Delay
    {
        get
        {
            return UnityEngine.Random.Range(m_delayMin, m_delayMax);
        }
    }
}

