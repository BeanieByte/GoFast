using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoundsBaseScript : MonoBehaviour
{
    protected EnemyBaseScript _myLogicScript;

    private float _footstepsCurrentTime;
    private float _footstepsMaxTime;
    private float _footstepsDefaultMaxTime = 0.4f;

    private float _footstepsBurningMaxTime = 0.1f;
    private float _footstepsParalyzedMaxTime = 0.3f;
    private float _footstepsFrozenMaxTime = 0.4f;
    private float _footstepsSlimedMaxTime = 0.25f;

    private bool _enemyHasStatus;
    private float _statusConditionCurrentTime;
    private float _statusConditionMaxTime;
    private float _statusConditionDefaultMaxTime = 0.5f;

    private void Awake() {
        _myLogicScript = GetComponentInParent<EnemyBaseScript>();
    }

    private void Start() {

        _statusConditionMaxTime = _statusConditionDefaultMaxTime;
        _statusConditionCurrentTime = _statusConditionMaxTime - 0.01f;
        _enemyHasStatus = false;

        _footstepsMaxTime = _footstepsDefaultMaxTime;

        if (_myLogicScript.CanBurn()){
            _enemyHasStatus = true;
            _footstepsMaxTime = _footstepsBurningMaxTime;
        }
        if (_myLogicScript.CanParalyze()) {
            _enemyHasStatus = true;
            _footstepsMaxTime = _footstepsParalyzedMaxTime;
        }
        if (_myLogicScript.CanFreeze()) {
            _enemyHasStatus = true;
            _footstepsMaxTime = _footstepsFrozenMaxTime;
        }
        if (_myLogicScript.CanPoison()) {
            _enemyHasStatus = true;
        }
        if (_myLogicScript.CanSlime()) {
            _enemyHasStatus = true;
            _footstepsMaxTime = _footstepsSlimedMaxTime;
        }

        _footstepsCurrentTime = _footstepsMaxTime;

    }
    
    private void FixedUpdate() {

        if (_myLogicScript.IsEnemyWalking()) {
            if (_footstepsCurrentTime == _footstepsMaxTime) {
                PlayMyFootstepSound();
            }

            _footstepsCurrentTime -= Time.deltaTime;

            if (_footstepsCurrentTime <= 0) {
                _footstepsCurrentTime = _footstepsMaxTime;
            }
        }

        if (!_myLogicScript.IsEnemyWalking() && _footstepsCurrentTime != _footstepsMaxTime) {
            _footstepsCurrentTime = _footstepsMaxTime;
        }

        if (_enemyHasStatus) {
            StatusSoundTimer();
        }
    }
    
    private void StatusSoundTimer() {

        if (_statusConditionCurrentTime == _statusConditionMaxTime) {
            if (_myLogicScript.CanBurn()) {
                SoundManager.Instance.PlayBurningSound(_myLogicScript.transform.position);
            }
            if (_myLogicScript.CanParalyze()) {
                SoundManager.Instance.PlayParalyzedSound(_myLogicScript.transform.position);
            }
            if (_myLogicScript.CanFreeze()) {
                SoundManager.Instance.PlayFreezingSound(_myLogicScript.transform.position);
            }
            if (_myLogicScript.CanPoison()) {
                SoundManager.Instance.PlayPoisonSound(_myLogicScript.transform.position);
            }
            if (_myLogicScript.CanSlime()) {
                SoundManager.Instance.PlaySlimedSound(_myLogicScript.transform.position);
            }
        }

        _statusConditionCurrentTime -= Time.deltaTime;

        if (_statusConditionCurrentTime <= 0) {
            _statusConditionCurrentTime = _statusConditionMaxTime;
        }

    }

    protected virtual void PlayMyFootstepSound() {
        SoundManager.Instance.PlayFootstepSound(_myLogicScript.transform.position);
    }

    public virtual void PlayAttackSound() {
        SoundManager.Instance.PlayAttackSound(_myLogicScript.transform.position);
    }

    public void PlayHitSound() {
        SoundManager.Instance.PlayHitSound(_myLogicScript.transform.position);
    }

    public void PlayDeathSound() {
        SoundManager.Instance.PlayDeathSound(_myLogicScript.transform.position);
    }
}
