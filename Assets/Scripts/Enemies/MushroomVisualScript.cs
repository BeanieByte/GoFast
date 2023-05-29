using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomVisualScript : EnemyVisualBaseScript
{
    private const string crushed_CONST = "crushed";

    protected override void Start() {
        base.Start();
    }

    public override void Crushed() {
        _myAnimator.SetTrigger(crushed_CONST);
    }
}
