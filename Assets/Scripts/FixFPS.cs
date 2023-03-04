using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixFPS : MonoBehaviour
{
    public int FPS = 60;

    void Awake() {
        Application.targetFrameRate = FPS;
    }
}
