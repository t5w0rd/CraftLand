using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;
using UnityEngine.Serialization;

[AddComponentMenu("Input/TGAMEPLAY On-Screen Stick")]
public class OnScreenStick : OnScreenControl, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData == null)
            throw new System.ArgumentNullException(nameof(eventData));

        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out m_PointerDownPos);

        stickMoveRange.localPosition = m_PointerDownPos;
        if (autoHide)
        {
            SetStickVisible(true);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData == null)
            throw new System.ArgumentNullException(nameof(eventData));

        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out var position);
        var delta = position - m_PointerDownPos;

        delta = Vector2.ClampMagnitude(delta, movementRange);
        stickButton.anchoredPosition = m_StartPos + (Vector3)delta;

        // newPos接近1时会丢消息
        var newPos = new Vector2(delta.x * 0.9f / movementRange, delta.y * 0.9f / movementRange);
        SendValueToControl(newPos);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        stickButton.anchoredPosition = m_StartPos;
        SendValueToControl(Vector2.zero);

        stickMoveRange.localPosition = origRangePos;
        if (autoHide)
        {
            SetStickVisible(false);
        }
    }

    private void Start()
    {
        rangeCg = stickMoveRange.GetComponent<CanvasGroup>();
        origRangeCgAlpha = rangeCg.alpha;
        origRangePos = stickMoveRange.localPosition;
        if (autoMovementRange)
        {
            m_MovementRange = stickMoveRange.rect.width / 2;
        }
        if (autoHide)
        {
            SetStickVisible(false);
        }

        m_StartPos = stickButton.anchoredPosition;
    }

    public float movementRange
    {
        get => m_MovementRange;
        set => m_MovementRange = value;
    }

    public void SetStickVisible(bool value)
    {
        rangeCg.alpha = value ? origRangeCgAlpha : 0f;
    }

    [SerializeField] private RectTransform stickMoveRange;
    [SerializeField] private RectTransform stickButton;
    [SerializeField] private bool autoHide;

    private CanvasGroup rangeCg;
    private float origRangeCgAlpha;
    private Vector2 origRangePos;

    [FormerlySerializedAs("movementRange")]
    [SerializeField]
    private float m_MovementRange = 50;
    [SerializeField] private bool autoMovementRange;

    [InputControl(layout = "Vector2")]
    [SerializeField]
    private string m_ControlPath;

    private Vector3 m_StartPos;
    private Vector2 m_PointerDownPos;

    protected override string controlPathInternal
    {
        get => m_ControlPath;
        set => m_ControlPath = value;
    }
}
