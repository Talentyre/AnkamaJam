using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Lightning : MonoBehaviour {
	private SpriteRenderer _spriteRenderer;

	private List<Ease> LightningEase = new List<Ease>
	{
		Ease.Flash,
		Ease.InFlash,
		Ease.InOutFlash,
		Ease.OutFlash,
	};

	void Start ()
	{
		_spriteRenderer = GetComponent<SpriteRenderer>();
		_spriteRenderer.color = new Color(1f, 1f, 1f, 0f);
		StartCoroutine(Action());
	}

	private IEnumerator Action()
	{
		while (true)
		{
			yield return new WaitForSeconds(Random.Range(5,15));

			var amplitude = Random.Range(8,16);
			if (amplitude % 2 != 0)
				amplitude++;

			var ease = LightningEase[Helper.random(LightningEase.Count)];
			_spriteRenderer.DOColor(Color.white, Random.Range(0.7f,1.2f)).SetEase(ease, amplitude, 1);
		}
	}
}
