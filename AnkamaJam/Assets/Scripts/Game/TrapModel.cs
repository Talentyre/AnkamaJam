using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

public abstract class TrapModel : MonoBehaviour
{
    [SerializeField] private List<Image> m_evolvedImage;
    [SerializeField] private Highlight m_highlight;
    [SerializeField] public int Cooldown;
    [SerializeField] private float m_delayMin;
    [SerializeField] private float m_delayMax;
    [SerializeField] public List<TrapModel> m_evolutions;
    [SerializeField] private List<Animator> m_animator;
    [SerializeField] protected TrapAOE m_aoe = TrapAOE.Point;
    [SerializeField] private TrapAOE m_blockAOE = TrapAOE.Point;
    [SerializeField] private int m_souls;
    [SerializeField] private bool m_automatic = true;
    [SerializeField] private Image m_cooldownImage;

    public int Souls
    {
        get { return m_souls; }
    }

    public bool Automatic
    {
        get { return m_automatic; }
    }

    public Highlight Highlight
    {
        get { return m_highlight; }
    }

    private int m_evolutionIndex = -1;
    public int EvolutionIndex
    {
        get { return m_evolutionIndex; }
        set
        {
            if (value > m_evolutionIndex)
                OnEvolved(value);
            m_evolutionIndex = value;
        }
    }

    public TrapModel Evolution
    {
        get
        {
            return EvolutionIndex < 0 || EvolutionIndex > m_evolutions.Count - 1 ? null : m_evolutions[EvolutionIndex];
        }
    }
    
    private void Awake()
    {
        m_highlight.Unhighlight();
        m_highlight.Init(m_aoe, m_blockAOE);
        m_evolvedImage.ForEach(i => i.color = new Color(1f, 1f, 1f, 0f));
    }

    public void OnEvolved(int level)
    {
        var image = m_evolvedImage[level];
        var sequence = DOTween.Sequence();
        sequence.Append(image.DOFade(1f, 0.25f));
        sequence.Append(image.DOFade(0f, 0.25f));
        sequence.SetLoops(3);
        sequence.OnComplete(() => image.DOFade(1f, 0.25f));
        sequence.Play();
    }

    public List<Vector2Int> ActivationPositions(Vector2Int pos)
    {
        return Helper.PositionsFromAOE(m_aoe, pos);
    }

    public List<Vector2Int> BlockPositions(Vector2Int pos)
    {
        return Helper.PositionsFromAOE(m_blockAOE, pos);
    }

    public abstract void Activate(CharacterBehaviour c, bool evolved, Vector2Int position = new Vector2Int(), Vector2Int targetPosition = new Vector2Int());

    public List<Animator> Animators
    {
        get { return m_animator; }
    }

    public Image CooldownImage
    {
        get { return m_cooldownImage; }
    }

    public float Delay
    {
        get { return Random.Range(m_delayMin, m_delayMax); }
    }
}