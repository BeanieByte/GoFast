using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomScript : EnemyBaseScript {

    [SerializeField] private EnemyCrush _mushroomEnemyCrushScript;

    protected override void Start() {
        _isOriginallyFacingRight = true;
        CanIWalk(true);
        base.Start();
        _mushroomEnemyCrushScript.OnEnemyCrushed += _mushroomEnemyCrushScript_OnEnemyCrushed;
    }

    private void _mushroomEnemyCrushScript_OnEnemyCrushed(object sender, EventArgs e) {
        _mushroomEnemyCrushScript.gameObject.SetActive(false);
        Damage(_mySO.health);
    }
}
