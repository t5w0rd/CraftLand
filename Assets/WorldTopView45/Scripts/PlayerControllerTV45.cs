using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerTV45 : MonoBehaviour
{
    public float speed;

    private Rigidbody2D rb;
    private Animator anim;
    private Vector2 inputMove;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        rb.linearVelocity = speed * Time.deltaTime * inputMove;

        bool isMoving = inputMove != Vector2.zero;
        anim.SetBool("IsMoving", isMoving);
        if (isMoving)
        {
            anim.SetFloat("InputX", inputMove.x);
            anim.SetFloat("InputY", inputMove.y);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        inputMove = context.ReadValue<Vector2>().normalized;
    }
}
