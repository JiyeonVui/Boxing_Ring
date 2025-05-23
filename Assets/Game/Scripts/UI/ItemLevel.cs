using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemLevel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _numberId;
    [SerializeField] private Button _btnOnClick;
    [SerializeField] private Button _btnLock;
    private int _idLevel;

    public void Init(int id, bool isUnLock)
    {
        _numberId.text = id.ToString();
        _idLevel = id;

        if (!isUnLock)
        {
            _btnOnClick.gameObject.SetActive(false);
            _btnLock.gameObject.SetActive(true);
        }
        else
        {
            _btnOnClick.gameObject.SetActive(true);
            _btnLock.gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        _btnOnClick.onClick.AddListener(() =>
        {
            UIManager.Instance.GetScreen<ScreenLevel>().NextLevel(_idLevel);
        });

        _btnLock.onClick.AddListener(() =>
        {
            UIManager.Instance.GetScreen<ScreenLevel>().Notification();
        });
    }
}
