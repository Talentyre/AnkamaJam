using UnityEngine;
using System;

public abstract class TrapModel : MonoBehaviour
{
	[SerializeField]
	private int m_cooldown;

	public abstract void Activate(CharacterBehaviour c);
}

