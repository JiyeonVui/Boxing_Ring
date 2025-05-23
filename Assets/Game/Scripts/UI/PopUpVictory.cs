using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpVictory : PopupBase
{
    [SerializeField] private Button _btnNext;
    public override void OnInit()
    {
        base.OnInit();
        _btnNext.onClick.AddListener(() =>
        {
            OnHide();
            GameController.Instance.BattleEnd();
            UIManager.Instance.ShowScreen<ScreenLevel>();
        });
    }

}
