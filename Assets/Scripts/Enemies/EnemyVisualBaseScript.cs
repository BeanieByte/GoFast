using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVisualBaseScript : MonoBehaviour
{
    protected EnemyBaseScript _myLogicScript;
    protected Animator _myAnimator;

    private const string hit_CONST = "hit";
    protected const string die_CONST = "die";
    protected const string attack_CONST = "attack";
    protected const string isRunning_CONST = "isRunning";

    public event EventHandler OnEnemyHitAnimStarted;
    public event EventHandler OnEnemyHitAnimStopped;

    private void Awake() {
        _myLogicScript = GetComponentInParent<EnemyBaseScript>();
        _myAnimator = GetComponent<Animator>();
    }

    protected virtual void Start() { 
    
    }

    protected virtual void FixedUpdate() { 
        
    }

    public void Flip() {
        transform.Rotate(0f, 180f, 0f);
    }

    public void Attack() {
        _myAnimator.SetTrigger(attack_CONST);
    }

    public void PlayHitAnim() {
        _myAnimator.SetTrigger(hit_CONST);
        OnEnemyHitAnimStarted?.Invoke(this, EventArgs.Empty);
    }

    protected void StopHitAnim() {
        OnEnemyHitAnimStopped?.Invoke(this, EventArgs.Empty);
    }

    public virtual void PlayDeadAnim() {
        _myAnimator.SetTrigger(die_CONST);
    }

    public void Die() {
        EnemyManager.Instance.IncreaseKilledEnemiesCounter();
        Destroy(_myLogicScript.gameObject);
    }

    public virtual void DisableTouchAttackTrigger() {
        _myLogicScript.CanIWalk(false);
        _myLogicScript.SetTouchAttackTrigger(false);
    }

    public virtual void EnableTouchAttackTrigger() {
        _myLogicScript.CanIWalk(true);
        _myLogicScript.SetTouchAttackTrigger(true);
        StopHitAnim();
    }

    public void EnableAttackTrigger() {
        _myLogicScript.SetAttackTrigger(true);
    }

    public void DisableAttackTrigger() {
        _myLogicScript.SetAttackTrigger(false);
    }

    public virtual void Crushed() {
    }

    public virtual void DisableAllCollidersOnDeath() {
        DisableTouchAttackTrigger();
        DisableAttackTrigger();
    }

    public void SetIsWalkingBoolTrue() {
        _myAnimator.SetBool(isRunning_CONST, true);
    }

    public void SetIsWalkingBoolFalse() {
        _myAnimator.SetBool(isRunning_CONST, false);
    }

}
