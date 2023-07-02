using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisualScript : MonoBehaviour
{
    #region Variables

    private PlayerScript _playerLogicScript;
    private PlayerVisualShaderScript _playerShaderScript;

    private Animator _playerAnimator;

    private const string isRunning_CONST = "isRunning";
    private const string runAnimSpeed_CONST = "runAnimSpeed";
    private const string isGrounded_CONST = "isGrounded";
    private const string jumped_CONST = "jumped";
    private const string airJumped_CONST = "airJumped";
    private const string isAttacking_CONST = "isAttacking";
    private const string attack_CONST = "attack";
    private const string isJumping_CONST = "isJumping";
    private const string isFalling_CONST = "isFalling";
    private const string die_CONST = "die";
    private const string isAlive_CONST = "isAlive";

    public event EventHandler OnPlayerHitAnimStarted;
    public event EventHandler OnPlayerHitAnimStopped;

    public event EventHandler OnPlayerInvincibleAnimStarted;
    public event EventHandler OnPlayerInvincibleAnimAlmostOver;
    public event EventHandler OnPlayerInvincibleAnimStopped;

    public event EventHandler OnPlayerBurnAnimStarted;
    public event EventHandler OnPlayerBurnAnimStopped;

    public event EventHandler OnPlayerParalyzedAnimStarted;
    public event EventHandler OnPlayerParalyzedAnimStopped;

    public event EventHandler OnPlayerFreezeAnimStarted;
    public event EventHandler OnPlayerFreezeAnimStopped;

    public event EventHandler OnPlayerPoisonedAnimStarted;
    public event EventHandler OnPlayerPoisonedAnimStopped;

    public event EventHandler OnPlayerSlimedAnimStarted;
    public event EventHandler OnPlayerSlimedAnimStopped;

    enum AnimAirState { 
        Grounded,
        Jumping,
        Falling,
    }

    private AnimAirState _animAirState;

    #endregion

    private void Awake()
    {
        _playerLogicScript = GetComponentInParent<PlayerScript>();
        _playerShaderScript = GetComponent<PlayerVisualShaderScript>();
        _playerAnimator = GetComponent<Animator>();
    }

    private void Start()
    {
        _animAirState = AnimAirState.Grounded;
        _playerAnimator.SetBool(isAlive_CONST, true);
        
        _playerLogicScript.OnRunAnimSpeedChange += _playerLogicScript_OnRunAnimSpeedChange;

        _playerLogicScript.OnPlayerAttacked += _playerLogicScript_OnPlayerAttacked;

        _playerLogicScript.OnPlayerJumped += _playerLogicScript_OnPlayerJumped;
        _playerLogicScript.OnPlayerAirJumped += _playerLogicScript_OnPlayerAirJumped;
    }

    #region EventReceiverFunctions

    private void _playerLogicScript_OnRunAnimSpeedChange(object sender, PlayerScript.OnRunAnimSpeedChangeEventArgs e) {
        _playerAnimator.SetFloat(runAnimSpeed_CONST, e.runAnimSpeedMultiplier);
    }

    private void _playerLogicScript_OnPlayerAirJumped(object sender, System.EventArgs e)
    {
        _playerAnimator.SetTrigger(airJumped_CONST);
    }

    private void _playerLogicScript_OnPlayerJumped(object sender, System.EventArgs e)
    {
        _playerAnimator.SetTrigger(jumped_CONST);
    }

    private void _playerLogicScript_OnPlayerAttacked(object sender, System.EventArgs e)
    {
        _playerAnimator.SetTrigger(attack_CONST);
    }

    #endregion

    private void FixedUpdate()
    {

        if (_playerLogicScript.IsPlayerRunning()) {
            _playerAnimator.SetBool(isRunning_CONST, true);
        }
        else _playerAnimator.SetBool(isRunning_CONST, false);

        if (_playerLogicScript.PlayersYVelocity() == 0)
        {
            _animAirState = AnimAirState.Grounded;
        }
        if (_playerLogicScript.PlayersYVelocity() > 0)
        {
            _animAirState = AnimAirState.Jumping;
        }
        if (_playerLogicScript.PlayersYVelocity() < 0)
        {
            _animAirState = AnimAirState.Falling;
        }

        AnimAirStateMachine();
    }

    private void AnimAirStateMachine() {

        switch (_animAirState) {
            case AnimAirState.Grounded:
                _playerAnimator.SetBool(isGrounded_CONST, true);
                _playerAnimator.SetBool(isJumping_CONST, false);
                _playerAnimator.SetBool(isFalling_CONST, false);
                break;
            case AnimAirState.Jumping:
                _playerAnimator.SetBool(isGrounded_CONST, false);
                _playerAnimator.SetBool(isJumping_CONST, true);
                _playerAnimator.SetBool(isFalling_CONST, false);
                break;
            case AnimAirState.Falling:
                _playerAnimator.SetBool(isGrounded_CONST, false);
                _playerAnimator.SetBool(isJumping_CONST, false);
                _playerAnimator.SetBool(isFalling_CONST, true);
                break;
        }

    }

    #region FunctionsCalledByLogicScript

    public void Flip() {
        transform.Rotate(0f, 180f, 0f);
    }

    public void SetIsAttackingToTrue() {
        _playerAnimator.SetBool(isAttacking_CONST, true);
    }

    public void SetIsAttackingToFalse() {
        _playerAnimator.SetBool(isAttacking_CONST, false);
    }

    public void PlayHitAnim() {
        OnPlayerHitAnimStarted?.Invoke(this, EventArgs.Empty);
    }

    public void StopHitAnim() {
        OnPlayerHitAnimStopped?.Invoke(this, EventArgs.Empty);
    }

    public void PlayInvincibleAnim() {
        OnPlayerInvincibleAnimStarted?.Invoke(this, EventArgs.Empty);
    }

    public void PlayInvincibleAnimAlmostOver() {
        OnPlayerInvincibleAnimAlmostOver?.Invoke(this, EventArgs.Empty);
    }

    public void StopInvincibleAnim() {
        OnPlayerInvincibleAnimStopped?.Invoke(this, EventArgs.Empty);
    }

    public void PlayDeathAnim() {
        _playerAnimator.SetBool(isAlive_CONST, false);
        _playerAnimator.SetTrigger(die_CONST);
    }

    public void Die() {
        GameManager.Instance.SetGameOver();
        Destroy(_playerLogicScript.gameObject);
    }

    public void BurnPlayerStart() {
        OnPlayerBurnAnimStarted?.Invoke(this, EventArgs.Empty);
    }

    public void BurnPlayerStop() { 
        OnPlayerBurnAnimStopped?.Invoke(this, EventArgs.Empty);
    }

    public void ParalyzePlayerStart() { 
        OnPlayerParalyzedAnimStarted?.Invoke(this, EventArgs.Empty);
    }

    public void ParalyzePlayerStop() { 
        OnPlayerParalyzedAnimStopped?.Invoke(this, EventArgs.Empty);
    }

    public void FreezePlayerStart() {
        _playerAnimator.enabled = false;
        OnPlayerFreezeAnimStarted?.Invoke(this, EventArgs.Empty);
    }

    public void FreezePlayerStop() {
        _playerAnimator.enabled = true;
        OnPlayerFreezeAnimStopped?.Invoke(this, EventArgs.Empty);
    }

    public void PoisonPlayerStart() { 
        OnPlayerPoisonedAnimStarted?.Invoke(this, EventArgs.Empty);
    }

    public void PoisonPlayerStop() { 
        OnPlayerPoisonedAnimStopped?.Invoke(this, EventArgs.Empty);
    }

    public void SlimePlayerStart() { 
        OnPlayerSlimedAnimStarted?.Invoke(this, EventArgs.Empty);
    }

    public void SlimePlayerStop() { 
        OnPlayerSlimedAnimStopped?.Invoke(this, EventArgs.Empty);
    }

    #endregion
}

