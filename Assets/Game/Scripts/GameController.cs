using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoSingleton<GameController>
{
    //[SerializeField] private TouchController _touchController;
    [SerializeField] private PlayerBoxer _playerBoxer;
    public PlayerBoxer playerBoxer => _playerBoxer;


    public static Action attackAction;
    public static Action<Vector2> movingAction;
    public static Action stopAction;
    public static Action<bool> holdAction;


    public void StartIntro()
    {

    }

}
