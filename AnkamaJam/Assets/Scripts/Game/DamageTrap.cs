using System;
using UnityEngine; 
public class DamageTrap : TrapModel
{
	[SerializeField]
	private int m_damage; 

	public override void Activate(CharacterBehaviour character) {
        base.Activate(character);
        character.Damage (m_damage);
	}	
}

