using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    [SerializeField] private Adventurer ctrl;
    public void OnJumpFrameEvent()
    {
        ctrl.OnJumpEventFrame();
    }
}
