using System;
using System.Collections;
using UnityEngine; 
public class DamageTrap : TrapModel
{
	[SerializeField]
	private int m_damage;

	protected int Damage
	{
		get { return m_damage; }
	}


	public override void Activate(CharacterBehaviour character, bool evolved, Vector2Int position, Vector2Int targetPosition) {
		switch (m_aoe)
		{
			case TrapAOE.LineHorizontal:
			{
				StartCoroutine(DoDamage(1.2f + (targetPosition.x - position.x) * 0.2f, character, evolved));
				break;
			}
			case TrapAOE.LineVertical:
			{
				StartCoroutine(DoDamage(1.2f + (position.y - targetPosition.y) * 0.2f, character, evolved));
				break;
			}
			default:
			{
				character.Damage (evolved ? ((DamageTrap)Evolution).Damage : m_damage);		
				break;
			}
		}
	}

	private IEnumerator DoDamage(float duration, CharacterBehaviour character, bool evolved)
	{
		yield return new WaitForSeconds(duration);
		character.Damage (evolved ? ((DamageTrap)Evolution).Damage : m_damage);
	}
}

