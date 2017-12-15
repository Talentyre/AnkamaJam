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
	public Image AlertGauge;
	private Tweener _alertTweener;
	private Vector2 _soulGaugeBaseSizeDelta;

	// Use this for initialization
	void Start ()
	{
		_soulGaugeBaseSizeDelta = SoulGauge.rectTransform.sizeDelta;
		SoulGauge.rectTransform.sizeDelta = new Vector2(_soulGaugeBaseSizeDelta.x, 0);
		
		var doPunchScale = ComboText.transform.DOPunchScale(Vector3.one * 0.1f, 0.25f).Pause();
		GameSingleton.Instance.OnAlertUpdate += f =>
		{
			AlertGauge.fillAmount = (float)f/GameSingleton.MaxAlert;
			if (AlertGauge.fillAmount > 0.9 && _alertTweener == null)
			{
				_alertTweener = AlertGauge.DOFade(0f, 1f).OnComplete(() => AlertGauge.DOFade(1f, 1f)).SetLoops(-1);
			}
		};
		GameSingleton.Instance.OnSoulUpdate += f =>
		{
			SoulText.text = f.ToString();
			var percent = (float)f/GameSingleton.MaxSouls;
			SoulGauge.rectTransform.sizeDelta = new Vector2(_soulGaugeBaseSizeDelta.x, _soulGaugeBaseSizeDelta.y*percent);
			//SoulGauge.fillAmount = percent;
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
