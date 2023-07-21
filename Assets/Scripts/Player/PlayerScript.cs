using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEditor;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

    public static PlayerScript Instance;

    #region Variables

    [Header("Player Elements")]

    [SerializeField] private PlayerVisualScript _playerVisual;
    [SerializeField] private PlayerSoundsScript _playerSounds;

    private Rigidbody2D _playerRigidBody;

    private Vector2 _currentPlayerVelocity;


    [Header("Player Stats")]

    [SerializeField] private int _maxHealth = 20;
    private int _maxHealthDefault = 20;
    private int _totalMaxHealth = 99;
    private int _currentHealth;
    [SerializeField] private int _maxAttackPower = 20;
    private int _maxAttackPowerDefault = 20;
    private int _currentAttackPower;

    [SerializeField] private float _playerMoveSpeed = 8f;
    private bool _isPlayerRunning = false;
    private float _playerMoveSpeedMultiplier;
    private float _playerMoveSpeedMultiplierDefault = 1f;
    [SerializeField] private bool _playerFacingRight;

    [SerializeField] private Transform _playerWallCheck;
    private float _wallIsInFrontSpeedMultiplier = 1f;
    private float _wallCheckRayOffset = 0.005f;
    private bool _isWallRightInFrontOfPlayer;

    [SerializeField] private float _highestJumpForce = 7f;
    [SerializeField] private float _lowestJumpForce = 9f;
    private float _lowestJumpForceDefault = 9f;
    private float _currentJumpForce;

    [SerializeField] private int _maxAvailableAirJumps = 1;
    private int _maxAvailableAirJumpsDefault = 1;
    private int _currentAvailableAirJumps;

    private float _maxUntilCanAttackCooldown = 1f;
    private float _currentCanAttackCooldown = 0f;
    private bool _canAttack = true;
    private bool _isTryingToAttack = false;

    private bool _isInvincible = false;
    [SerializeField] private GameObject _invincibleTouchAttackTrigger;
    private float _checkForInvinciblePowerUpAlmostOver;
    private bool _invincibleAlmostOverHasStarted;
    private float _maxInvincibleTime = 1.5f;
    private float _maxInvincibleTimeDefault = 1.5f;
    private float _currentInvincibleTime = 0f;
    private float _invinciblePowerUpSpeedMultiplier = 1.5f;
    private bool _gotInvinciblePowerUp = false;

    private bool _isDead = false;

    public event EventHandler<OnHealthChangedEventArgs> OnHealthChanged;

    public class OnHealthChangedEventArgs : EventArgs {
        public int health;
    }

    [Header("Jump Elements")]

    [SerializeField] private Transform _playerFeetPos;
    [SerializeField] private LayerMask _layerIsGrounded;
    [SerializeField] private LayerMask _layerPlatforms;
    private float _checkFeetRadius;
    private float _checkFeetRadiusDivider = 19f; //13.3f;
    private bool _isGrounded;

    private bool _canJump = true;
    private bool _isTryingToJump = false;
    private float _maxJumpTime = 0.3f;
    private float _jumpTime = 0f;

    private bool _jumpedOnBounceable;

    private float _jumpApexModifierMaxTime = 0.15f;
    private float _jumpApexModifierCurrentTime;
    private float _jumpApexModifierMaxPlayerForce = 1.5f;
    private float _jumpApexModifierDefaultPlayerForce = 1.2f;
    private float _jumpApexModifierCurrentPlayerForce;
    private bool _jumpApexModifierPlayed;

    private float _jumpApexModifierMinimumTimeToEnable = 0.1f;
    private float _jumpApexModifierCurrentTimeToEnable;

    private float _jumpBufferingMaxTime = 0.2f;
    private float _jumpBufferingCurrentTime;
    private bool _isBufferingJump;
    private bool _canBufferedJumpBeUsed;

    private float _jumpCoyoteMaxTime = 0.15f;
    private float _jumpCoyoteCurrentTime;
    private bool _canStartJumpCoyoteTime;
    private bool _isWithinJumpCoyoteTime;

    private float _minFallingYVelocity = -18f;

    [SerializeField] private Transform _playerSideEdgeDetectionCheck;
    [SerializeField] private Transform _playerUpperEdgeDetectionRightCheck;
    [SerializeField] private Transform _playerUpperEdgeDetectionLeftCheck;
    private float _jumpNudgeSpeed = 0.05f;
    private bool _isNudgingOfUpperEdges;
    private bool _startedUpperEdgeNudging;
    private float _jumpUpperEdgeDetectionRadius;
    private float _jumpUpperEdgeDetectionRadiusDivider = 50f;

    public event EventHandler OnPlayerJumped;
    public event EventHandler OnPlayerAirJumped;

    public event EventHandler<OnAirJumpCounterChangedEventArgs> OnAirJumpCounterChanged;

    public class OnAirJumpCounterChangedEventArgs : EventArgs {
        public int airJumps;
    }

    [field: Header("Turbo Elements")]

    [field: SerializeField] private float _maxTurboTime = 3f;
    private float _maxTurboTimeDefault = 3f;
    private float _currentTurboTime = 0f;
    private bool _canTurbo = true;
    private bool _isTryingToTurbo = false;

    private float _turboSpeedMultiplier;
    private float _turboSpeedMultiplierDefault = 2f;
    private float _turboSpeedMultiplierWhileNotTurboing = 1f;

    private float _cooldownUntilStartingTurboRefill = 2f;
    private float _currentTurboCooldownTime = 0f;

    public event EventHandler<OnTurboTimeChangedEventArgs> OnTurboTimeChanged;
    public class OnTurboTimeChangedEventArgs : EventArgs {
        public float turboTime;
    }

    public event EventHandler<OnRunAnimSpeedChangeEventArgs> OnRunAnimSpeedChange;
    public class OnRunAnimSpeedChangeEventArgs : EventArgs {
        public float runAnimSpeedMultiplier;
    }

    public event EventHandler OnPlayerAttacked;


    [field: Header("Attack Elements")]
    [SerializeField] private PlayerAttackScript _playerAttack;
    [SerializeField] private PlayerTouchAttackScript _playerTouchAttack;


    [field: Header("Status Effects")]

    private bool _isPlayerAfflictedWithStatus = false;
    private float _statusTimer = 0f;

    private float _maxBurnedTime = 5f;
    private float _burnedAttackMultiplier = 0.5f;
    private float _burnedSpeedMultiplier = 1.5f;

    private float _maxParalyzedTime = 5f;
    private float _paralyzedSpeedMultiplier = 0.5f;

    private bool _isFrozen = false;
    private float _maxFrozenTime = 3f;

    private float _maxPoisonedTime = 10f;
    private float _defaultPoisonTimer = 2f;
    private float _currentPoisonTimer;
    private int _currentHealthToDecrease;
    private int _healthToDecreaseMultiplier = 2;
    private int _poisonHealthDivider = 20;

    private float _maxSlimedTime = 5f;
    private float _slimedSpeedMultiplier = 0.8f;

    enum JumpState { 
        Grounded,
        Jumping,
        AirJumping,
        Falling
    }

    private JumpState _jumpState;

    enum StatusState
    {
        Unafflicted,
        Burned,
        Paralyzed,
        Frozen,
        Poisoned,
        Slimed
    }

    private StatusState _statusState;

    #endregion

    private void Awake() {
        _playerRigidBody = GetComponent<Rigidbody2D>(); 
        _isDead = false;
    }

    private void Start() {

        GameInput.Instance.OnJumpStarted += Instance_OnJumpStarted;
        GameInput.Instance.OnJumpPerformed += Instance_OnJumpPressed;
        GameInput.Instance.OnJumpCanceled += Instance_OnJumpCanceled;

        GameInput.Instance.OnTurboPressed += Instance_OnTurboPressed;
        GameInput.Instance.OnTurboCanceled += Instance_OnTurboCanceled;

        GameInput.Instance.OnAttackHeld += Instance_OnAttackPressed;
        GameInput.Instance.OnAttackReleased += Instance_OnAttackReleased;

        _playerAttack.OnKillingEnemy += _playerAttack_OnKillingEnemy;
        _playerTouchAttack.OnInvincibleTouchKillingEnemy += _playerTouchAttack_OnInvincibleTouchKillingEnemy;

        _checkFeetRadius = transform.localScale.x / _checkFeetRadiusDivider;
        _jumpUpperEdgeDetectionRadius = transform.localScale.x / _jumpUpperEdgeDetectionRadiusDivider;

        if (!_playerFacingRight) {
            _playerVisual.Flip();
        }

        _isInvincible = false;
        _invincibleAlmostOverHasStarted = false;
        _invincibleTouchAttackTrigger.SetActive(false);

        _isPlayerAfflictedWithStatus = false;
        _statusTimer = 0f;
        _isFrozen = false;

        _currentPlayerVelocity = _playerRigidBody.velocity;
        _wallIsInFrontSpeedMultiplier = 1f;
        _isWallRightInFrontOfPlayer = false;

        _maxHealth = _maxHealthDefault;
        _currentHealth = _maxHealth;

        _playerMoveSpeedMultiplier = _playerMoveSpeedMultiplierDefault;

        _canJump = true;

        _jumpedOnBounceable = false;

        _jumpApexModifierCurrentTime = _jumpApexModifierMaxTime;
        _jumpApexModifierCurrentPlayerForce = _jumpApexModifierDefaultPlayerForce;
        _jumpApexModifierPlayed = false;

        _jumpApexModifierCurrentTimeToEnable = 0f;

        _jumpBufferingCurrentTime = 0f;
        _isBufferingJump = false;
        _canBufferedJumpBeUsed = false;

        _jumpCoyoteCurrentTime = _jumpCoyoteMaxTime;
        _canStartJumpCoyoteTime = false;
        _isWithinJumpCoyoteTime = false;

        _isNudgingOfUpperEdges = false;
        _startedUpperEdgeNudging = false;

        OnHealthChanged?.Invoke(this, new OnHealthChangedEventArgs {
            health = _currentHealth
        });

        _maxAvailableAirJumps = _maxAvailableAirJumpsDefault;
        _currentAvailableAirJumps = _maxAvailableAirJumps;

        OnAirJumpCounterChanged?.Invoke(this, new OnAirJumpCounterChangedEventArgs {
            airJumps = _currentAvailableAirJumps
        });

        _maxAttackPower = _maxAttackPowerDefault;
        _currentAttackPower = _maxAttackPower;

        _jumpState = JumpState.Grounded;

        _turboSpeedMultiplier = _turboSpeedMultiplierWhileNotTurboing;
        _maxTurboTime = _maxTurboTimeDefault;
        _currentTurboTime = _maxTurboTime;

        OnTurboTimeChanged?.Invoke(this, new OnTurboTimeChangedEventArgs {
            turboTime = _currentTurboTime
        }) ;

        _canAttack = true;
        _isTryingToAttack = false;

        _currentPoisonTimer = _defaultPoisonTimer;
    }

    #region EventReceiverFunctions

    private void _playerAttack_OnKillingEnemy(object sender, EventArgs e) {
        PlayerKilledEnemy();
    }

    private void _playerTouchAttack_OnInvincibleTouchKillingEnemy(object sender, EventArgs e)
    {
        PlayerKilledEnemy();
    }

    private void Instance_OnJumpStarted(object sender, System.EventArgs e) {
        if (!_canJump) { return; }
        Jump();
    }

    private void Instance_OnJumpPressed(object sender, System.EventArgs e) {
        if (!_isTryingToJump && _canJump) {
            _isTryingToJump = true;
        }
    }

    private void Instance_OnJumpCanceled(object sender, System.EventArgs e) {
        if (_isTryingToJump) {
            _isTryingToJump = false;
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
        if(!_isTryingToAttack)
        {
            _isTryingToAttack = true;
        }
    }

    private void Instance_OnAttackReleased(object sender, EventArgs e)
    {
        if (_isTryingToAttack)
        {
            _isTryingToAttack = false;
        }
    }

    #endregion

    private void OnDrawGizmos() {
        Gizmos.DrawSphere(_playerFeetPos.position, _checkFeetRadius);
        Gizmos.DrawSphere(_playerUpperEdgeDetectionLeftCheck.position, _jumpUpperEdgeDetectionRadius);
        Gizmos.DrawSphere(_playerUpperEdgeDetectionRightCheck.position, _jumpUpperEdgeDetectionRadius);
    }

    private void FixedUpdate() {
        if (_isDead) {
            return;
        }

        Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();

        if (inputVector == Vector2.zero) {
            _isPlayerRunning = false;
        } else _isPlayerRunning = true;

        Vector2 moveDir = new Vector2(inputVector.x, 0);

        CheckIfWallIsRightInFront();

        if (_isWallRightInFrontOfPlayer) {
            JumpSideEdgeDetection();
        }

        if (!_isFrozen) {
            transform.position = (Vector2)transform.position + _playerMoveSpeed * Time.deltaTime * moveDir * _playerMoveSpeedMultiplier * _turboSpeedMultiplier * _wallIsInFrontSpeedMultiplier * _jumpApexModifierCurrentPlayerForce;
        }

        IsGroundedCheck();

        if (moveDir.x < 0 && _playerFacingRight && !_isFrozen) {
            _playerVisual.Flip();
            _playerFacingRight = !_playerFacingRight;
        } else if (moveDir.x > 0 && !_playerFacingRight && !_isFrozen) {
            _playerVisual.Flip();
            _playerFacingRight = !_playerFacingRight;
        }

        TurboHandler();

        if (_canAttack && _isTryingToAttack && _statusState != StatusState.Paralyzed && _statusState != StatusState.Frozen)
        {
            Attack();
        }

        if (!_canAttack && _statusState != StatusState.Paralyzed && _statusState != StatusState.Frozen) {
            _currentCanAttackCooldown += Time.deltaTime;
            if (_currentCanAttackCooldown >= _maxUntilCanAttackCooldown) {
                _canAttack = true;
            }
        }

        if (_isInvincible) {
            _currentInvincibleTime += Time.deltaTime;
            if (!_invincibleAlmostOverHasStarted && _gotInvinciblePowerUp && _checkForInvinciblePowerUpAlmostOver <= _currentInvincibleTime)
            {
                StartInvinciblePowerUpAlmostOverAnim();
            }
            if (_currentInvincibleTime >= _maxInvincibleTime) {
                StopInvincibleTime();
            }
        }

        if(_canStartJumpCoyoteTime)
        {
            _jumpCoyoteCurrentTime -= Time.deltaTime;
            if(_jumpCoyoteCurrentTime < 0)
            {
                _isWithinJumpCoyoteTime = false;
                _canStartJumpCoyoteTime = false;
            }
            else
            {
                _isWithinJumpCoyoteTime = true;
            }
        }

        JumpStateMachine();

        StatusStateMachine();
    }

    private void CheckIfWallIsRightInFront() {
        Vector2 rayOrigin = _playerWallCheck.transform.position;

        Vector2 rayEnd = new Vector2(rayOrigin.x + _wallCheckRayOffset, _playerWallCheck.transform.position.y);
        if (_playerFacingRight) {
            rayEnd.x = rayOrigin.x + _wallCheckRayOffset;
        } else {
            rayEnd.x = rayOrigin.x - _wallCheckRayOffset;
        }

        LayerMask rayLayerMask = _layerIsGrounded;

        RaycastHit2D hit = Physics2D.Linecast(rayOrigin, rayEnd, rayLayerMask);

        if (hit) {
            PlatformEffector2D isNotAWall = hit.transform.gameObject.GetComponent<PlatformEffector2D>();

            if (isNotAWall) {
                _wallIsInFrontSpeedMultiplier = 1f;
                _isWallRightInFrontOfPlayer = false;
            } 
            else {
                _wallIsInFrontSpeedMultiplier = 0f;
                _isWallRightInFrontOfPlayer = true;
            }

        } else {
            _wallIsInFrontSpeedMultiplier = 1f;
            _isWallRightInFrontOfPlayer = false;
        }
    }

    #region StateMachines

    private void JumpStateMachine() {

        switch (_jumpState) {
            case JumpState.Grounded:

                RecoverAirJumpsInstantly();

                if(_isTryingToJump && _canBufferedJumpBeUsed && _canJump)
                {
                    Jump();
                }

                if (!_isGrounded && !_jumpedOnBounceable) {
                    _canStartJumpCoyoteTime = true;
                    _jumpState = JumpState.Falling;
                }

                break;
            case JumpState.Jumping:

                JumpUpperEdgeDetection();

                HandleJumping();

                if (PlayersYVelocity() <= 0 && !_isWithinJumpCoyoteTime && !_isNudgingOfUpperEdges)
                {

                    if (_jumpedOnBounceable)
                    {
                        _jumpedOnBounceable = false;
                    }
                    if (_lowestJumpForce != _lowestJumpForceDefault)
                    {
                        _lowestJumpForce = _lowestJumpForceDefault;
                    }

                    if (_startedUpperEdgeNudging) {
                        _startedUpperEdgeNudging = false;
                    }

                    _jumpState = JumpState.Falling;
                }

                break;
            case JumpState.AirJumping:

                JumpUpperEdgeDetection();

                HandleJumping();

                if (PlayersYVelocity() <= 0 && !_isNudgingOfUpperEdges)
                {
                    if (_startedUpperEdgeNudging) {
                        _startedUpperEdgeNudging = false;
                    }

                    _jumpState = JumpState.Falling;
                }

                break;
            case JumpState.Falling:

                if (_jumpApexModifierCurrentTimeToEnable >= _jumpApexModifierMinimumTimeToEnable)
                {
                    HandleJumpApexModifiers();
                }

                if (_playerRigidBody.velocity.y <= _minFallingYVelocity) {
                    _currentPlayerVelocity.y = _minFallingYVelocity;
                    SetPlayerRigidBodyVelocity(_currentPlayerVelocity);
                }

                if (_jumpTime != 0f)
                {
                    _jumpTime = 0f;
                }

                if (_isTryingToJump && _jumpBufferingCurrentTime <= _jumpBufferingMaxTime)
                {
                    _isBufferingJump = true;
                }

                if (_isBufferingJump)
                {
                    if (_jumpBufferingCurrentTime <= _jumpBufferingMaxTime)
                    {
                        _canBufferedJumpBeUsed = true;
                        _jumpBufferingCurrentTime += Time.deltaTime;
                    }
                    else if (_jumpBufferingCurrentTime > _jumpBufferingMaxTime)
                    {
                        _canBufferedJumpBeUsed = false;
                        _isBufferingJump = false;
                    }
                }

                if (_isGrounded)
                {
                    StopYVelocity();

                    if (_jumpApexModifierPlayed) {
                        _jumpApexModifierPlayed = false;
                    }
                    if (_jumpApexModifierCurrentPlayerForce > _jumpApexModifierDefaultPlayerForce) {
                        _jumpApexModifierCurrentPlayerForce = _jumpApexModifierDefaultPlayerForce;
                    }
                    if (_jumpApexModifierCurrentTime <= 0) {
                        _jumpApexModifierCurrentTime = _jumpApexModifierMaxTime;
                    }

                    if (_jumpCoyoteCurrentTime != _jumpCoyoteMaxTime)
                    {
                        _jumpCoyoteCurrentTime = _jumpCoyoteMaxTime;
                    }

                    _playerSounds.PlayLandingSound();
                    _jumpState = JumpState.Grounded;
                }

                break;
        }

    }

    private void StatusStateMachine() {

        if (!_isPlayerAfflictedWithStatus) {
            _statusState = StatusState.Unafflicted;
        }

        switch (_statusState) {
            case StatusState.Unafflicted:
                break;
            case StatusState.Burned:
                _statusTimer -= Time.deltaTime;
                if (_statusTimer > 0) { return; }
                StopPlayerWasBurned();
                break;
            case StatusState.Paralyzed:
                _canTurbo = false;
                _statusTimer -= Time.deltaTime;
                if (_statusTimer > 0) { return; }
                StopPlayerWasParalyzed();
                break;
            case StatusState.Frozen:
                _isFrozen = true;
                _canJump = false;
                _canTurbo = false;
                _statusTimer -= Time.deltaTime;
                if (_statusTimer > 0) { return; }
                StopPlayerWasFrozen();
                break;
            case StatusState.Poisoned:
                _statusTimer -= Time.deltaTime;
                WhilePlayerIsPoisoned();
                if (_statusTimer > 0) { return; }
                StopPlayerWasPoisoned();
                break;
            case StatusState.Slimed:
                _canTurbo = false;
                _canJump = false;
                _statusTimer -= Time.deltaTime;
                if (_statusTimer > 0) { return; }
                StopPlayerWasSlimed();
                break;
        }
    }

    #endregion

    #region PublicFunctions

    public void BounceOffCrush(float jumpBoostMultiplier) {
        _jumpedOnBounceable = true;
        _jumpState = JumpState.Grounded;
        EnemyManager.Instance.IncreaseKilledEnemiesCounter();
        RecoverTurboSpeedInstantly();
        RecoverAirJumpsInstantly();
        _lowestJumpForce = _lowestJumpForceDefault * jumpBoostMultiplier;
        Jump();
    }

    public void Damage(int enemyAttackPower) {
        if (!GameManager.Instance.IsGamePlaying()) return;


        if (_isInvincible) {
            return;
        }

        _currentHealth -= enemyAttackPower;

        if (_currentHealth <= 0) {
            _currentHealth = 0;
            OnHealthChanged?.Invoke(this, new OnHealthChangedEventArgs {
                health = _currentHealth
            });
            _playerSounds.PlayDeathSound();
            Die();
            return;
        }

        OnHealthChanged?.Invoke(this, new OnHealthChangedEventArgs {
            health = _currentHealth
        });

        _playerSounds.PlayHitSound();
        StartHitInvincibleTime();
    }

    #endregion

    #region PublicPowerUpFunctions

    public void PowerUpHealthRegen(int increaseMaxHealthBy) {
        IncrementMaxHealth(increaseMaxHealthBy);
        RecoverHealthInstantly();
    }

    public void PowerUpTurboTimeDuplication(float multiplyTimeBy) {
        MultiplyTurboTime(multiplyTimeBy);
        RecoverTurboSpeedInstantly();
    }

    public void PowerUpExtraAirJump(int increaseMaxAirJumpsBy) {
        IncrementMaxAvailableAirJumps(increaseMaxAirJumpsBy);
        RecoverAirJumpsInstantly();
    }

    public void PowerUpInvincibility(float changeTimerTo) {
        ModifyInvincibleTime(changeTimerTo);
        _checkForInvinciblePowerUpAlmostOver = changeTimerTo - 3f;
        StartInvinciblePowerUpTime();
    }

    #endregion

    #region JumpRelatedFunctions

    private void Jump()
    {

        if (_jumpState == JumpState.Grounded || (_jumpState == JumpState.Falling && _isWithinJumpCoyoteTime))
        {
            _jumpApexModifierCurrentTimeToEnable = 0f;
            OnPlayerJumped?.Invoke(this, EventArgs.Empty);
            _jumpState = JumpState.Jumping;
        }

        //else if ((_jumpState == JumpState.Falling || _jumpState == JumpState.Jumping) && _currentAvailableAirJumps > 0)
        else if (_jumpState != JumpState.Grounded && _currentAvailableAirJumps > 0) 
        {
            _currentAvailableAirJumps--;
            if (_jumpedOnBounceable)
            {
                _jumpedOnBounceable = false;
            }
            if(_lowestJumpForce != _lowestJumpForceDefault)
            {
                _lowestJumpForce = _lowestJumpForceDefault;
            }
            if (_jumpTime != 0f)
            {
                _jumpTime = 0f;
            }
            _jumpApexModifierCurrentTimeToEnable = 0f;
            OnPlayerAirJumped?.Invoke(this, EventArgs.Empty);
            OnAirJumpCounterChanged?.Invoke(this, new OnAirJumpCounterChangedEventArgs
            {
                airJumps = _currentAvailableAirJumps
            });
            _jumpState = JumpState.AirJumping;
        }

        if (_jumpBufferingCurrentTime != _jumpBufferingMaxTime)
        {
            _canBufferedJumpBeUsed = false;
            _isBufferingJump = false;
            _jumpBufferingCurrentTime = 0f;
        }

    }

    private void IsGroundedCheck() {
        if (Physics2D.OverlapCircle(_playerFeetPos.position, _checkFeetRadius, _layerIsGrounded))
        {
            _isGrounded = true;
        }
        else if (Physics2D.OverlapCircle(_playerFeetPos.position, _checkFeetRadius, _layerPlatforms))
        {
            _isGrounded = true;
        }
        else 
        {
            _isGrounded = false;
        }
    }

    private void HandleJumping()
    {

        if (_jumpApexModifierPlayed)
        {
            _jumpApexModifierPlayed = false;
        }
        if (_jumpApexModifierCurrentPlayerForce > _jumpApexModifierDefaultPlayerForce)
        {
            _jumpApexModifierCurrentPlayerForce = _jumpApexModifierDefaultPlayerForce;
        }
        if (_jumpApexModifierCurrentTime <= 0)
        {
            _jumpApexModifierCurrentTime = _jumpApexModifierMaxTime;
        }

        if (_isTryingToJump)
        {
            _jumpApexModifierCurrentTimeToEnable += Time.deltaTime;
        }

        if (!_isNudgingOfUpperEdges) {
            _jumpTime += Time.deltaTime;
        }

        if (_jumpedOnBounceable)
        {
            HandleInstantJump();
        }

        if (_jumpTime <= _maxJumpTime && _isTryingToJump && !_jumpedOnBounceable)
        {
            HandleJumpPressingOverTime();
        }
    }

    private void HandleJumpApexModifiers()
    {
        if (_jumpApexModifierPlayed)
        {
            _jumpApexModifierCurrentTimeToEnable = 0f;
            return;
        }

        _jumpApexModifierCurrentTime -= Time.deltaTime;

        if (_jumpApexModifierCurrentTime > 0)
        {
            StopYVelocity();

            if (_jumpApexModifierCurrentPlayerForce != _jumpApexModifierMaxPlayerForce)
            {
                _jumpApexModifierCurrentPlayerForce = _jumpApexModifierMaxPlayerForce;
            }

            return;
        }
        _jumpApexModifierCurrentPlayerForce = _jumpApexModifierDefaultPlayerForce;
        _jumpApexModifierPlayed = true;
    }

    private void IncrementMaxAvailableAirJumps(int incrementBy) {
        _maxAvailableAirJumps += incrementBy;
    }

    private void RecoverAirJumpsInstantly() {
        if (_currentAvailableAirJumps < _maxAvailableAirJumps) {
            _currentAvailableAirJumps = _maxAvailableAirJumps;
            OnAirJumpCounterChanged?.Invoke(this, new OnAirJumpCounterChangedEventArgs {
                airJumps = _currentAvailableAirJumps
            });
        }
    }

    private void HandleInstantJump()
    {
        _currentJumpForce = _lowestJumpForce;

        _currentPlayerVelocity.y = _currentJumpForce;
        SetPlayerRigidBodyVelocity(_currentPlayerVelocity);

        if (_jumpedOnBounceable)
        {
            _jumpApexModifierCurrentTimeToEnable = _jumpApexModifierMinimumTimeToEnable;
            _jumpedOnBounceable = false;
        }
    }

    private void HandleJumpPressingOverTime()
    {
        float normalizedJumpTime = Mathf.Clamp01(_jumpTime / _maxJumpTime);
        _currentJumpForce = Mathf.Lerp(_lowestJumpForce, _highestJumpForce, normalizedJumpTime);

        _currentPlayerVelocity.y = _currentJumpForce;
        SetPlayerRigidBodyVelocity(_currentPlayerVelocity);
    }

    private void JumpSideEdgeDetection() {
        Vector2 rayOrigin = _playerSideEdgeDetectionCheck.transform.position;

        Vector2 rayEnd = new Vector2(rayOrigin.x + _wallCheckRayOffset, _playerSideEdgeDetectionCheck.transform.position.y);
        if (_playerFacingRight) {
            rayEnd.x = rayOrigin.x + _wallCheckRayOffset;
        } else {
            rayEnd.x = rayOrigin.x - _wallCheckRayOffset;
        }

        LayerMask rayLayerMask = _layerIsGrounded;

        RaycastHit2D hit = Physics2D.Linecast(rayOrigin, rayEnd, rayLayerMask);

        if (!hit && _jumpState == JumpState.Falling) {
            StopYVelocity();
            JumpSideEdgeNudge();
        }
    }

    private void JumpSideEdgeNudge() {
        transform.Translate(Vector2.up * _jumpNudgeSpeed);
        if (!_isPlayerRunning) {
            if (_playerFacingRight) {
                transform.Translate(Vector2.right * _jumpNudgeSpeed);
            }
            else if (!_playerFacingRight) {
                transform.Translate(Vector2.left * _jumpNudgeSpeed);
            }
        }
    }

    private void JumpUpperEdgeDetection() {

        bool leftHit;
        bool rightHit;

        if (Physics2D.OverlapCircle(_playerUpperEdgeDetectionLeftCheck.position, _jumpUpperEdgeDetectionRadius, _layerIsGrounded)) {
            leftHit = true;
        } else {
            leftHit = false;
        }

        if (Physics2D.OverlapCircle(_playerUpperEdgeDetectionRightCheck.position, _jumpUpperEdgeDetectionRadius, _layerIsGrounded)) {
            rightHit = true;
        } else {
            rightHit = false;
        }

        if (leftHit && !rightHit) {

            PlatformEffector2D leftIsAPlatform = Physics2D.OverlapCircle(_playerUpperEdgeDetectionLeftCheck.position, _jumpUpperEdgeDetectionRadius, _layerIsGrounded).transform.gameObject.GetComponent<PlatformEffector2D>();

            if (leftIsAPlatform == null && !_playerFacingRight) {
                JumpUpperEdgeLeftNudge();
            }
            if (leftIsAPlatform == null && _playerFacingRight) {
                JumpUpperEdgeRightNudge();
            }
            if (leftIsAPlatform != null) {
                if (_startedUpperEdgeNudging) {
                    _startedUpperEdgeNudging = false;
                }
                _isNudgingOfUpperEdges = false;
            }

        }
        if (!leftHit && rightHit) {

            PlatformEffector2D rightIsAPlatform = Physics2D.OverlapCircle(_playerUpperEdgeDetectionRightCheck.position, _jumpUpperEdgeDetectionRadius, _layerIsGrounded).transform.gameObject.GetComponent<PlatformEffector2D>();

            if (rightIsAPlatform == null && !_playerFacingRight) {
                JumpUpperEdgeRightNudge();
            }
            if (rightIsAPlatform == null && _playerFacingRight) {
                JumpUpperEdgeLeftNudge();
            }
            if (rightIsAPlatform != null) {
                if (_startedUpperEdgeNudging) {
                    _startedUpperEdgeNudging = false;
                }
                _isNudgingOfUpperEdges = false;
            }

        }
        if ((!leftHit && !rightHit) || (leftHit && rightHit)) {
            if (_startedUpperEdgeNudging) {
                _startedUpperEdgeNudging = false;
            }
            _isNudgingOfUpperEdges = false;
        }

    }

    private void JumpUpperEdgeLeftNudge() {
        if (!_startedUpperEdgeNudging) {

            _currentPlayerVelocity.y = _playerRigidBody.velocity.y;
            _startedUpperEdgeNudging = true;
        }
        _isNudgingOfUpperEdges = true;
        SetPlayerRigidBodyVelocity(_currentPlayerVelocity);
        if (!_isPlayerRunning) {
            transform.Translate(Vector2.left * _jumpNudgeSpeed);
        }
        
    }

    private void JumpUpperEdgeRightNudge() {
        if (!_startedUpperEdgeNudging) {

            _currentPlayerVelocity.y = _playerRigidBody.velocity.y;
            _startedUpperEdgeNudging = true;
        }
        _isNudgingOfUpperEdges = true;
        SetPlayerRigidBodyVelocity(_currentPlayerVelocity);
        if (!_isPlayerRunning) {
            transform.Translate(Vector2.right * _jumpNudgeSpeed);
        }
    }

    private void StopYVelocity() {
        _currentPlayerVelocity.y = 0f;
        SetPlayerRigidBodyVelocity(_currentPlayerVelocity);
    }

    #endregion

    #region SpeedRelatedFunctions

    private void SetPlayerRigidBodyVelocity(Vector2 newPlayerVelocity) {
        _playerRigidBody.velocity = newPlayerVelocity;
    }

    private void SetPlayerMovementVelocity(float speedMultiplier) {
        _playerMoveSpeedMultiplier = speedMultiplier;
        OnRunAnimSpeedChange?.Invoke(this, new OnRunAnimSpeedChangeEventArgs {
            runAnimSpeedMultiplier = _turboSpeedMultiplier * speedMultiplier
        });
        if (speedMultiplier != _playerMoveSpeedMultiplierDefault) {
            _maxUntilCanAttackCooldown /= _turboSpeedMultiplier + speedMultiplier;
        } else _maxUntilCanAttackCooldown = _playerMoveSpeedMultiplierDefault * _turboSpeedMultiplier;
    }

    private void TurboHandler() {
        if (_isTryingToTurbo && _canTurbo)
        {
            _currentTurboTime -= Time.deltaTime;
            OnTurboTimeChanged?.Invoke(this, new OnTurboTimeChangedEventArgs
            {
                turboTime = _currentTurboTime
            });

            if (_currentTurboTime > 0)
            {
                SetPlayerTurboVelocity(_turboSpeedMultiplierDefault);
            }
            else _canTurbo = false;
            return;
        }

        SetPlayerTurboVelocity(_turboSpeedMultiplierWhileNotTurboing);

        if (!_canTurbo || !_isTryingToTurbo)
        {
            RecoverTurboSpeedOverTime();
            if (_currentTurboTime > 0 && _statusState != StatusState.Paralyzed && _statusState != StatusState.Frozen && _statusState != StatusState.Slimed)
            {
                _canTurbo = true;
            }
        }
    }

    private void SetPlayerTurboVelocity(float turboMultiplier)
    {
        _turboSpeedMultiplier = turboMultiplier;
        OnRunAnimSpeedChange?.Invoke(this, new OnRunAnimSpeedChangeEventArgs
        {
            runAnimSpeedMultiplier = _playerMoveSpeedMultiplier * turboMultiplier
        });
        if (turboMultiplier != _turboSpeedMultiplierWhileNotTurboing)
        {
            _maxUntilCanAttackCooldown /= _playerMoveSpeedMultiplier + turboMultiplier;
        }
        else _maxUntilCanAttackCooldown = _playerMoveSpeedMultiplier * _turboSpeedMultiplierWhileNotTurboing;
    }

    private void MultiplyTurboTime(float multiplyBy) {
        _maxTurboTime *= multiplyBy;
    }

    private void RecoverTurboSpeedOverTime() {

        OnTurboTimeChanged?.Invoke(this, new OnTurboTimeChangedEventArgs
        {
            turboTime = _currentTurboTime
        });

        if (_isTryingToTurbo)
        {
            return;
        }

        if (_currentTurboTime >= _maxTurboTime) {
            _currentTurboTime = _maxTurboTime;
            return;
        }

        _currentTurboCooldownTime += Time.deltaTime;

        if (_currentTurboCooldownTime >= _cooldownUntilStartingTurboRefill && _currentTurboTime < _maxTurboTime) {
            _currentTurboTime += Time.deltaTime;
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

    public float PlayersYVelocity() {
        return _playerRigidBody.velocity.y;
    }

    public float PlayersYPosition() {
        return transform.position.y;
    }

    public bool IsPlayerRunning() {
        if (!_isPlayerRunning) {
            return false;
        }

        return true;
    }

    public bool IsPlayerGrounded()
    {
        if (!_isGrounded)
        {
            return false;
        }

        return true;
    }

    public bool IsWallRightInFrontOfPlayer() {
        if (!_isWallRightInFrontOfPlayer) {
            return false;
        }

        return true;
    }

    public float MaxTurboTime() {
        return _maxTurboTime;
    }

    #endregion

    #region HealthRelatedFunctions

    private void IncrementMaxHealth(int incrementAmount) {
        if (_maxHealth == _totalMaxHealth) {
            return;
        }

        _maxHealth += incrementAmount;

        if (_maxHealth > _totalMaxHealth) {
            _maxHealth = _totalMaxHealth;
        }
    }

    private void RecoverHealthInstantly() {
        if (_currentHealth != _maxHealth) {
            _currentHealth = _maxHealth;
            OnHealthChanged?.Invoke(this, new OnHealthChangedEventArgs {
                health = _currentHealth
            });
        }
    }

    private void Die() {
        if (_isPlayerAfflictedWithStatus) {
            StopStatusConditions();
        }
        _playerVisual.PlayDeathAnim();
        StopPlayer();
    }

    private void StopPlayer() {
        _isDead = true;
        _playerRigidBody.constraints = RigidbodyConstraints2D.FreezePosition;
    }

    #endregion

    #region AttackRelatedFunctions

    private void Attack() {
        _playerAttack.SetAttackPower(_currentAttackPower);
        OnPlayerAttacked?.Invoke(this, EventArgs.Empty);
        _currentCanAttackCooldown = 0f;
        _canAttack = false;
    }

    private void ChangeAttackPower(float attackMultiplier) {
        _currentAttackPower = (int)(_maxAttackPower * attackMultiplier);
    }

    private void PlayerKilledEnemy() {
        EnemyManager.Instance.IncreaseKilledEnemiesCounter();
        RecoverTurboSpeedInstantly();
    }

    #endregion

    #region StatusFunctions

    private void StartInvinciblePowerUpTime() {
        _gotInvinciblePowerUp = true;
        StopStatusConditions();
        SetInvincibleBool(true);
        _invincibleTouchAttackTrigger.SetActive(true);
        SetPlayerMovementVelocity(_invinciblePowerUpSpeedMultiplier);
        _playerVisual.PlayInvincibleAnim();
    }

    private void StartInvinciblePowerUpAlmostOverAnim() {
        _playerVisual.PlayInvincibleAnimAlmostOver();
        _invincibleAlmostOverHasStarted = true;
    }

    private void StartHitInvincibleTime()
    {
        SetInvincibleBool(true);
        _playerVisual.PlayHitAnim();
    }

    private void StopInvincibleTime() {
        SetInvincibleBool(false);
        _currentInvincibleTime = 0f;
        _maxInvincibleTime = _maxInvincibleTimeDefault;
        if (_gotInvinciblePowerUp) {
            _playerVisual.StopInvincibleAnim();
            SetPlayerMovementVelocity(_playerMoveSpeedMultiplierDefault);
            _invincibleTouchAttackTrigger.SetActive(false);
            _gotInvinciblePowerUp = false;
            _invincibleAlmostOverHasStarted = false;
            return;
        }
        _playerVisual.StopHitAnim();

    }

    private void ModifyInvincibleTime(float modifyTo) {
        _maxInvincibleTime = modifyTo;
    }

    private void SetInvincibleBool(bool isInvincible) {
        _isInvincible = isInvincible;
    }

    private void StopStatusConditions() {
        _statusTimer = 0f;
        
        if (_statusState == StatusState.Burned)
        {
            StopPlayerWasBurned();
        }
        if (_statusState == StatusState.Paralyzed)
        {
            StopPlayerWasParalyzed();
        }
        if (_statusState == StatusState.Frozen)
        {
            StopPlayerWasFrozen();
        }
        if (_statusState == StatusState.Poisoned)
        {
            StopPlayerWasPoisoned();
        }
        if (_statusState == StatusState.Slimed)
        {
            StopPlayerWasSlimed();
        }
    }

    public void PlayerWasBurned() {
        if (_gotInvinciblePowerUp || _isDead) { return; }
        StopStatusConditions(); 
        _playerVisual.BurnPlayerStart();
        _playerSounds.StartBurnConditionSound();
        _statusTimer = _maxBurnedTime;
        _isPlayerAfflictedWithStatus = true;
        ChangeAttackPower(_burnedAttackMultiplier);
        SetPlayerMovementVelocity(_burnedSpeedMultiplier);
        _statusState = StatusState.Burned;
    }

    private void StopPlayerWasBurned()
    {
        SetPlayerMovementVelocity(_playerMoveSpeedMultiplierDefault);
        _maxAttackPower = _maxAttackPowerDefault;
        ChangeAttackPower(1);
        _playerSounds.StopAnyStatusConditionSound();
        _playerVisual.BurnPlayerStop();
        _isPlayerAfflictedWithStatus = false;
    }

    public void PlayerWasParalyzed() {
        if (_gotInvinciblePowerUp || _isDead) { return; }
        StopStatusConditions();
        _playerVisual.ParalyzePlayerStart();
        _playerSounds.StartParalyzedConditionSound();
        _statusTimer = _maxParalyzedTime;
        _isPlayerAfflictedWithStatus = true;
        SetPlayerMovementVelocity(_paralyzedSpeedMultiplier);
        _statusState = StatusState.Paralyzed;
    }

    private void StopPlayerWasParalyzed()
    {
        SetPlayerMovementVelocity(_playerMoveSpeedMultiplierDefault);
        _maxTurboTime = _maxTurboTimeDefault;
        _canTurbo = true;
        _playerSounds.StopAnyStatusConditionSound();
        _playerVisual.ParalyzePlayerStop();
        _isPlayerAfflictedWithStatus = false;
    }

    public void PlayerWasFrozen() {
        if (_gotInvinciblePowerUp || _isDead) { return; }
        StopStatusConditions();
        _playerVisual.FreezePlayerStart();
        _playerSounds.StartFrozenConditionSound();
        _statusTimer = _maxFrozenTime;
        _isPlayerAfflictedWithStatus = true;
        _statusState = StatusState.Frozen;
    }

    private void StopPlayerWasFrozen()
    {
        _playerVisual.FreezePlayerStop();
        _playerSounds.StopAnyStatusConditionSound();
        _isFrozen = false;
        _canJump = true;
        _canTurbo = true;
        _isPlayerAfflictedWithStatus = false;
    }

    public void PlayerWasPoisoned() {
        if (_gotInvinciblePowerUp || _isDead) { return; }
        StopStatusConditions();
        _playerVisual.PoisonPlayerStart();
        _playerSounds.StartPoisonConditionSound();
        _statusTimer = _maxPoisonedTime;
        _isPlayerAfflictedWithStatus = true;
        _currentHealthToDecrease = Mathf.RoundToInt(_currentHealth / _poisonHealthDivider);
        if (_currentHealthToDecrease <= 0) { _currentHealthToDecrease = 1; }
        _statusState = StatusState.Poisoned;
    }

    private void WhilePlayerIsPoisoned() {
        _currentPoisonTimer -= Time.deltaTime;

        if (_currentPoisonTimer <= 0f) {
            _currentHealth -= _currentHealthToDecrease;
            OnHealthChanged?.Invoke(this, new OnHealthChangedEventArgs
            {
                health = _currentHealth
            });

            if (_currentHealth <= 0)
            {
                _currentHealth = 0;
                OnHealthChanged?.Invoke(this, new OnHealthChangedEventArgs
                {
                    health = _currentHealth
                });
                Die();
                return;
            }

            OnHealthChanged?.Invoke(this, new OnHealthChangedEventArgs
            {
                health = _currentHealth
            });

            _currentHealthToDecrease *= _healthToDecreaseMultiplier;
            _currentPoisonTimer = _defaultPoisonTimer;
        }

    }

    private void StopPlayerWasPoisoned()
    {
        _playerSounds.StopAnyStatusConditionSound();
        _playerVisual.PoisonPlayerStop();
        _currentPoisonTimer = _defaultPoisonTimer;
        _isPlayerAfflictedWithStatus = false;
    }

    public void PlayerWasSlimed() {
        if (_gotInvinciblePowerUp || _isDead) { return; }
        StopStatusConditions();
        _playerVisual.SlimePlayerStart();
        _playerSounds.StartSlimeConditionSound();
        _statusTimer = _maxSlimedTime;
        _isPlayerAfflictedWithStatus = true;
        SetPlayerMovementVelocity(_slimedSpeedMultiplier);
        _statusState = StatusState.Slimed;
    }

    private void StopPlayerWasSlimed()
    {
        SetPlayerMovementVelocity(_playerMoveSpeedMultiplierDefault);
        _canJump = true;
        _maxTurboTime = _maxTurboTimeDefault;
        _canTurbo = true;
        _playerSounds.StopAnyStatusConditionSound();
        _playerVisual.SlimePlayerStop();
        _isPlayerAfflictedWithStatus = false;
    }

    #endregion

    private void OnDestroy() {

        GameInput.Instance.OnJumpStarted -= Instance_OnJumpStarted;
        GameInput.Instance.OnJumpPerformed -= Instance_OnJumpPressed;
        GameInput.Instance.OnJumpCanceled -= Instance_OnJumpCanceled;

        GameInput.Instance.OnTurboPressed -= Instance_OnTurboPressed;
        GameInput.Instance.OnTurboCanceled -= Instance_OnTurboCanceled;

        GameInput.Instance.OnAttackHeld -= Instance_OnAttackPressed;
        GameInput.Instance.OnAttackReleased -= Instance_OnAttackReleased;

        _playerAttack.OnKillingEnemy -= _playerAttack_OnKillingEnemy;
        _playerTouchAttack.OnInvincibleTouchKillingEnemy -= _playerTouchAttack_OnInvincibleTouchKillingEnemy;

    }

}
