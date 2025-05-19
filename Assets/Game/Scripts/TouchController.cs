using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchController : MonoBehaviour
{
    private bool isMoving = false;
    //public PlayerBoxer boxer;
    public float swipeThreshold = 20f;
    public DynamicJoystick dynamicJoystick;
    public Action<Vector2> movingAction;
    public float touchTimeThreshold = 0.5f; // nho hon tap lon hon hold
    public float touchStartTime = 0f;
    public bool isHolding = false;
    void Update()
    {
        if (Input.touchCount == 0)
        {
            isMoving = false;
            return;
        }

        Touch touch = Input.GetTouch(0);

        switch (touch.phase)
        {
            case TouchPhase.Began:
                touchStartTime = Time.time;
                break;

            case TouchPhase.Stationary:

                if (!isMoving)
                {
                    float holdDuration = Time.time - touchStartTime;

                    if (holdDuration > touchTimeThreshold && !isHolding)
                    {
                        if (dynamicJoystick.Direction.magnitude < 0.1f)
                        {
                            isHolding = true;
                            GameController.holdAction?.Invoke(isHolding);
                        }
                        else
                        {
                            isHolding = false;
                            //GameController.holdAction?.Invoke(isHolding);
                        }
                    }
                }

                break;

            case TouchPhase.Moved:
                //Debug.Log("MoveThreshold " + dynamicJoystick.MoveThreshold);
                if (dynamicJoystick.Direction.magnitude > 0.1f)
                {
                    isMoving = true;
                    Debug.Log("MoveThreshold " + dynamicJoystick.MoveThreshold);
                }
                break;

            case TouchPhase.Ended:

                if (!isMoving && !isHolding)
                {
                    GameController.attackAction?.Invoke();
                }
                    

                if(isHolding)
                {
                    isHolding = false;
                    GameController.holdAction?.Invoke(isHolding);
                }
                if (isMoving)
                {
                    Debug.LogError("stop action moving");
                    isMoving = false;
                    GameController.stopAction?.Invoke();
                }
                break;
        }

        if (isMoving)
        {
            Vector2 dir = dynamicJoystick.Direction * dynamicJoystick.MoveThreshold;
            if (dynamicJoystick.MoveThreshold < 0.25f)
            {
                dir = Vector2.zero;
            }
            GameController.movingAction?.Invoke(dir);
        }
    }
}
