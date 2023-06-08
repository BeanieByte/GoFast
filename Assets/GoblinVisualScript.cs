using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinVisualScript : EnemyVisualBaseScript 
{


    public void OnAttackStarted() {
        EnableAttackTrigger();
        _myLogicScript.SetPlayerRadiusCheckTrigger(false);
    }

    public void OnAttackFinished() {
        DisableAttackTrigger();
        _myLogicScript.SetPlayerRadiusCheckTrigger(true);
    }

    public override void DisableTouchAttackTrigger() {
        _myLogicScript.SetTouchAttackTrigger(false);
    }

    public override void EnableTouchAttackTrigger() {
        _myLogicScript.SetTouchAttackTrigger(true);
        StopHitAnim();
    }
}
