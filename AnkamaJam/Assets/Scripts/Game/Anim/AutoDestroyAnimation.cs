using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyAnimation : MonoBehaviour {
	private float m_lifetime = 1f;
	private int m_checkFrameInterval = 30;
	private Animator[] m_animators;

	// Use this for initialization
	void Start () {
		m_animators = GetComponentsInChildren<Animator>();
		StartCoroutine(CheckCoroutine());
	}

	private IEnumerator CheckCoroutine()
	{
		if(m_lifetime > 0f)
		{
			yield return new WaitForSeconds(m_lifetime);
		}
		
		while(!IsAnyAnimatorPlaying())
		{
			int frameBuffer = m_checkFrameInterval;
			do
			{
				yield return new WaitForEndOfFrame();
				frameBuffer--;
			}
			while (frameBuffer > 0);
		}
		
		Destroy(gameObject);
	}
	
	protected bool IsAnyAnimatorPlaying()
	{
		int animatorCount = m_animators.Length;
		for (int i = 0; i < animatorCount; ++i)
		{
			Animator a = m_animators[i];
			if (a.IsInTransition(0))
			{
				return true;
			}
			else
			{
				AnimatorStateInfo info = a.GetCurrentAnimatorStateInfo(0);
				if (info.normalizedTime < 1f || info.loop)
				{
					return true;
				}
			}
		}

		return false;
	}
}
