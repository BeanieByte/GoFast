using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundsScript : MonoBehaviour
{
    private PlayerScript _playerLogicScript;

    private float _footstepsCurrentTime;
    private float _footstepsMaxTime;
    private float _footstepsDeafultMaxTime = 0.2f;
    private float _footstepsMaxTimeCurrentMultiplier;
    private float _footstepsMaxTimeMultiplier;
    private float _footstepsMaxTimeDefaultMultiplier = 1f;
    private bool _playerIsRunningAndGrounded;
    private bool _wasFootstepMaxTimeUpdated;

    private float _tryingToWalkButHasAWallCurrentTime;
    private float _tryingToWalkButHasAWallMaxTime = 0.3f;
    private bool _playerIsTryingToMoveButHasAWall;

    private float _statusConditionCurrentTime;
    private float _statusConditionMaxTime;
    private float _statusConditionDefaultMaxTime = 0.5f;

    enum StatusState {
        Unafflicted,
        Burned,
        Paralyzed,
        Frozen,
        Poisoned,
        Slimed
    }

    private StatusState _statusState;

    private void Awake()
    {
        _playerLogicScript = GetComponentInParent<PlayerScript>();
    }

    private void Start()
    {

        _footstepsMaxTimeMultiplier = _footstepsMaxTimeDefaultMultiplier;
        _footstepsMaxTimeCurrentMultiplier = _footstepsMaxTimeMultiplier;

        _footstepsMaxTime = _footstepsDeafultMaxTime;
        _footstepsCurrentTime = _footstepsMaxTime;

        _statusConditionMaxTime = _statusConditionDefaultMaxTime;
        _statusConditionCurrentTime = _statusConditionMaxTime - 0.01f;

        _wasFootstepMaxTimeUpdated = false;

        _playerLogicScript.OnRunAnimSpeedChange += _playerLogicScript_OnRunAnimSpeedChange;

        _playerLogicScript.OnPlayerAttacked += _playerLogicScript_OnPlayerAttacked;

        _playerLogicScript.OnPlayerJumped += _playerLogicScript_OnPlayerJumped;
        _playerLogicScript.OnPlayerAirJumped += _playerLogicScript_OnPlayerAirJumped;
    }

    private void _playerLogicScript_OnRunAnimSpeedChange(object sender, PlayerScript.OnRunAnimSpeedChangeEventArgs e) {
        //_footstepsMaxTimeCurrentDivider = e.runAnimSpeedMultiplier;
        //_footstepsMaxTime *= _footstepsMaxTimeCurrentDivider;
    }

    private void _playerLogicScript_OnPlayerAirJumped(object sender, System.EventArgs e) {
        SoundManager.Instance.PlayJumpSound(_playerLogicScript.transform.position);
    }

    private void _playerLogicScript_OnPlayerJumped(object sender, System.EventArgs e) {
        SoundManager.Instance.PlayJumpSound(_playerLogicScript.transform.position);
    }

    private void _playerLogicScript_OnPlayerAttacked(object sender, System.EventArgs e) {
        SoundManager.Instance.PlayAttackSound(_playerLogicScript.transform.position);
    }

    private void FixedUpdate()
    {
        StatusStateMachine();

        PlayerWallHitSoundTimer();
    }

    private void FootstepSoundTimer(float playSpeedMultiplier) {

        if (_footstepsMaxTimeCurrentMultiplier != playSpeedMultiplier) {
            _footstepsMaxTime = _footstepsDeafultMaxTime;
            _footstepsMaxTimeMultiplier = playSpeedMultiplier;
            _footstepsMaxTimeCurrentMultiplier = _footstepsMaxTimeMultiplier;
        }

        if (!_wasFootstepMaxTimeUpdated && _statusState != StatusState.Unafflicted) {
            _footstepsMaxTime *= _footstepsMaxTimeCurrentMultiplier;
            _wasFootstepMaxTimeUpdated = true;
        }

        if (_playerLogicScript.IsPlayerRunning() && _playerLogicScript.IsPlayerGrounded()) {
            _playerIsRunningAndGrounded = true;
        } else {
            _playerIsRunningAndGrounded = false;
        }

        if (_playerIsRunningAndGrounded) {
            if (_footstepsCurrentTime == _footstepsMaxTime) {
                SoundManager.Instance.PlayFootstepSound(_playerLogicScript.transform.position);
            }

            _footstepsCurrentTime -= Time.deltaTime;

            if (_footstepsCurrentTime <= 0) {
                _footstepsCurrentTime = _footstepsMaxTime;
            }
        }

        if (!_playerIsRunningAndGrounded && _footstepsCurrentTime != _footstepsMaxTime) {
            _footstepsCurrentTime = _footstepsMaxTime;
        }
    }

    private void PlayerWallHitSoundTimer() {

        if (_playerLogicScript.IsPlayerRunning() && _playerLogicScript.IsWallRightInFrontOfPlayer()) {
            _playerIsTryingToMoveButHasAWall = true;
        } else {
            _playerIsTryingToMoveButHasAWall = false;
        }

        if (_playerIsTryingToMoveButHasAWall) {
            if (_tryingToWalkButHasAWallCurrentTime == _tryingToWalkButHasAWallMaxTime) {
                SoundManager.Instance.PlayPlayerWallHitSound(_playerLogicScript.transform.position);
            }

            _tryingToWalkButHasAWallCurrentTime -= Time.deltaTime;

            if (_tryingToWalkButHasAWallCurrentTime <= 0) {
                _tryingToWalkButHasAWallCurrentTime = _tryingToWalkButHasAWallMaxTime;
            }
        }

        if (!_playerIsTryingToMoveButHasAWall && _tryingToWalkButHasAWallCurrentTime != _tryingToWalkButHasAWallMaxTime) {
            _tryingToWalkButHasAWallCurrentTime = _tryingToWalkButHasAWallMaxTime;
        }
    }

    private void StatusSoundTimer() {

        _statusConditionCurrentTime -= Time.deltaTime;

        if (_statusConditionCurrentTime <= 0) {
            _statusConditionCurrentTime = _statusConditionMaxTime;
        }

    }

    public void StartBurnConditionSound() {
        SoundManager.Instance.PlayPlayerBurningStart(_playerLogicScript.transform.position);
        _statusState = StatusState.Burned;
    }

    public void StartParalyzedConditionSound() {
        SoundManager.Instance.PlayPlayerParalyzedStart(_playerLogicScript.transform.position);
        _statusState = StatusState.Paralyzed;
    }

    public void StartFrozenConditionSound() {
        SoundManager.Instance.PlayPlayerFreezeStart(_playerLogicScript.transform.position);
        _statusState = StatusState.Frozen;
    }

    public void StartPoisonConditionSound() {
        SoundManager.Instance.PlayPlayerPoisonedStart(_playerLogicScript.transform.position);
        _statusState = StatusState.Poisoned;
    }

    public void StartSlimeConditionSound() {
        SoundManager.Instance.PlayPlayerSlimedStart(_playerLogicScript.transform.position);
        _statusState = StatusState.Slimed;
    }

    public void StopAnyStatusConditionSound() {

        if (_statusState == StatusState.Burned) {
            SoundManager.Instance.PlayPlayerBurningStop(_playerLogicScript.transform.position);
        }
        if (_statusState == StatusState.Paralyzed) {
            SoundManager.Instance.PlayPlayerParalyzedStop(_playerLogicScript.transform.position);
        }
        if (_statusState == StatusState.Frozen) {
            SoundManager.Instance.PlayPlayerFreezeStop(_playerLogicScript.transform.position);
        }
        if (_statusState == StatusState.Poisoned) {
            SoundManager.Instance.PlayPlayerPoisonedStop(_playerLogicScript.transform.position);
        }
        if (_statusState == StatusState.Slimed) {
            SoundManager.Instance.PlayPlayerSlimedStop(_playerLogicScript.transform.position);
        }

        if (_wasFootstepMaxTimeUpdated) {
            _wasFootstepMaxTimeUpdated = false;
        }

        _statusState = StatusState.Unafflicted;
        _statusConditionMaxTime = _statusConditionDefaultMaxTime;
        _statusConditionCurrentTime = _statusConditionMaxTime - 0.49f;
    }

    private void StatusStateMachine() {

        switch (_statusState) {
            case StatusState.Unafflicted:
                FootstepSoundTimer(_footstepsMaxTimeDefaultMultiplier);
                break;

            case StatusState.Burned:
                FootstepSoundTimer(1.5f);
                if (_statusConditionCurrentTime == _statusConditionMaxTime) {
                    SoundManager.Instance.PlayBurningSound(_playerLogicScript.transform.position);
                }
                StatusSoundTimer();
                break;

            case StatusState.Paralyzed:
                FootstepSoundTimer(0.5f);
                if (_statusConditionCurrentTime == _statusConditionMaxTime) {
                    SoundManager.Instance.PlayParalyzedSound(_playerLogicScript.transform.position);
                }
                StatusSoundTimer();
                break;

            case StatusState.Frozen:
                if (_statusConditionCurrentTime == _statusConditionMaxTime) {
                    SoundManager.Instance.PlayFreezingSound(_playerLogicScript.transform.position);
                }
                StatusSoundTimer();
                break;

            case StatusState.Poisoned:
                FootstepSoundTimer(_footstepsMaxTimeDefaultMultiplier);
                if (_statusConditionCurrentTime == _statusConditionMaxTime) {
                    SoundManager.Instance.PlayPoisonSound(_playerLogicScript.transform.position);
                }
                StatusSoundTimer();
                break;

            case StatusState.Slimed:
                FootstepSoundTimer(0.7f);
                if (_statusConditionCurrentTime == _statusConditionMaxTime) {
                    SoundManager.Instance.PlaySlimedSound(_playerLogicScript.transform.position);
                }
                StatusSoundTimer();
                break;
        }
    }

    public void PlayLandingSound() {
        SoundManager.Instance.PlayLandingSound(_playerLogicScript.transform.position);
    }

    public void PlayHitSound() {
        SoundManager.Instance.PlayHitSound(_playerLogicScript.transform.position);
    }

    public void PlayDeathSound() {
        SoundManager.Instance.PlayDeathSound(_playerLogicScript.transform.position);
    }

    private void OnDestroy() {
        _playerLogicScript.OnRunAnimSpeedChange -= _playerLogicScript_OnRunAnimSpeedChange;

        _playerLogicScript.OnPlayerAttacked -= _playerLogicScript_OnPlayerAttacked;

        _playerLogicScript.OnPlayerJumped -= _playerLogicScript_OnPlayerJumped;
        _playerLogicScript.OnPlayerAirJumped -= _playerLogicScript_OnPlayerAirJumped;
    }

}
