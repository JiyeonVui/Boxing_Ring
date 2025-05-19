using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DynamicJoystick : Joystick
{
    public float MoveThreshold { get { return moveThreshold; } set { moveThreshold = Mathf.Abs(value); } }

    [SerializeField] private float moveThreshold = 15;
    private bool isDragging = false;
    private Vector2 startPos;

    protected override void Start()
    {
        MoveThreshold = moveThreshold;
        base.Start();
        background.gameObject.SetActive(false);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        startPos = eventData.position;
        isDragging = false;
        background.gameObject.SetActive(false);
        //background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        //background.gameObject.SetActive(true);
        //base.OnPointerDown(eventData);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        float distance = Vector2.Distance(startPos, eventData.position);
        if (!isDragging && distance > MoveThreshold)
        {
            isDragging = true;

            background.anchoredPosition = ScreenPointToAnchoredPosition(startPos);
            background.gameObject.SetActive(true);
            base.OnPointerDown(eventData); // Kích hoạt joystick logic
        }

        if (isDragging)
        {
            base.OnDrag(eventData);
        }
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        background.gameObject.SetActive(false);
        base.OnPointerUp(eventData);
    }

    protected override void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
    {
        if (magnitude > moveThreshold)
        {
            Vector2 difference = normalised * (magnitude - moveThreshold) * radius;
            background.anchoredPosition += difference;
        }
        base.HandleInput(magnitude, normalised, radius, cam);
    }
}