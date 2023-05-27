using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class PlayerScript : MonoBehaviour {
    [Header("Player Elements")]

    [SerializeField] private PlayerVisualScript _playerVisual;
    private Rigidbody2D _playerRigidBody;

    private Vector2 _currentPlayerVelocity;


    [Header("Player Stats")]

    [SerializeField] private int _maxHealth = 100;
    private int _currentHealth;
    [SerializeField] private int _attackPower = 20;

    [SerializeField] private float _playerMoveSpeed = 8f;
    private float _playerMoveSpeedMultiplier = 1f;
    private float _playerMoveSpeedMultiplierDefault = 1f;
    [SerializeField] private bool _playerFacingRight;

    [SerializeField] private float _highestJumpForce = 7f;
    [SerializeField] private float _lowestJumpForce = 9f;
    private float _currentJumpForce;

    [SerializeField] private int _maxAvailableAirJumps = 1;
    private int _currentAvailableAirJumps;

    private float _playerCrouchedMoveSpeedMultiplier = 0.5f;
    private float _decreasedSpeedModifier = 0.5f;

    [Header("Jump Elements")]

    [SerializeField] private Transform _playerFeetPos;
    [SerializeField] private LayerMask _layerIsGrounded;
    private float _checkFeetRadius = 0.3f;
    private bool _isGrounded;

    private bool _isTryingToJump = false;
    private float _maxJumpTime = 0.3f;
    private float _jumpTime = 0f;


    [field: Header("Turbo Elements")]

    [field: SerializeField] public float _maxTurboTime { get; private set; }
    private float _currentTurboTime = 0f;
    private bool _canTurbo = true;
    private bool _isTryingToTurbo = false;

    private float _turboSpeedMultiplier = 2f;

    private float _cooldownUntilStartingTurboRefill = 2f;
    private float _currentTurboCooldownTime = 0f;

    public event EventHandler<OnTurboTimeChangedEventArgs> OnTurboTimeChanged;
    public class OnTurboTimeChangedEventArgs : EventArgs {
        public float turboTime;
    }

    public event EventHandler OnPlayerAttacked;

    public event EventHandler OnPlayerJumped;
    public event EventHandler OnPlayerAirJumped;


    [field: Header("Attack Elements")]
    [SerializeField] private PlayerAttackScript _playerAttack;

    enum JumpState { 
        Grounded,
        Jumping,
        AirJumping,
        Falling
    }

    private JumpState _jumpState;


    enum SpeedState { 
        Regular,
        Turbo,
        Decreased
    }

    private SpeedState _speedState;

    private void Awake() {
        _playerRigidBody = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        GameInput.Instance.OnJumpStarted += Instance_OnJumpStarted;
        GameInput.Instance.OnJumpPerformed += Instance_OnJumpPressed;
        GameInput.Instance.OnJumpCanceled += Instance_OnJumpCanceled;

        GameInput.Instance.OnTurboPressed += Instance_OnTurboPressed;
        GameInput.Instance.OnTurboCanceled += Instance_OnTurboCanceled;

        GameInput.Instance.OnAttackPressed += Instance_OnAttackPressed;

        _playerAttack.OnKillingEnemy += _playerAttack_OnKillingEnemy;

        if (!_playerFacingRight) {
            _playerVisual.Flip();
        }

        _currentPlayerVelocity = _playerRigidBody.velocity;
        _currentHealth = _maxHealth;

        _jumpState = JumpState.Grounded;

        _speedState = SpeedState.Regular;
        _maxTurboTime = 3f;
        _currentTurboTime = _maxTurboTime;

        OnTurboTimeChanged?.Invoke(this, new OnTurboTimeChangedEventArgs {
            turboTime = _currentTurboTime
        }) ;
    }

    private void _playerAttack_OnKillingEnemy(object sender, EventArgs e) {
        RecoverTurboSpeedInstantly();
    }

    private void Instance_OnJumpStarted(object sender, System.EventArgs e) {
        if (_jumpState == JumpState.Grounded) {
            _jumpState = JumpState.Jumping;
            OnPlayerJumped?.Invoke(this, EventArgs.Empty);
        } else if (_jumpState == JumpState.Falling && _currentAvailableAirJumps > 0) {
            StopYVelocity();
            _jumpState = JumpState.AirJumping;
            _currentAvailableAirJumps--;
            OnPlayerAirJumped?.Invoke(this, EventArgs.Empty);
        }
    }

    private void Instance_OnJumpPressed(object sender, System.EventArgs e) {
        if (!_isTryingToJump) {
            _isTryingToJump = true;
            _jumpTime = 0f;
        }
    }

    private void Instance_OnJumpCanceled(object sender, System.EventArgs e) {
        if (_isTryingToJump) {
            _isTryingToJump = false;
            _jumpTime = 0f;
        }
    }

    private void Instance_OnTurboPressed(object sender, System.EventArgs e) {
        if (!_isTryingToTurbo) {
            _isTryingToTurbo = true;
            _currentTurboCooldownTime = 0f;
        }
    }

    private void Instance_OnTurboCanceled(object sender, System.EventArgs e) {
        if (_isTryingToTurbo) {
            _isTryingToTurbo = false;
            _currentTurboCooldownTime = 0f;
        }
    }

    private void Instance_OnAttackPressed(object sender, System.EventArgs e) {
        Attack();
    }


    private void Update() {
        Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();

        Vector2 moveDir = new Vector2(inputVector.x, 0);

        transform.position = (Vector2)transform.position + _playerMoveSpeed * Time.deltaTime * moveDir * _playerMoveSpeedMultiplier;

        _isGrounded = Physics2D.OverlapCircle(_playerFeetPos.position, _checkFeetRadius, _layerIsGrounded);

        if (!_isGrounded && !_isTryingToJump) {
            _jumpState = JumpState.Falling;
        }

        if (moveDir.x < 0 && _playerFacingRight) {
            _playerVisual.Flip();
            _playerFacingRight = !_playerFacingRight;
        } else if (moveDir.x > 0 && !_playerFacingRight){
            _playerVisual.Flip();
            _playerFacingRight = !_playerFacingRight;
        }

        if (_isTryingToTurbo && _canTurbo) {
            _speedState = SpeedState.Turbo;
        }


        switch (_speedState) {
            case SpeedState.Regular:
                SetPlayerMovementVelocity(_playerMoveSpeedMultiplierDefault);
                RecoverTurboSpeedOverTime();
                if (_currentTurboTime > 0) {
                    _canTurbo = true;
                }
                break;
            case SpeedState.Turbo:
                _currentTurboTime -= Time.deltaTime;
                OnTurboTimeChanged?.Invoke(this, new OnTurboTimeChangedEventArgs {
                    turboTime = _currentTurboTime
                });
                if (_currentTurboTime > 0) {
                    SetPlayerMovementVelocity(_turboSpeedMultiplier);
                } else _canTurbo = false;
                if (!_canTurbo || !_isTryingToTurbo) {
                    _speedState = SpeedState.Regular;
                }
                break;
            case SpeedState.Decreased:
                _canTurbo = false;
                SetPlayerMovementVelocity(_decreasedSpeedModifier);
                RecoverTurboSpeedOverTime();
                break;
        }
    }

    private void FixedUpdate() {

        switch (_jumpState) {
            case JumpState.Grounded:
                if (_currentAvailableAirJumps < _maxAvailableAirJumps) {
                    for (int i = 0; i < _maxAvailableAirJumps; i++) {
                        _currentAvailableAirJumps++;
                    }
                }
                break;
            case JumpState.Jumping:
                _jumpTime += Time.deltaTime;
                if (_jumpTime <= _maxJumpTime) {
                    HandleJumpPressingOverTime();
                } else _jumpState = JumpState.Falling;

                break;
            case JumpState.AirJumping:
                _jumpTime += Time.deltaTime;
                if (_jumpTime <= _maxJumpTime) {
                    HandleJumpPressingOverTime();
                } else
                    _jumpState = JumpState.Falling;
                break;
            case JumpState.Falling:
                if (_isGrounded) {
                    _jumpState = JumpState.Grounded;
                }
                break;
        }
    }

    //Unused for now
    private void HandleJumpFirstFrame() {
        _currentPlayerVelocity.y = _lowestJumpForce;
        SetPlayerRigidBodyVelocity(_currentPlayerVelocity);
    }

    private void HandleJumpPressingOverTime() {
        float normalizedJumpTime = Mathf.Clamp01(_jumpTime / _maxJumpTime);
        _currentJumpForce = Mathf.Lerp(_lowestJumpForce, _highestJumpForce, normalizedJumpTime);

        _currentPlayerVelocity.y = _currentJumpForce;
        SetPlayerRigidBodyVelocity(_currentPlayerVelocity);
    }

    private void StopYVelocity() {
        _currentPlayerVelocity.y = 0f;
        SetPlayerRigidBodyVelocity(_currentPlayerVelocity);
    }

    private void SetPlayerRigidBodyVelocity(Vector2 newPlayerVelocity) {
        _playerRigidBody.velocity = newPlayerVelocity;
    }

    private void SetPlayerMovementVelocity(float speedMultiplier) {
        _playerMoveSpeedMultiplier = speedMultiplier;
    }

    private void RecoverTurboSpeedOverTime() {
        //if (!_isTryingToTurbo && _currentTurboTime < _maxTurboTime) {
        //    _currentTurboCooldownTime += Time.deltaTime;
        //    OnTurboTimeChanged?.Invoke(this, new OnTurboTimeChangedEventArgs {
        //        turboTime = _currentTurboTime
        //    });
        //    if (_currentTurboCooldownTime >= _cooldownUntilStartingTurboRefill) {
        //        _currentTurboTime += Time.deltaTime;
        //        if (_currentTurboTime > _maxTurboTime) {
        //            _currentTurboTime = _maxTurboTime;
        //            OnTurboTimeChanged?.Invoke(this, new OnTurboTimeChangedEventArgs {
        //                turboTime = _currentTurboTime
        //            });
        //        }
        //    }
        //}

        if (_currentTurboTime > _maxTurboTime) {
            _currentTurboTime = _maxTurboTime;
            OnTurboTimeChanged?.Invoke(this, new OnTurboTimeChangedEventArgs {
                turboTime = _currentTurboTime
            });
        }

        if (_isTryingToTurbo || _currentTurboTime >= _maxTurboTime) {
            return;
        }

        _currentTurboCooldownTime += Time.deltaTime;

        if (_currentTurboCooldownTime >= _cooldownUntilStartingTurboRefill && _currentTurboTime < _maxTurboTime) {
            _currentTurboTime += Time.deltaTime;
            OnTurboTimeChanged?.Invoke(this, new OnTurboTimeChangedEventArgs {
                turboTime = _currentTurboTime
            });
        }
        
        
    }

    private void RecoverTurboSpeedInstantly() {
        if (_currentTurboTime != _maxTurboTime) {
            _currentTurboTime = _maxTurboTime;
            OnTurboTimeChanged?.Invoke(this, new OnTurboTimeChangedEventArgs {
                turboTime = _currentTurboTime
            });
        }
    }

    private void Attack() {
        _playerAttack.SetAttackPower(_attackPower);
        OnPlayerAttacked?.Invoke(this, EventArgs.Empty);
    }
    

    public void Damage(int enemyAttackPower) {
        _currentHealth -= enemyAttackPower;
    }

    public float PlayersYVelocity()
    {
        return _playerRigidBody.velocity.y;
    }

    public bool IsPlayerRunning()
    {
        Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();

        if(inputVector != Vector2.zero)
        {
            return true;
        }

        return false;
    }
}
