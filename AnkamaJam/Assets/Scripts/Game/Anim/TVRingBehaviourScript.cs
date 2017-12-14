using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVRingBehaviourScript : StateMachineBehaviour
{
	private float _nextEyesTime;
	private Animator _animator;
	private const float MinEyesApparition = 1f; 
	private const float MaxEyesApparition = 3f;
	private bool _eyesSet;
	
	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (!_eyesSet && Time.time > _nextEyesTime)
		{
			_animator.SetBool("TvComp",true);
			_eyesSet = true;
		}
	}
	
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		_animator = animator;
		_nextEyesTime = Time.time + Random.Range(MinEyesApparition, MaxEyesApparition);
		_eyesSet = false;
	}
	
	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		_animator.SetBool("TvComp",false);
	}
}
