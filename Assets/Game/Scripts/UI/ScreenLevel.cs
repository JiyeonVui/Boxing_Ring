using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenLevel : ScreenBase
{
    [SerializeField] private Transform _content;
    [SerializeField] private Transform _itemLevelTrans;

    [SerializeField] private Button _btnUnlockAllLevel;

    private List<ItemLevel> _itemLevels = new List<ItemLevel>();
    private DataUser _dataUser;

    public override void OnInit()
    {
        base.OnInit();
        _dataUser = DataManager.Instance.GetData<DataUser>();
        _itemLevels = new List<ItemLevel>();
        _btnUnlockAllLevel.onClick.AddListener(() =>
        {
            for(int i = 0; i < 10; i++)
            {
                _dataUser.UnlockIndex(i);
            }
            SpawnItem();
        });
    }


    public override void OnShow()
    {
        base.OnShow();
        SpawnItem();
    }

    public void SpawnItem()
    {
        if(_itemLevels.Count > 0)
        {
            int index = 0;
            foreach (var item in _itemLevels)
            {
                item.Init(index, _dataUser.DataSave.lockIndex.Contains(index));
                index++;
            }
        }
        else
        {
            for (int i = 0; i < 10; i++)
            {
                ItemLevel temp = PoolManager.Instance.SpawnObject(_itemLevelTrans,Vector3.zero,Quaternion.identity, _content).GetComponent<ItemLevel>();
                temp.Init(i, _dataUser.DataSave.lockIndex.Contains(i));
                _itemLevels.Add(temp);
            }
        }
    }


    public void NextLevel(int id)
    {
        OnHide();
        GameController.Instance.InitBattle(id,() =>
        {
            OnHide();
            CameraManager.Instance.PlayIntro(() =>
            {
                // countdown
                UIManager.Instance.ShowScreen<TouchController>();
            });
        });
    }

    public void Notification()
    {

    }
}
