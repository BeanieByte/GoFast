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

    public event EventHandler OnEnemyHitAnimStarted;
    public event EventHandler OnEnemyHitAnimStopped;

    private void Awake() {
        _myLogicScript = GetComponentInParent<EnemyBaseScript>();
        _myAnimator = GetComponent<Animator>();
    }

    protected virtual void Start() { 
    
    }

    public void Flip() {
        transform.Rotate(0f, 180f, 0f);
    }

    public void PlayHitAnim() {
        _myAnimator.SetTrigger(hit_CONST);
        OnEnemyHitAnimStarted?.Invoke(this, EventArgs.Empty);
    }

    private void StopHitAnim() {
        OnEnemyHitAnimStopped?.Invoke(this, EventArgs.Empty);
    }

    public virtual void PlayDeadAnim() {
        _myAnimator.SetTrigger(die_CONST);
    }

    public void Die() {
        EnemyManager.Instance.IncreaseKilledEnemiesCounter();
        Destroy(_myLogicScript.gameObject);
    }

    public void DisableTouchAttackTrigger() {
        _myLogicScript.CanIWalk(false);
        _myLogicScript.SetTouchAttackTrigger(false);
    }

    public void EnableTouchAttackTrigger() {
        _myLogicScript.CanIWalk(true);
        _myLogicScript.SetTouchAttackTrigger(true);
        StopHitAnim();
    }

    public virtual void Crushed() {
    }
}
