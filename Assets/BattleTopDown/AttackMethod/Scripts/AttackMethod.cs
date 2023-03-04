using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class AttackMethod
{
    public virtual void CheckState(UnitControllerTD unit, float deltaTime) { }
    public virtual bool OnAttack(UnitControllerTD unit, InputAction.CallbackContext context) { return false; }
    public virtual bool OnCast(UnitControllerTD unit, InputAction.CallbackContext context, string name) { return false; }
    public virtual void OnEnterState(UnitControllerTD unit, string stateName) { }
}
