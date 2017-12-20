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
		_trap.Model.EvolutionIndex++;
		GameSingleton.Instance.OnSoulModified(-_trap.Model.Evolution.Souls, transform.position);
		gameObject.SetActive(false);
	}

	public void UpdateInfos(Trap trap)
	{
		_trap = trap;
		SellButton.GetComponentInChildren<Text>().text = "+ " + trap.SellCost;
		var trapEvolutionCount = trap.Model.m_evolutions.Count-1;
		var index = Mathf.Max(0,Mathf.Min(trapEvolutionCount,_trap.Model.EvolutionIndex+1));
		var upCost = trap.Model.m_evolutions[index].Souls;
		UpgradeButton.GetComponentInChildren<Text>().text = "- " + upCost;
		UpgradeButton.interactable = _trap.Model.EvolutionIndex < trapEvolutionCount && upCost <= GameSingleton.Instance.Souls;
	}
}
