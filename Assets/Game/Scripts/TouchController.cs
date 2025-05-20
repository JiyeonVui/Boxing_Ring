using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchController : MonoBehaviour
{
    //public PlayerBoxer boxer;
    public bool isHolding = false;

    [SerializeField] private Button _btnHold;
    [SerializeField] private Button _btnAttack;

    private void Start()
    {
        _btnAttack.onClick.AddListener(() =>
        {
            if (isHolding)
            {
                return;
            }

            GameController.attackAction?.Invoke();
        });

    }

    public void OnHold()
    {
        isHolding = true;
        GameController.holdAction?.Invoke(isHolding);
    }

    public void OnReleaseHold()
    {
        isHolding = false;
        GameController.holdAction?.Invoke(isHolding);
    }

    void Update()
    {
        //if (Input.touchCount == 0)
        //{
        //    isMoving = false;
        //    return;
        //}

        //Touch touch = Input.GetTouch(0);

        //switch (touch.phase)
        //{
        //    case TouchPhase.Began:
        //        touchStartTime = Time.time;
        //        break;

        //    case TouchPhase.Stationary:

        //        if (!isMoving)
        //        {
        //            float holdDuration = Time.time - touchStartTime;

        //            if (holdDuration > touchTimeThreshold && !isHolding)
        //            {
        //                if (dynamicJoystick.Direction.magnitude < 0.1f)
        //                {
        //                    isHolding = true;
        //                    GameController.holdAction?.Invoke(isHolding);
        //                }
        //                else
        //                {
        //                    isHolding = false;
        //                    //GameController.holdAction?.Invoke(isHolding);
        //                }
        //            }
        //        }

        //        break;

        //    case TouchPhase.Moved:
        //        //Debug.Log("MoveThreshold " + dynamicJoystick.MoveThreshold);
        //        if (dynamicJoystick.Direction.magnitude > 0.1f)
        //        {
        //            isMoving = true;
        //            Debug.Log("MoveThreshold " + dynamicJoystick.MoveThreshold);
        //        }
        //        break;

        //    case TouchPhase.Ended:

        //        if (!isMoving && !isHolding)
        //        {
        //            GameController.attackAction?.Invoke();
        //        }


        //        if(isHolding)
        //        {
        //            isHolding = false;
        //            GameController.holdAction?.Invoke(isHolding);
        //        }
        //        if (isMoving)
        //        {
        //            Debug.LogError("stop action moving");
        //            isMoving = false;
        //            GameController.stopAction?.Invoke();
        //        }
        //        break;
        //}

        //if (isMoving)
        //{
        //    Vector2 dir = dynamicJoystick.Direction * dynamicJoystick.MoveThreshold;
        //    if (dynamicJoystick.MoveThreshold < 0.25f)
        //    {
        //        dir = Vector2.zero;
        //    }
        //    GameController.movingAction?.Invoke(dir);
        //}
    }
}




