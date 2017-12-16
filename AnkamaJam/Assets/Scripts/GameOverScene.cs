using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScene : MonoBehaviour {

	public void Replay()
	{
		var showPanels = SceneHandler.Instance.gameObject.GetComponent<ShowPanels>();
		SceneHandler.Instance.Load(SceneHandler.StartScene, () => showPanels.ShowMenu());
	}
}
