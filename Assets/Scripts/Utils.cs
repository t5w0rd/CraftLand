using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Utils
{

    public static Vector3 WorldToScreenPointInScreenSpaceOverlayMode(Vector3 world, Camera camera)
    {
        return camera.WorldToScreenPoint(world);
    }

    public static Vector3 WorldToScreenPointInScreenSpaceCameraMode(Vector3 world, RectTransform screenParent, Camera camera)
    {
        var screen = camera.WorldToScreenPoint(world);
        RectTransformUtility.ScreenPointToWorldPointInRectangle(screenParent, screen, camera, out Vector3 rectPosition);
        return rectPosition;
    }

    public static Vector2 WorldToScreenLocalPoint(Vector3 world, RectTransform screenParent, Camera camera)
    {
        var screen = camera.WorldToScreenPoint(world);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(screenParent, screen, camera, out Vector2 rectPosition);
        return rectPosition;
    }
}
