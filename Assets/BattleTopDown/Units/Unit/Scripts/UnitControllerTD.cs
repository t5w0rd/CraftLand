using System.Collections;
using System.Collections.Generic;
using TGamePlay.TBattle;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;


public enum UnitControllerTDState
{
    Idle,
    Moving,
    Attacking,
    Casting,
}

public class UnitControllerTD : MonoBehaviour
{
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        rightWeaponBc = rightWeaponPoint.GetComponent<BoxCollider2D>();
        playerInput = GetComponent<PlayerInput>();
        playerInput.enabled = false;
    }

    private void Update()
    {
        CalcMoveVelocity();
        CalcRotation();

        attackMethod?.CheckState(this, Time.deltaTime);

        if (state == UnitControllerTDState.Idle)
        {
            if (inputMove != Vector2.zero)
            {
                state = UnitControllerTDState.Moving;
                Debug.Log("Move");
                return;
            }
        }

        if (state == UnitControllerTDState.Moving)
        {
            if (inputMove == Vector2.zero)
            {
                state = UnitControllerTDState.Idle;
                Debug.Log("Idle");
                return;
            }
        }
    }

    private void CalcMoveVelocity()
    {
        if (inputMove == Vector2.zero || state == UnitControllerTDState.Attacking)
        {
            rb.linearVelocity = Vector2.MoveTowards(rb.linearVelocity, Vector2.zero, moveDeAcceleration * Time.deltaTime);
        }
        else
        {
            rb.linearVelocity = Vector2.ClampMagnitude(rb.linearVelocity + inputMove * moveAcceleration * Time.deltaTime, moveSpeed);
        }
    }

    private void CalcRotation()
    {
        if (inputMove == Vector2.zero || state == UnitControllerTDState.Attacking)
        {
            return;
        }
        Quaternion to = Quaternion.LookRotation(Vector3.forward, inputMove);
        spriteObject.transform.rotation = Quaternion.RotateTowards(spriteObject.transform.rotation, to, Time.deltaTime * rotateSpeed);
    }

    public void EnablePlayerInput(bool value)
    {
        playerInput.enabled = value;
    }

    public void EquipLeft(WeaponTD prefab)
    {
    }

    public void EquipRight(WeaponTD prefab)
    {
        if (rightWeapon != null)
        {
            Destroy(rightWeapon.gameObject);
            unit.Fight?.ChangeATKCoefB(-rightWeapon.atk);
            rightWeapon.unitID = 0;
        }

        rightWeapon = Instantiate(prefab, rightWeaponPoint);
        anim.SetFloat(WeaponTD.speedRateHash, rightWeapon.speedRate);
        if (rightWeapon.hitRect != null)
        {
            rightWeaponBc.offset = rightWeapon.hitRect.anchoredPosition;
            rightWeaponBc.size = rightWeapon.hitRect.sizeDelta;
        }
        else
        {
            rightWeaponBc.offset = Vector2.zero;
            rightWeaponBc.size = new Vector2(0.1f, 0.1f);
        }
        unit.Fight?.ChangeATKCoefB(rightWeapon.atk);
        rightWeapon.unitID = unit.ID;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Weapon"))
        {
            return;
        }

        Debug.Log($"{collision.gameObject.name} {gameObject.name}");

        WeaponTD weapon = collision.GetComponentInChildren<WeaponTD>();
        if (weapon == null)
        {
            Debug.Log("null weapon");
            return;
        }

        OnDamage?.Invoke(this, weapon);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        inputMove = context.ReadValue<Vector2>().normalized;
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (attackMethod != null && attackMethod.OnAttack(this, context))
        {
            state = UnitControllerTDState.Attacking;
            Debug.Log("Attacking");
        }
    }

    public void OnCast1(InputAction.CallbackContext context)
    {
        if (attackMethod != null && attackMethod.OnCast(this, context, "Cast1"))
        {
            state = UnitControllerTDState.Casting;
            Debug.Log("Casting");
        }
    }

    public void OnFrameEvent(string param)
    {
        string[] split = param.Split('/');
        switch (split[0])
        {
            case "Idle":
                state = UnitControllerTDState.Idle;
                Debug.Log("Idle");
                return;

            case "Attack":
                attackMethod.OnEnterState(this, split[1]);
                return;

            case "Cast":
                attackMethod.OnEnterState(this, split[1]);
                return;
        }
    }

    public event UnityAction<UnitControllerTD, WeaponTD> OnDamage;

    [SerializeField] private GameObject spriteObject;
    [SerializeField] private Transform rightWeaponPoint;
    public RectTransform hpBarRect;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float moveAcceleration = 120f;
    [SerializeField] private float moveDeAcceleration = 60f;

    [SerializeField] private float rotateSpeed = 1f;

    public AttackMethod attackMethod { get; set; }

    private Rigidbody2D rb;
    internal Animator anim;
    internal WeaponTD rightWeapon;
    private BoxCollider2D rightWeaponBc;
    private PlayerInput playerInput;

    internal UnitControllerTDState state = UnitControllerTDState.Idle;

    private Vector2 inputMove;

    [HideInInspector] public Unit unit;
}
