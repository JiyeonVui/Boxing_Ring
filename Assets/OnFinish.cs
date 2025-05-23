using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnFinish : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Debug.Log("OnStateExit " + animator.name);
        animator.transform.parent.GetComponent<PlayerBoxer>().OnAttackCallBack(stateInfo.length);
    }
}
