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

    public void Init(TrapModel model, Vector3Int position)
    {
        m_model = model;
        m_nextActivation = Time.realtimeSinceStartup + m_model.Delay;
        m_activationPositions = new List<Vector2Int>();

        m_position = new Vector2Int(position.x, position.y);
        m_activationPositions.AddRange(m_model.ActivationPositions(m_position));
    }

    public TrapModel Model { get { return m_model; } }

    public void Act()
    {
        if (m_nextActivation <= Time.realtimeSinceStartup)
        {
            IEnumerable<CharacterBehaviour> charactersInTrap = GameSingleton.Instance.GetCharactersAt(m_activationPositions);
            foreach (var characterBehaviour in charactersInTrap)
            {
                m_model.Activate(characterBehaviour);
            }

            m_model.Animator.SetTrigger("TriggerTrap");
            m_nextActivation = Time.realtimeSinceStartup + m_model.Cooldown;
        }
		
	}
 
    public void OnPurchase()
    {
        GameSingleton.Instance.OnSoulModified(-Model.Souls,transform.position);
    }
    
}