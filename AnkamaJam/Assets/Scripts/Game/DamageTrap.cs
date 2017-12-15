using System;
using UnityEngine; 
public class DamageTrap : TrapModel
{
	[SerializeField]
	private int m_damage; 

	public override void Activate(CharacterBehaviour character) {
        character.Damage (m_damage);
	}	
}

