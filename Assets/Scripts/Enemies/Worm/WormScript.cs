using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormScript : EnemyBaseScript
{
    protected override void Awake() {
        base.Awake();
        _isOriginallyFacingRight = true;
    }

    protected override void Attack() {

    }

    public override void Damage(int attackPower) {
        _currentHealth -= attackPower;
        DeadCheck();
    }

    public override bool DeadCheck() {
        if (_currentHealth > 0) {
            _myVisual.PlayHitAnim();
            return false;
        }

        _myRigidBody.constraints = RigidbodyConstraints2D.FreezePosition;

        _myVisual.PlayDeadAnim();
        return true;
    }
}
