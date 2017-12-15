using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playSoundAnim : StateMachineBehaviour {

    public AudioClip ClipToPlay;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        var audioSource = animator.gameObject.GetComponent<AudioSource>();
        audioSource.clip = ClipToPlay;
        audioSource.Play();
    }
}
