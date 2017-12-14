using UnityEngine;
using UnityEditor;

using System.Collections.Generic;
using System.Linq;

public class Trap : MonoBehaviour
{
	private int Level;
	private TrapModel m_model;

	private float ActTimer;
	public float TimeActivation;
	private bool Active;

    private Vector2Int m_position;
	private List<Vector2Int> m_activationPositions;


	public void Update(){
		ActTimer += Time.deltaTime;
	}
    public void Act()
    {
		if (ActTimer >= TimeActivation) {
			Active = true;
		}

		if (Active == true) {
			ActTimer = 0;
		}

		List<CharacterBehaviour> charactersInTrap = GameSingleton.Instance.GetCharactersAt (m_activationPositions);
		for (int i = 0, size = charactersInTrap; i < size; ++i) {
			CharacterBehaviour c = charactersInTrap [i];
			m_model.Activate (c);
		}
	
    	
	}
    
}