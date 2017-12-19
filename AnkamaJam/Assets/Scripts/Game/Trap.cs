using UnityEngine;

using System.Collections.Generic;
using System.Linq;

public class Trap : MonoBehaviour
{
	private int m_level;
	private TrapModel m_model;

	private float m_nextActivation;

    private Vector2Int m_position;
    private List<Vector2Int> m_activationPositions;
    private List<Vector2Int> m_blockPositions;
    private bool m_evolved;
    public float m_cooldown;

    public Vector2Int Position { get { return m_position; } }
    public List<Vector2Int> ActivationPositions { get { return m_activationPositions; } }
    public List<Vector2Int> BlockPositions { get { return m_blockPositions; } }
    public bool Evolved
    {
        get { return m_evolved; }
        set { m_evolved = value; }
    }

    public float Cooldown
    {
        get { return Evolved ? m_model.m_evolution.Cooldown : m_cooldown; }
    }

    public float CooldownPercentage
    {
        get
        {
            if (m_model.Cooldown <= 0)
                return 1;
            var endTime = m_nextActivation;
            var startTime = m_nextActivation - m_model.Cooldown;
            var elapsedTime = Time.realtimeSinceStartup - startTime;
            return elapsedTime / m_model.Cooldown;
        }
    }

    public void UpdateCooldownProgressBar()
    {
        m_model.CooldownImage.fillAmount = CooldownPercentage;
    }

    public void Init(TrapModel model, Vector3Int position)
    {
        m_model = model;
        m_nextActivation = Time.realtimeSinceStartup + m_model.Delay;
        m_activationPositions = new List<Vector2Int>();
        m_blockPositions = new List<Vector2Int>();

        m_position = new Vector2Int(position.x, position.y);
        m_activationPositions.AddRange(m_model.ActivationPositions(m_position));
        m_blockPositions.AddRange(m_model.BlockPositions(m_position));
    }

    public TrapModel Model { get { return m_model; } }

    public void Act(bool automatic = true)
    {
        var cooldown = IsInCooldown;
        m_model.Animators.ForEach(a => a.SetBool("Cooldown", cooldown));
        UpdateCooldownProgressBar();

        if (automatic != m_model.Automatic)
            return;

        if (IsInCooldown)
            return;

        var charactersInTrap = GameSingleton.Instance.GetCharactersAt(m_activationPositions).ToList() ;
        if (automatic && charactersInTrap.Count == 0) 
            return;

        foreach (var characterBehaviour in charactersInTrap)
        {
            m_model.Activate(characterBehaviour, Evolved);
        }

        m_model.Animators.ForEach(a => a.SetTrigger("TriggerTrap"));
        m_nextActivation = Time.realtimeSinceStartup + m_model.Cooldown;
	}
 
    public bool IsInCooldown
    {
        get { return m_model.Cooldown > 0 && m_nextActivation > Time.realtimeSinceStartup; }
    }

    public void OnPurchase()
    {
        GameSingleton.Instance.OnSoulModified(-Model.Souls,transform.position);
    }

    public void Unhighlight()
    {
        m_model.Highlight.Unhighlight();
    }

    public void Highlight(bool effects, bool blocks)
    {
        m_model.Highlight.DoHighlight(effects, blocks);
    }



}