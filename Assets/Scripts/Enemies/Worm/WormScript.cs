using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormScript : EnemyBaseScript
{
    protected override void Start() {
        _isOriginallyFacingRight = true;
        CanIWalk(true);
        base.Start();
    }
}
