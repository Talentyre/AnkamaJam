using UnityEngine;

using System.Collections.Generic;

public class Trap : MonoBehaviour
{
	private int m_level;
	private TrapModel m_model;

	private float m_lastActivation;

    private Vector2Int m_position;
	private List<Vector2Int> m_activationPositions;

    public void Init(TrapModel model)
    {
        m_model = model;
        m_lastActivation = Time.realtimeSinceStartup;
    }

    public TrapModel Model { get { return m_model; } }

    public void Act()
    {
        if (m_lastActivation + m_model.Cooldown <= Time.realtimeSinceStartup)
        {
            IEnumerable<CharacterBehaviour> charactersInTrap = GameSingleton.Instance.GetCharactersAt(m_activationPositions);
            foreach (var characterBehaviour in charactersInTrap)
            {
                m_model.Activate(characterBehaviour);
            }

            m_lastActivation = Time.realtimeSinceStartup;
        }
		
	}
    
}