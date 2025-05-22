using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TouchController : ScreenBase
{
    //public PlayerBoxer boxer;
    public bool isHolding = false;

    [SerializeField] private Button _btnHold;
    [SerializeField] private Button _btnAttack;
    [SerializeField] private CanvasGroup _uiCv;
    [SerializeField] private TextMeshProUGUI _countDownText;
    [SerializeField] private GameObject _blackMaskObj;

    private Coroutine _corCountDown;
    public override void OnInit()
    {
        base.OnInit();
        _uiCv.SetActive(false);
        _blackMaskObj.SetActive(true);
        _btnAttack.onClick.AddListener(() =>
        {
            if (isHolding)
            {
                return;
            }

            GameController.attackAction?.Invoke();
        });
        
    }

    public override void OnShow()
    {
        base.OnShow();
        if(_corCountDown != null)
        {
            StopCoroutine(_corCountDown);
        }

        _corCountDown = StartCoroutine(IECountDown());
    }

    private IEnumerator IECountDown()
    {
        _countDownText.SetText("3");
        yield return new WaitForSeconds(0.8f);
        _countDownText.SetText("2");
        yield return new WaitForSeconds(0.8f);
        _countDownText.SetText("1");
        yield return new WaitForSeconds(0.8f);
        _countDownText.SetText("Go");
        yield return new WaitForSeconds(0.5f);
        _uiCv.SetActive(true);
        _blackMaskObj.SetActive(false);
        GameController.Instance.BattleStart();
    }

    public void OnHold()
    {
        isHolding = true;
        GameController.holdAction?.Invoke(isHolding);
    }

    public void OnReleaseHold()
    {
        isHolding = false;
        GameController.holdAction?.Invoke(isHolding);
    }
}




