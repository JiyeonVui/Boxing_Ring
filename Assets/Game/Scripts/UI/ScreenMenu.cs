using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenMenu : ScreenBase
{
    [SerializeField] private Button _btnSolo;
    [SerializeField] private Button _btnOneVsMany;
    [SerializeField] private Button _btnManyVsMany;

    public override void OnInit()
    {
        base.OnInit();
        _btnSolo.onClick.AddListener(() =>
        {
            GameController.Instance.InitSolo(() =>
            {
                OnHide();
                CameraManager.Instance.PlayIntro(() =>
                {
                    // countdown
                    UIManager.Instance.ShowScreen<TouchController>();
                });
            });
        });

    }
}
