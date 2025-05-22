using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpVictory : PopupBase
{
    [SerializeField] private Button _btnNext;
    [SerializeField] private Button _btnBack;

    public override void OnInit()
    {
        base.OnInit();
        _btnNext.onClick.AddListener(() =>
        {
            GameController.Instance.BattleEnd();
            UIManager.Instance.ShowScreen<ScreenLevel>();

        });
    }
}
