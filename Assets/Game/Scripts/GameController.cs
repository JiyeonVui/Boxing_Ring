using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private TouchController _touchController;
    [SerializeField] private PlayerBoxer _playerBoxer;
    


    public static Action attackAction;
    public static Action<Vector2> movingAction;
    public static Action stopAction;
    public static Action<bool> holdAction;


    private static GameController _instance;

    public static GameController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameController>();
                if (_instance == null)
                {
                    Debug.LogError("No GameController instance found in the scene.");
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

}
