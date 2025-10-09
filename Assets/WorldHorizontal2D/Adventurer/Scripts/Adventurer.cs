using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerState
{
    Static,
    Idle,
    Moving,
    PreJump,
    Jumping,
    Falling,
    Attacking
}

public class Adventurer : MonoBehaviour
{
    [SerializeField] private GameObject spriteObject;

    [SerializeField] private LayerMask ground;

    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float moveAcceleration = 120f;
    [SerializeField] private float moveDeAcceleration = 60f;

    [SerializeField] private float jumpVelocityY = 30f;
    [SerializeField] private float fallMaxVelocityY = 30f;

    private float inputX;
    private float curVelocityX;
    private float waitJump;

    private SpriteRenderer sr;
    private Rigidbody2D rb;
    private Animator anim;
    private CircleCollider2D cc;

    [SerializeField] private PlayerState state;  // TODO: remove

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = spriteObject.GetComponent<SpriteRenderer>();
        anim = spriteObject.GetComponent<Animator>();
        cc = spriteObject.GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        state = PlayerState.Idle;
    }

    private void Update()
    {
        CalcMoveVelocity();

        if ((state == PlayerState.Idle || state == PlayerState.Moving || state == PlayerState.PreJump || state == PlayerState.Jumping) && !onGround && rb.linearVelocity.y < 0f)
        {
            state = PlayerState.Falling;
            anim.SetTrigger("Fall");
            Debug.Log("Fall");
            return;
        }

        if (state == PlayerState.Falling && onGround)
        {
            state = PlayerState.Idle;
            anim.SetTrigger("Land");
            Debug.Log("Land");
            return;
        }

        if (state == PlayerState.Idle)
        {
            if (inputX != 0)
            {
                state = PlayerState.Moving;
                anim.SetTrigger("Move");
                Debug.Log("Move");
                return;
            }
        }

        if (state == PlayerState.Moving)
        {
            if (inputX == 0f)
            {
                state = PlayerState.Idle;
                anim.SetTrigger("Idle");
                Debug.Log("Idle");
                return;
            }
        }
    }

    private void CalcMoveVelocity()
    {
        if (inputX == 0f)// || state == PlayerState.PreJump
        {
            curVelocityX = Mathf.MoveTowards(curVelocityX, 0, moveDeAcceleration * Time.deltaTime);
        }
        else
        {
            curVelocityX = Mathf.Clamp(curVelocityX + inputX * moveAcceleration * Time.deltaTime, -moveSpeed, moveSpeed);
        }

        if (state == PlayerState.Falling)
        {
            rb.linearVelocity = new(curVelocityX, Mathf.Clamp(rb.linearVelocity.y, -fallMaxVelocityY, 0));
        }
        else
        {
            rb.linearVelocity = new(curVelocityX, rb.linearVelocity.y);
        }
    }

    private bool onGround => cc.IsTouchingLayers(ground);

    //public IEnumerator WaitForIdle()
    //{
    //    for (var state = anim.GetCurrentAnimatorStateInfo(0);
    //        !state.IsName(idleAnimation);
    //        state = anim.GetCurrentAnimatorStateInfo(0))
    //    {
    //        yield return null;
    //    }
    //}

    public void OnJumpEventFrame()
    {
        if (state != PlayerState.PreJump)
        {
            return;
        }

        state = PlayerState.Jumping;
        rb.linearVelocity = new(rb.linearVelocity.x, jumpVelocityY);
        Debug.Log("Jump");
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        float inX = context.ReadValue<Vector2>().x;
        if (inX != 0 && inX != inputX)
        {
            sr.flipX = inX < 0f;
        }
        inputX = inX;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (state != PlayerState.Idle && state != PlayerState.Moving)
        {
            return;
        }

        switch (context.phase)
        {
            case InputActionPhase.Performed:
                state = PlayerState.PreJump;
                anim.SetTrigger("Jump");
                Debug.Log("PreJump");
                break;
        }
    }
}
