using UnityEngine;
using System.Collections.Generic;

public abstract class TrapModel : MonoBehaviour
{
    [SerializeField]
    private Sprite m_trapUISprite;
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
    [SerializeField]
    private TrapAOE m_aoe = TrapAOE.Point;
    [SerializeField]
    private int m_souls;

    public int Cooldown { get { return m_cooldown; } }
    public int Souls { get { return m_souls; } }

    public List<Vector2Int> ActivationPositions(Vector2Int pos)
    {
        var list = new List<Vector2Int>();
        list.Add(pos);

        switch (m_aoe)
        {
            case TrapAOE.Point:
                break;
            case TrapAOE.LineHorizontal:
                list.Add(pos + Vector2Int.left);
                list.Add(pos + Vector2Int.right);
                break;
            case TrapAOE.LineVertical:
                list.Add(pos + Vector2Int.up);
                list.Add(pos + Vector2Int.down);
                break;
            case TrapAOE.Cross:
                list.Add(pos + Vector2Int.left);
                list.Add(pos + Vector2Int.right);
                list.Add(pos + Vector2Int.up);
                list.Add(pos + Vector2Int.down);
                break;
        }

        return list;
    }

    public abstract void Activate(CharacterBehaviour c);

    public Animator Animator { get { return m_animator; } }

    public float Delay
    {
        get
        {
            return UnityEngine.Random.Range(m_delayMin, m_delayMax);
        }
    }
}

