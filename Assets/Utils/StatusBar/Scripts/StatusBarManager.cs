using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusBarManager : MonoBehaviour
{
    public StatusBar CreateStatusBar(RectTransform follow)
    {
        StatusBar bar = Instantiate(prefab, transform);
        bar.canvas = canvas;
        bar.follow = follow;
        return bar;
    }

    [SerializeField] Canvas canvas;
    [SerializeField] StatusBar prefab;
}
