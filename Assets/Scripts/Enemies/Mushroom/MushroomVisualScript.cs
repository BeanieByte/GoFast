using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomVisualScript : EnemyVisualBaseScript
{
    private const string crushed_CONST = "crushed";

    public override void Crushed() {
        _myAnimator.SetTrigger(crushed_CONST);
    }
}
