﻿using System.Collections;
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
	public Image AlertGauge1;
	public Image AlertGauge2;
	public Image AlertGauge3;
	public Image AlertGauge4;
	public Image AlertGauge5;
	private Tweener _alertTweener;
	private Vector2 _soulGaugeBaseSizeDelta;
	private Image _lastAlertGaugeActivated;

	// Use this for initialization
	void Start ()
	{
		_soulGaugeBaseSizeDelta = SoulGauge.rectTransform.sizeDelta;
		SoulGauge.rectTransform.sizeDelta = new Vector2(_soulGaugeBaseSizeDelta.x, 0);
		
		var doPunchScale = ComboText.transform.DOPunchScale(Vector3.one * 0.1f, 0.25f).Pause();
		GameSingleton.Instance.OnAlertUpdate += f =>
		{
			var alertPercent = (float)f/GameSingleton.MaxAlert;
			var alerteGaugeToBlink = GetAlerteGaugeToBlink(alertPercent);
			if (alerteGaugeToBlink != null && _alertTweener == null)
			{
				_alertTweener = alerteGaugeToBlink.DOFade(0f, 1f).OnComplete(() => alerteGaugeToBlink.DOFade(1f, 1f)).SetLoops(-1);
			}
			else
			{
				var alerteGaugeToActive = GetAlerteGaugeToActive(alertPercent);
				if (alerteGaugeToActive != null && _lastAlertGaugeActivated != alerteGaugeToActive)
				{
					_lastAlertGaugeActivated = alerteGaugeToActive;
					_alertTweener = null;
					alerteGaugeToActive.DOKill();
					alerteGaugeToActive.color = Color.white;
				}	
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

	public Image GetAlerteGaugeToBlink(float percent)
	{
		if (percent > 0.15 && percent < 0.2)
		{
			return AlertGauge1;
		}
		if (percent > 0.35 && percent < 0.4)
		{
			return AlertGauge2;
		}
		if (percent > 0.55 && percent < 0.6)
		{
			return AlertGauge3;
		}
		if (percent > 0.75 && percent < 0.8)
		{
			return AlertGauge4;
		}
		if (percent > 0.95 && percent < 1)
		{
			return AlertGauge5;
		}
		return null;
	}

	public Image GetAlerteGaugeToActive(float percent)
	{
		if (percent >= 0.2 && percent < 0.4)
		{
			return AlertGauge1;
		}
		if (percent < 0.6)
		{
			return AlertGauge2;
		}
		if (percent < 0.8)
		{
			return AlertGauge3;
		}
		if (percent < 1)
		{
			return AlertGauge4;
		}
		if (percent >= 1)
		{
			return AlertGauge5;
		}
		return null;
	}

	public void QuitGame()
	{
		var showPanels = SceneHandler.Instance.gameObject.GetComponent<ShowPanels>();
		SceneHandler.Instance.Load(SceneHandler.StartScene, () => showPanels.ShowMenu());
	}
}
