using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoSingleton<GameController>
{
    public enum GameMode
    {
        OneVsOne,
        OneVsThree,
        ThreeVsThree
    }

    public PlayerBoxer playerBoxer;
    public Enemy enemy;


    public static Action attackAction;
    public static Action<Vector2> movingAction;
    public static Action stopAction;
    public static Action<bool> holdAction;

    [SerializeField] private Transform playerPos;
    [SerializeField] private Transform enemyPos;

    [SerializeField] private Transform _playerTrans;
    [SerializeField] private Transform _enemyTrans;


    [SerializeField] private List<PlayerBoxer> _listPlayerBoxer = new List<PlayerBoxer>();

    public void InitSolo(Action callback = null)
    {
        playerBoxer = PoolManager.Instance.SpawnObject(_playerTrans, Vector3.zero, Quaternion.identity, playerPos).GetComponent<PlayerBoxer>();
        enemy = PoolManager.Instance.SpawnObject(_enemyTrans, Vector3.zero, Quaternion.identity, enemyPos).GetComponent<Enemy>();
        playerBoxer.transform.localPosition = Vector3.zero;
        enemy.transform.localPosition = Vector3.zero;
        enemy.transform.localRotation = Quaternion.identity;
        playerBoxer.transform.localRotation = Quaternion.identity;

        enemy.Init();
        playerBoxer.Init();

        callback?.Invoke();
    }

    public void BattleStart()
    {
        enemy.Fight();
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
}
