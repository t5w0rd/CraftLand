using System.Collections;
using System.Collections.Generic;
using TGamePlay.TBattle;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwordAttackMethod : AttackMethod
{
    protected enum State
    {
        Wait,
        Attack1,
        Attack2,
        Attack3,
        Spin
    }

    public override void CheckState(UnitControllerTD unit, float deltaTime)
    {
        if ((unit.state == UnitControllerTDState.Idle || unit.state == UnitControllerTDState.Moving) && waitTimeLeft >= 0f)
        {
            //Debug.Log(unit.state);
            waitTimeLeft -= deltaTime;
            if (waitTimeLeft < 0f)
            {
                state = State.Wait;
                Debug.Log("Wait");
            }
        }
    }

    public override bool OnAttack(UnitControllerTD unit, InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed)
        {
            return false;
        }

        if (unit.state != UnitControllerTDState.Idle && unit.state != UnitControllerTDState.Moving && unit.state != UnitControllerTDState.Attacking)
        {
            return false;
        }

        switch (state)
        {
            case State.Wait:
                unit.anim.SetTrigger(attack1Hash);
                return true;

            case State.Attack1:
                unit.anim.SetTrigger(attack2Hash);
                return true;

            case State.Attack2:
                unit.anim.SetTrigger(attack3Hash);
                return true;

            case State.Attack3:
                if (unit.state == UnitControllerTDState.Idle || unit.state == UnitControllerTDState.Moving)
                {
                    unit.anim.SetTrigger(attack1Hash);
                    return true;
                }
                break;

            case State.Spin:
                unit.anim.SetTrigger(attack1Hash);
                return true;
        }
        return false;
    }

    public override bool OnCast(UnitControllerTD unit, InputAction.CallbackContext context, string name)
    {
        if (context.phase != InputActionPhase.Performed)
        {
            return false;
        }

        Debug.Log(unit.state);

        if (unit.state != UnitControllerTDState.Idle && unit.state != UnitControllerTDState.Moving)
        {
            return false;
        }

        switch (name)
        {
            case "Cast1":
                unit.anim.SetTrigger(spinHash);
                return true;
        }
        
        return false;
    }

    public override void OnEnterState(UnitControllerTD unit, string stateName)
    {
        DamageInfo damageInfo;
        SwordTD sword = unit.rightWeapon as SwordTD;
        switch (stateName)
        {
            case "Attack1":
                state = State.Attack1;
                damageInfo = unit.unit.Fight.Attack();
                damageInfo.Value = (int)(damageInfo.Value * attack1Rate);
                sword.damageInfo = damageInfo;
                waitTimeLeft = waitNextAttack;
                Debug.Log("Attack1");
                break;

            case "Attack2":
                state = State.Attack2;
                damageInfo = unit.unit.Fight.Attack();
                damageInfo.Value = (int)(damageInfo.Value * attack2Rate);
                sword.damageInfo = damageInfo;
                waitTimeLeft = waitNextAttack;
                Debug.Log("Attack2");
                break;

            case "Attack3":
                state = State.Attack3;
                damageInfo = unit.unit.Fight.Attack();
                damageInfo.Value = (int)(damageInfo.Value * attack3Rate);
                sword.damageInfo = damageInfo;
                waitTimeLeft = waitNextAttack;
                Debug.Log("Attack3");
                break;

            case "PreSpin":
                state = State.Spin;
                damageInfo = unit.unit.Fight.Attack();
                damageInfo.Value = (int)(damageInfo.Value * spinRate);  // TODO: 改变机制，目前旋风斩所有斩击都将享受概率OnAttack攻击提升加成
                sword.damageInfo = damageInfo;
                Debug.Log("Spin");
                break;
        }
    }

    [SerializeField] private float attack1Rate = 0.9f;
    [SerializeField] private float attack2Rate = 1.0f;
    [SerializeField] private float attack3Rate = 1.2f;
    [SerializeField] private float spinRate = 0.6f;

    [SerializeField] private float waitNextAttack = 1f;

    protected static int attack1Hash = Animator.StringToHash("SwordAttack1");
    protected static int attack2Hash = Animator.StringToHash("SwordAttack2");
    protected static int attack3Hash = Animator.StringToHash("SwordAttack3");
    protected static int spinHash = Animator.StringToHash("SwordSpin");

    private State state = State.Wait;
    private float waitTimeLeft;
}
