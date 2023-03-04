using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class StatusBar : MonoBehaviour
{
    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    private void Update()
    {
        UpdatePosition();
        SetSizeOnce();
    }

    private void SetSizeOnce()
    {
        if (initSize)
        {
            return;
        }

        Vector2 size = follow.sizeDelta;
        if (size.x > 0f && size.y > 0f)
        {
            Vector2 a = Utils.WorldToScreenLocalPoint(follow.position, transform.parent as RectTransform, Camera.main);
            Vector2 b = Utils.WorldToScreenLocalPoint(follow.position + new Vector3(size.x, size.y), transform.parent as RectTransform, Camera.main);

            RectTransform rt = slider.transform as RectTransform;
            //rt.sizeDelta = b - a;
            Vector2 sz = b - a;
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, sz.x);
            rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, sz.y);
            initSize = true;
        }
    }

    private void UpdatePosition()
    {
        slider.transform.position = WorldToScreenPoint(_follow.transform.position);
    }

    private Vector3 WorldToScreenPoint(Vector3 pos)
    {
        switch (_canvas.renderMode)
        {
            case RenderMode.ScreenSpaceOverlay:
                return Utils.WorldToScreenPointInScreenSpaceOverlayMode(pos, Camera.main);
            case RenderMode.ScreenSpaceCamera:
                return Utils.WorldToScreenPointInScreenSpaceCameraMode(pos, transform.parent as RectTransform, Camera.main);
        }
        return Vector3.zero;
    }

    public float value
    {
        get => slider.value;
        set => slider.value = value;
    }

    public void SetValue(int value, int maxValue)
    {
        slider.value = value * 1.0f / maxValue;
    }

    public Canvas canvas
    {
        get => _canvas;
        set => _canvas = value;
    }

    public RectTransform follow
    {
        get => _follow;
        set => _follow = value;
    }

    [SerializeField] private Canvas _canvas;
    [SerializeField] private RectTransform _follow;

    private Slider slider;
    private bool initSize;
}
