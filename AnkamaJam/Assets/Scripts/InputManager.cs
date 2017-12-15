using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class InputManager : MonoBehaviour {

    public event Action<Vector3Int> OnTrapCell;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonUp(0))
		{
			var screenToWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			//var vector3Int = new Vector3Int(Mathf.RoundToInt(screenToWorldPoint.x), Mathf.RoundToInt(screenToWorldPoint.y),0);

			var worldToCell = GameSingleton.Instance.GridLayout.WorldToCell(screenToWorldPoint);
			var positionProperty = GameSingleton.Instance.GridInformation.GetPositionProperty(worldToCell, TilemapProperty.TrapProperty, 0);
			if (positionProperty == 1)
            {
                if (OnTrapCell != null)
                    OnTrapCell(worldToCell);
            }
		}
	}
}
