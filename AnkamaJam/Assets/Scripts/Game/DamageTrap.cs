using System;
using UnityEngine; 
public class DamageTrap : TrapModel
{
	[SerializeField]
	private int m_damage;

	protected int Damage
	{
		get { return m_damage; }
	}


	public override void Activate(CharacterBehaviour character, bool evolved) {
        character.Damage (evolved ? ((DamageTrap)m_evolution).Damage : m_damage);
	}	
}

