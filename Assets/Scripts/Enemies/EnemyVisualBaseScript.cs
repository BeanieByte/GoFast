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

    [SerializeField] private int _numberOfTimesHitAnimCanLoop = 3;
    private int _numberOfTimesHitAnimHasLooped;

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

    public virtual void Attack() {
        _myAnimator.SetTrigger(attack_CONST);
    }

    public void PlayHitAnim() {
        _numberOfTimesHitAnimHasLooped = 0;
        _myAnimator.SetTrigger(hit_CONST);
        OnEnemyHitAnimStarted?.Invoke(this, EventArgs.Empty);
    }

    public void StopHitAnim() {
        _numberOfTimesHitAnimHasLooped++;
        if (_numberOfTimesHitAnimHasLooped < _numberOfTimesHitAnimCanLoop) { return; }
        OnEnemyHitAnimStopped?.Invoke(this, EventArgs.Empty);
    }

    public virtual void PlayDeadAnim() {
        _myAnimator.SetTrigger(die_CONST);
    }

    public void Die() {
        Destroy(_myLogicScript.gameObject);
    }

    public virtual void Crushed() {
    }

    public void SetIsWalkingBoolTrue() {
        _myAnimator.SetBool(isRunning_CONST, true);
    }

    public void SetIsWalkingBoolFalse() {
        _myAnimator.SetBool(isRunning_CONST, false);
    }
}
