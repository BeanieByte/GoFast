using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyVisualScript : EnemyVisualBaseScript
{
    private Vector3 _positionBeforeAttacking;

    public void OnAttackStarted() {
        _myLogicScript.CanIWalk(false);
        _positionBeforeAttacking = _myLogicScript.transform.position;
        DisableTouchAttackTrigger();
        EnableAttackTrigger();
        _myLogicScript.SetPlayerRadiusCheckTrigger(false);
        _myLogicScript.transform.position = new Vector3(_myLogicScript.PlayersCurrentXLocation(), _positionBeforeAttacking.y, _positionBeforeAttacking.z) ;
    }

    public void OnAttackFinished() {
        DisableAttackTrigger();
        _myLogicScript.transform.position = _positionBeforeAttacking;
        _myLogicScript.SetPlayerRadiusCheckTrigger(true);
        _myLogicScript.CanIWalk(true);
    }

    public override void DisableAllCollidersOnDeath() {
        base.DisableAllCollidersOnDeath();
    }
}
