using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomScript : EnemyBaseScript {

    [SerializeField] private EnemyCrush _mushroomEnemyCrushScript;

    private bool _wasCrushed;

    protected override void Start() {
        base.Start();
        _wasCrushed = false;
        _mushroomEnemyCrushScript.OnEnemyCrushed += _mushroomEnemyCrushScript_OnEnemyCrushed;
    }

    private void _mushroomEnemyCrushScript_OnEnemyCrushed(object sender, EventArgs e) {
        _wasCrushed = true;
        _mushroomEnemyCrushScript.gameObject.SetActive(false);
        Damage(_mySO.health);
    }

    protected override void Attack()
    {

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

        if (_wasCrushed) {
            _myVisual.Crushed();
            return true;
        }

        _myVisual.PlayDeadAnim();
        return true;
    }
}
