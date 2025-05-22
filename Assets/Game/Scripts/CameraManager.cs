using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoSingleton<CameraManager>
{
    [SerializeField] private Animator _cameraAnimator;

    public void PlayIntro(Action callback = null)
    {
        _cameraAnimator.CrossFade("Intro", 0.1f);
        StartCoroutine(IEWaitIntro());

        IEnumerator IEWaitIntro()
        {
            yield return new WaitForSeconds(8f);
            callback?.Invoke();
        }
    }
}
