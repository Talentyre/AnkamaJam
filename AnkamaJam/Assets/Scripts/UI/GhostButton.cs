using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GhostButton : MonoBehaviour
{
	[SerializeField] 
	private TrapModel m_model;
	
	public float BaseGhostCooldown = 20;
	public float _ghostCooldown = 20;
	private bool _activated;
	
	private Image _image;
	private Sequence _sequence;
	private Trap m_ghost;

	private void Start()
	{
		_image = GetComponent<Image>();
	}

	private void Update()
	{
		if (_ghostCooldown <= 0)
			return;
			
		if (_activated)
		{
			if (Input.GetMouseButton(0))
			{
				Debug.Log("GHOST !");
				_activated = false;
				_ghostCooldown = BaseGhostCooldown;
				_sequence.Kill();
			}
			return;	
		}
		_image.fillAmount = (BaseGhostCooldown - _ghostCooldown) / BaseGhostCooldown;
		_ghostCooldown -= Time.deltaTime;
		if (_ghostCooldown <= 0)
		{
			_sequence = DOTween.Sequence();
			_sequence.Append(transform.DOPunchScale(Vector3.one * 0.1f, 0.5f))
				.AppendInterval(1f).SetLoops(-1);	
		}
	}
	
	public void OnClick()
	{
		
		_activated = true;
		m_ghost = GameSingleton.Instance.TrapManager.StartPreviewTrap(m_model);

	}
}
