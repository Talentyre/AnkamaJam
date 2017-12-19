using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrapMenu : MonoBehaviour
{

	public Button SellButton;
	public Button UpgradeButton;
	private Trap _trap;

	private void Awake()
	{
		SellButton.onClick.AddListener(Sell);
		UpgradeButton.onClick.AddListener(UpgradeTrap);
	}

	private void Sell()
	{
		GameSingleton.Instance.RemoveTrap(_trap);
		GameSingleton.Instance.OnSoulModified(_trap.SellCost, transform.position);
	}

	private void UpgradeTrap()
	{
		_trap.Evolved = true;
		GameSingleton.Instance.OnSoulModified(-_trap.Model.m_evolution.Souls, transform.position);
		gameObject.SetActive(false);
	}

	public void UpdateInfos(Trap trap)
	{
		_trap = trap;
		SellButton.GetComponentInChildren<Text>().text = "Del +" + trap.SellCost;
		var upCost = trap.Model.m_evolution.Souls;
		UpgradeButton.GetComponentInChildren<Text>().text = "Up -" + upCost;
		UpgradeButton.interactable = !trap.Evolved && upCost <= GameSingleton.Instance.Souls;
		
		
	}
}
