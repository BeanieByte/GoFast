using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisualScript : MonoBehaviour
{
    private PlayerScript _playerLogicScript;

    private Animator _playerAnimator;

    private const string isRunningBool = "isRunning";
    private const string isGroundedBool = "isGrounded";
    private const string jumpedTrigger = "jumped";
    private const string airJumpedTrigger = "airJumped";
    private const string fellTrigger = "fell";
    private const string isAttackingBool = "isAttacking";
    private const string attackTrigger = "attack";
    private const string isJumpingBool = "isJumping";
    private const string isFallingBool = "isFalling";
    private const string dieTrigger = "die";
    private const string hitTrigger = "hit";

    enum AnimAirState { 
        Grounded,
        Jumping,
        Falling,
    }

    private AnimAirState _animAirState;

    private void Awake()
    {
        _playerLogicScript = GetComponentInParent<PlayerScript>();
        _playerAnimator = GetComponent<Animator>();
    }

    private void Start()
    {
        _playerLogicScript.OnPlayerAttacked += _playerLogicScript_OnPlayerAttacked;

        _playerLogicScript.OnPlayerJumped += _playerLogicScript_OnPlayerJumped;
        _playerLogicScript.OnPlayerAirJumped += _playerLogicScript_OnPlayerAirJumped;
    }

    private void _playerLogicScript_OnPlayerAirJumped(object sender, System.EventArgs e)
    {
        _playerAnimator.SetTrigger(airJumpedTrigger);
    }

    private void _playerLogicScript_OnPlayerJumped(object sender, System.EventArgs e)
    {
        _playerAnimator.SetTrigger(jumpedTrigger);
    }

    private void _playerLogicScript_OnPlayerAttacked(object sender, System.EventArgs e)
    {
        _playerAnimator.SetTrigger(attackTrigger);
    }

    public void Flip() {
        transform.Rotate(0f, 180f, 0f);
    }

    private void Update()
    {
        if (_playerLogicScript.IsPlayerRunning()) {
            _playerAnimator.SetBool(isRunningBool, true);
        }
        else _playerAnimator.SetBool(isRunningBool, false);

        if (_playerLogicScript.PlayersYVelocity() == 0)
        {
            _animAirState = AnimAirState.Grounded;
        }
        else if (_playerLogicScript.PlayersYVelocity() > 0)
        {
            _animAirState = AnimAirState.Jumping;
        }
        else if (_playerLogicScript.PlayersYVelocity() < 0)
        {
            _animAirState = AnimAirState.Falling;
        }

        switch (_animAirState) {
            case AnimAirState.Grounded:
                _playerAnimator.SetBool(isGroundedBool, true);
                _playerAnimator.SetBool(isJumpingBool, false);
                _playerAnimator.SetBool(isFallingBool, false);
                break;
            case AnimAirState.Jumping:
                _playerAnimator.SetBool(isGroundedBool, false);
                _playerAnimator.SetBool(isJumpingBool, true);
                _playerAnimator.SetBool(isFallingBool, false);
                break;
            case AnimAirState.Falling:
                _playerAnimator.SetBool(isGroundedBool, false);
                _playerAnimator.SetBool(isJumpingBool, false);
                _playerAnimator.SetBool(isFallingBool, true);
                break;
        }
    }
    
}

