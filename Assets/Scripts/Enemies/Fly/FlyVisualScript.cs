using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyVisualScript : EnemyVisualBaseScript
{
    
    public event EventHandler OnEnemyAttackAnimStarted;
    public event EventHandler OnEnemyAttackAnimStopped;

    public override void Attack() {
        base.Attack();
        OnAttackStarted();
    }

    private void OnAttackStarted() {
        OnEnemyAttackAnimStarted?.Invoke(this, EventArgs.Empty) ;
        
    }

    public void OnAttackFinished() {
        OnEnemyAttackAnimStopped?.Invoke(this, EventArgs.Empty);
    }
}
