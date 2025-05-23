using DG.Tweening;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoSingleton<GameController>
{
    public enum GameMode
    {
        OneVsOne,
        OneVsThree

    }
    public GameMode gameMode = GameMode.OneVsOne;
    public PlayerBoxer playerBoxer;
    public Enemy enemy;


    public static Action attackAction;
    public static Action<Vector2> movingAction;
    public static Action stopAction;
    public static Action<bool> holdAction;
    private bool _isBattleEnd = false;

    [SerializeField] private Transform playerPos;
    [SerializeField] private Transform enemyPos;

    [SerializeField] private Transform _playerTrans;
    [SerializeField] private Transform _enemyTrans;

    [SerializeField] private Image _hpEnemy;
    [SerializeField] private Image _hpPlayer;

    [SerializeField] private GameObject _hpPlayerObj;
    [SerializeField] private GameObject _hpEnemyObj;

    [SerializeField] private List<Enemy> _enemyList = new List<Enemy>();

    private int _currentLevel;

    public void InitBattle(int id, Action callback)
    {
        _isBattleEnd = false;
        if(gameMode == GameMode.OneVsOne)
        {
            InitSolo(id, callback);
        }
        else
        {
            InitMulti(callback);
        }
        _hpEnemyObj.SetActive(false);
        _hpPlayerObj.SetActive(false);
        _currentLevel = 0;
    }

    private void InitSolo(int level, Action callback = null)
    {
        playerBoxer = PoolManager.Instance.SpawnObject(_playerTrans, Vector3.zero, Quaternion.identity, playerPos).GetComponent<PlayerBoxer>();
        enemy = PoolManager.Instance.SpawnObject(_enemyTrans, Vector3.zero, Quaternion.identity, enemyPos).GetComponent<Enemy>();
        playerBoxer.transform.localPosition = Vector3.zero;
        enemy.transform.localPosition = Vector3.zero;
        enemy.transform.localRotation = Quaternion.identity;
        playerBoxer.transform.localRotation = Quaternion.identity;

        enemy.Init(level);
        playerBoxer.Init();

        callback?.Invoke();

        _hpEnemy.fillAmount = 1f;
        _hpPlayer.fillAmount = 1f;
    }

    private void InitMulti(Action callback = null)
    {

    }

    public void BattleStart()
    {
        enemy.Fight();
        _hpEnemyObj.SetActive(true);
        _hpPlayerObj.SetActive(true);
    }

    public void SwitchPlayer(int id)
    {

    }

    public void BattleEnd()
    {
        PoolManager.Instance.DespawnObject(playerBoxer.transform);
        PoolManager.Instance.DespawnObject(enemy.transform);
        playerBoxer = null;
        enemy = null;
    }

    public void UpdateHpEnemy(float hp)
    {
        _hpEnemy.fillAmount = hp;
    }

    public void UpdateHpPlayer(float hp)
    {
        _hpPlayer.fillAmount = hp;
    }

    public void CheckBattleResult(bool isWin)
    {
        if (_isBattleEnd)
        {
            return;
        }

        _isBattleEnd = true;
        if (isWin)
        {
            playerBoxer.OnWin();
            int level = _currentLevel + 1;
            DataManager.Instance.GetData<DataUser>().UnlockIndex(level);
        }
        else
        {
            enemy.OnWin();
        }

        UIManager.Instance.GetScreen<TouchController>().OnHide();

        DOVirtual.DelayedCall(2f, () =>
        {
            if (isWin)
            {
                UIManager.Instance.ShowPopup<PopUpVictory>();
            }
            else
            {
                UIManager.Instance.ShowPopup<PopUpLose>();
            }
        });
    }
}
