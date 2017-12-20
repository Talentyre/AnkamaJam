using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GhostButton : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	public GameObject GhostPrefab;
	
	public float BaseGhostCooldown = 20;
	public float _ghostCooldown = 20;
	private bool _activated;
	
	private Image _image;
	private Sequence _sequence;
	private GameObject _ghost;

	private void Start()
	{
		_image = GetComponent<Image>();
	}

	private void Update()
	{
		// todo should be in GameSingleton
		if (!GameSingleton.Instance.GameStarted)
			return;
		if (_ghostCooldown <= 0)
			return;
			
		_image.fillAmount = (BaseGhostCooldown - _ghostCooldown) / BaseGhostCooldown;
		_ghostCooldown -= Time.deltaTime;
		if (_ghostCooldown <= 0)
		{
			_activated = true;
			_sequence = DOTween.Sequence();
			_sequence.Append(transform.DOPunchScale(Vector3.one * 0.1f, 0.5f))
				.AppendInterval(1f).SetLoops(-1);	
		}
	}
	
	public void OnBeginDrag(PointerEventData eventData)
	{
		if (!_activated)
			return;
		_ghost = Instantiate(GhostPrefab);
		_ghost.transform.SetParent(GameSingleton.Instance.CharacterSpawner.PnjsTransform);
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (_ghost == null)
			return;
		var pos = Helper.ExtractCellPosition(eventData);
		_ghost.transform.position = pos;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (_ghost == null)
			return;
		_activated = false;
		_ghostCooldown = BaseGhostCooldown;
		_sequence.Kill();
		
		_ghost.GetComponent<Animator>().Play("GhostCri");
		var pos = Helper.ToVector2Int(Helper.ExtractCellPosition(eventData));
		var characters = GameSingleton.Instance.GetCharactersAt(new List<Vector2Int>()
		{
			pos,
			pos+Vector2Int.left,
			pos+Vector2Int.left+Vector2Int.up,
			pos+Vector2Int.up,
			pos+Vector2Int.up+Vector2Int.right,
			pos+Vector2Int.right,
			pos+Vector2Int.right+Vector2Int.down,
			pos+Vector2Int.down,
			pos+Vector2Int.down+Vector2Int.left,
		});
		
		StartCoroutine(DestroyGhost(characters));
	}

	private IEnumerator DestroyGhost(IEnumerable<CharacterBehaviour> characters)
	{
		yield return new WaitForSeconds(0.5f);
		foreach (var characterBehaviour in characters)
		{
			characterBehaviour.OnFear(6);
		}
		yield return new WaitForSeconds(0.5f);
		_ghost.GetComponent<SpriteRenderer>().DOFade(0f, 1f);
		yield return new WaitForSeconds(1f);
		Destroy(_ghost);
	}
}
