using System.Collections;
using System.Collections.Generic;
using TGamePlay.TAction;
using UnityEngine;

/// <summary>
/// <para>单位控制器</para>
/// </summary>
public class UnitControllerAH2D : UnitController
{
    public float speed = 100f;

    protected Rigidbody2D rb;

    void Awake()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        var inHor = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(inHor * speed * Time.deltaTime, 0.0f);
    }
}
