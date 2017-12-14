using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableDataTilemaps : MonoBehaviour
{
	public List<GameObject> GameObjectToDisable;

	private void Start()
	{
		GameObjectToDisable.ForEach(g => g.SetActive(false));
	}
}
