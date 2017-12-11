using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneUI : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void QuitGame()
	{
		var showPanels = SceneHandler.Instance.gameObject.GetComponent<ShowPanels>();
		SceneHandler.Instance.Load(SceneHandler.StartScene, () => showPanels.ShowMenu());
	}
}
