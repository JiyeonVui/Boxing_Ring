using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class LevelSaveData
{
    [SerializeField]
    private int id;
    [SerializeField]
    private int enemyLeft;

    public int Id { get => id; set => id = value; }
    public int EnemyLeft { get => enemyLeft; set => enemyLeft = value; }
}

