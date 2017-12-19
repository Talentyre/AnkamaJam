﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public abstract class TrapModel : MonoBehaviour
{
    [SerializeField]
    private Highlight m_highlight;
	[SerializeField]
	public int Cooldown;
    [SerializeField]
    private float m_delayMin;
    [SerializeField]
    private float m_delayMax;
    [SerializeField]
    public TrapModel m_evolution;
    [SerializeField]
    private List<Animator> m_animator;
    [SerializeField]
    private TrapAOE m_aoe = TrapAOE.Point;
    [SerializeField]
    private TrapAOE m_blockAOE = TrapAOE.Point;
    [SerializeField]
    private int m_souls;
    [SerializeField]
    private bool m_automatic = true;
    [SerializeField]
    private Image m_cooldownImage;

    public int Souls { get { return m_souls; } }
    public bool Automatic { get { return m_automatic; } }
    public Highlight Highlight { get { return m_highlight; } }

    private void Awake()
    {
        m_highlight.Unhighlight();
        m_highlight.Init(m_aoe, m_blockAOE);
    }

    public List<Vector2Int> ActivationPositions(Vector2Int pos)
    {
        return Helper.PositionsFromAOE(m_aoe, pos);
    }

    public List<Vector2Int> BlockPositions(Vector2Int pos)
    {
        return Helper.PositionsFromAOE(m_blockAOE, pos);
    }

    public abstract void Activate(CharacterBehaviour c, bool evolved);

    public List<Animator> Animators { get { return m_animator; } }
    public Image CooldownImage { get { return m_cooldownImage; } }

    public float Delay
    {
        get
        {
            return Random.Range(m_delayMin, m_delayMax);
        }
    }
}

