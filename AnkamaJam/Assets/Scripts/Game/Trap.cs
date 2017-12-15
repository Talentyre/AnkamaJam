using UnityEngine;

using System.Collections.Generic;

public class Trap : MonoBehaviour
{
	private int m_level;
	private TrapModel m_model;

	private float m_nextActivation;

    private Vector2Int m_position;
	private List<Vector2Int> m_activationPositions;

    public Vector2Int Position { get { return m_position; } }
    public List<Vector2Int> ActivationPositions { get { return m_activationPositions; } }

    public void Init(TrapModel model, Vector3Int position)
    {
        m_model = model;
        m_nextActivation = Time.realtimeSinceStartup + m_model.Delay;
        m_activationPositions = new List<Vector2Int>();

        m_position = new Vector2Int(position.x, position.y);
        m_activationPositions.AddRange(m_model.ActivationPositions(m_position));
    }

    public TrapModel Model { get { return m_model; } }

    public void Act(bool automatic = true)
    {
        if (automatic != m_model.Automatic)
            return;
        if (IsInCooldown)
            return;

        IEnumerable<CharacterBehaviour> charactersInTrap = GameSingleton.Instance.GetCharactersAt(m_activationPositions);
        foreach (var characterBehaviour in charactersInTrap)
        {
            m_model.Activate(characterBehaviour);
        }

        m_model.Animator.SetTrigger("TriggerTrap");
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
    
}