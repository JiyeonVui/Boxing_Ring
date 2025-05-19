using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCombo : StateMachineBehaviour
{
    [SerializeField] private string animationName;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.transform.parent.GetComponent<PlayerBoxer>().ChangeAnimation(animationName, 0.2f, stateInfo.length);
    }
}
