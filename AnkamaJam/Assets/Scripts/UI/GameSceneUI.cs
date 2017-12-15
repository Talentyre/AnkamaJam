using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneUI : MonoBehaviour
{

	public Text ScoreText;
	public Text ComboText;
	public Text SoulText;
	public Image SoulGauge;
	
	// Use this for initialization
	void Start ()
	{
		var doPunchScale = ComboText.transform.DOPunchScale(Vector3.one * 0.1f, 0.25f).Pause();
		GameSingleton.Instance.OnSoulUpdate += f =>
		{
			SoulText.text = f.ToString();
			SoulGauge.fillAmount = (float)f/GameSingleton.MaxSouls;
		};
		GameSingleton.Instance.OnComboUpdate += f =>
		{
			if (f > 1)
			{
				ComboText.text = "x " + f;
				doPunchScale.Restart();
			}
			else
			{
				ComboText.text = "";	
			}
		};
		GameSingleton.Instance.OnScoreUpdate += f =>
		{
			ScoreText.text = f.ToString();
		};
	}

	public void QuitGame()
	{
		var showPanels = SceneHandler.Instance.gameObject.GetComponent<ShowPanels>();
		SceneHandler.Instance.Load(SceneHandler.StartScene, () => showPanels.ShowMenu());
	}
}
