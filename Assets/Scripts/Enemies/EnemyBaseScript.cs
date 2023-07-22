using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBaseScript : MonoBehaviour, IDamageable
{
    #region Variables

    [SerializeField] protected EnemySO _mySO;
    [SerializeField] protected EnemyVisualBaseScript _myVisual;
    [SerializeField] protected EnemySoundsBaseScript _mySFXObject;
    [SerializeField] private Collider2D _touchAttackTrigger;
    [SerializeField] private Collider2D _attackTrigger;

    protected Rigidbody2D _myRigidBody;
    protected Collider2D _myCollider;

    protected bool _isOriginallyFacingRight;
    [SerializeField] protected bool _isFacingRight;
    protected Vector2 _moveDir;

    protected int _currentHealth;
    private bool _isDead;

    protected bool _canIWalk;

    [SerializeField] protected Transform _wallCheck;
    [SerializeField] protected Transform _edgeCheck;
    [SerializeField] protected LayerMask _layerIsGrounded;
    [SerializeField] protected LayerMask _layerPlatforms;
    protected float _checkRadius = 0.1f;

    protected bool _isEnemyWalking;

    protected bool _canAttack;
    protected float _attackCooldownTime;
    protected float _currentAttackCooldownTime;

    [SerializeField] private bool _isEnemyRespawnable;
    private bool _wasIRespawnedOnce = false;
    private Transform _myOriginalPosition;
    private float _currentTimeUntilRespawn;
    private float _maxTimeUntilRespawn = 3f;
    private int _mandatoryDamageForProperRespawn = 0;

    #endregion

    #region ConstantEnemyMethods

    protected virtual void Awake() {
        _myRigidBody = GetComponent<Rigidbody2D>();
    }

    protected virtual void Start() {

        if (_isEnemyRespawnable)
        {
            _currentTimeUntilRespawn = _maxTimeUntilRespawn;

            if (!_wasIRespawnedOnce) {
                _myOriginalPosition = transform;
            }

        }

        SpawnEnemy();

        if (!_isFacingRight) {
            _myVisual.Flip();
        }

        _myVisual.OnEnemyHitAnimStarted += _myVisual_OnEnemyHitAnimStarted;
        _myVisual.OnEnemyHitAnimStopped += _myVisual_OnEnemyHitAnimStopped;

        _myVisual.OnEnemyRespawnStarted += _myVisual_OnEnemyRespawnStarted;
    }

    private void _myVisual_OnEnemyHitAnimStopped(object sender, EventArgs e) {
        if (_mySO.isAttacker) {
            SetAttackTrigger(true);
        }
        SetTouchAttackTrigger(true);
        if (_mySO.isWalker) {
            CanIWalk(true);
        }
    }

    protected virtual void _myVisual_OnEnemyHitAnimStarted(object sender, EventArgs e) {
        if (_mySO.isAttacker) {
            SetAttackTrigger(false);
        }
        SetTouchAttackTrigger(false);
        if (_mySO.isWalker) {
            CanIWalk(false);
        }
    }

    private void _myVisual_OnEnemyRespawnStarted(object sender, EventArgs e)
    {
        _myVisual.gameObject.SetActive(false);
    }

    protected virtual void FixedUpdate() {
        if (!GameManager.Instance.IsGamePlaying()) return;

        if(_isDead && _isEnemyRespawnable)
        {
            _currentTimeUntilRespawn -= Time.deltaTime;
            if(_currentTimeUntilRespawn <= 0)
            {
                RespawnEnemy();
                _currentTimeUntilRespawn = 1000f;
            }
        }

        if (_mySO.isAttacker) {

            if (_currentAttackCooldownTime <= 0) {
                _canAttack = true;
            }

            if (!_canAttack) {
                _currentAttackCooldownTime -= Time.deltaTime;
            }

        }

        Walk();
    }

    protected virtual void Walk()
    {
        if (_canIWalk) {

            if (_moveDir.x == 0f) {
                _isEnemyWalking = false;
            } else _isEnemyWalking = true;

            CheckToFlipIfIsEdgeOrHasWall();

            if (_isFacingRight) {
                _moveDir = new Vector2(1f, 0f);
            } else if (!_isFacingRight) {
                _moveDir = new Vector2(-1f, 0f);
            }

            transform.position = (Vector2)transform.position + _mySO.speed * Time.deltaTime * _moveDir;
        }
    }

    protected virtual void CheckToFlipIfIsEdgeOrHasWall() {
        if ((!Physics2D.OverlapCircle(_edgeCheck.position, _checkRadius, _layerIsGrounded) && !Physics2D.OverlapCircle(_edgeCheck.position, _checkRadius, _layerPlatforms)) || Physics2D.OverlapCircle(_wallCheck.position, _checkRadius, _layerIsGrounded)) {
            _isFacingRight = !_isFacingRight;
            _myVisual.Flip();
        };
    }

    protected virtual void Attack() {
        if (!_mySO.isAttacker) { return; }
        if (!_canAttack) { return; }
        _myVisual.Attack();
        _mySFXObject.PlayAttackSound();
        _currentAttackCooldownTime = _attackCooldownTime;
        _canAttack = false;
    }

    public virtual void Damage(int attackPower) {
        if (!GameManager.Instance.IsGamePlaying()) return;
        _currentHealth -= attackPower;
        DeadCheck();
    }

    public virtual bool DeadCheck() {

        if (_currentHealth > 0) {
            _myVisual.PlayHitAnim();
            _mySFXObject.PlayHitSound();
            _isDead = false;
            return _isDead;
        }

        if (_mySO.isWalker)
        {
            CanIWalk(false);
        }

        SetAttackTrigger(false);

        SetTouchAttackTrigger(false);
        
        if(_isEnemyRespawnable)
        {
            _myVisual.PlayDeadAnim();
            _currentTimeUntilRespawn = _maxTimeUntilRespawn;
            _isDead = true;
            return _isDead;
        }

        _myVisual.PlayDeadAnim();
        _mySFXObject.PlayDeathSound();
        _isDead = true;
        return _isDead;
    }

    #endregion

    #region SpecificEnemyMethods

    protected void CanIWalk(bool canIWalk) {
        _canIWalk = canIWalk;
    }

    protected void SetTouchAttackTrigger(bool isActive) {
        _touchAttackTrigger.gameObject.SetActive(isActive);
    }

    protected void SetAttackTrigger(bool isActive) {
        if (_mySO.isAttacker && _attackTrigger != null) {
            _attackTrigger.gameObject.SetActive(isActive);
        }
    }

    public float EnemyBounceOffMultiplier() {
        return _mySO.bounceOffMultiplier;
    }

    public bool CanBurn() {
        return _mySO.canBurn;
    }

    public bool CanParalyze() {
        return _mySO.canParalyze;
    }

    public bool CanFreeze()
    {
        return _mySO.canFreeze;
    }

    public bool CanPoison()
    {
        return _mySO.canPoison;
    }

    public bool CanSlime()
    {
        return _mySO.canSlime;
    }

    public int TouchAttackPower() {
        return _mySO.touchPower;
    }

    public int AttackPower() {
        return _mySO.attackPower;
    }

    public int MyMaxHealth() {
        return _mySO.health;
    }

    public bool IsFacingRight() {
        return _isFacingRight;
    }

    public virtual float PlayersCurrentXLocation() {
        return transform.position.x;
    }

    protected virtual void SetPlayerRadiusCheckTrigger(bool isActive) {
    }

    public virtual bool IsEnemyWalking() {
        return _isEnemyWalking;
    }

    #endregion

    private void SpawnEnemy()
    {
        _isDead = false;

        if (_mySO.isWalker)
        {
            CanIWalk(true);
        }
        else CanIWalk(false);

        _canAttack = true;
        _currentHealth = _mySO.health;

        _attackCooldownTime = _mySO.attackCooldownTime;
        _currentAttackCooldownTime = 0f;

        if (!_myVisual.isActiveAndEnabled) {
            _myVisual.gameObject.SetActive(true);
            transform.SetPositionAndRotation(_myOriginalPosition.position, _myOriginalPosition.rotation);
        }

        if (!_isOriginallyFacingRight) {
            _myVisual.Flip();
        }
    }

    protected virtual void RespawnEnemy()
    {
        
        if (!_wasIRespawnedOnce)
        {
            _wasIRespawnedOnce = true;
        }

        if (_mySO.isAttacker && _attackTrigger != null)
        {
            SetAttackTrigger(true);
        }

        SetTouchAttackTrigger(true);

        SpawnEnemy();

        Damage(_mandatoryDamageForProperRespawn);
        _mySFXObject.PlayHitSound();
    }

    public bool IsEnemyRespawnable()
    {
        return _isEnemyRespawnable;
    }

    public bool WasEnemyRespawnedBefore()
    {
        return _wasIRespawnedOnce;
    }
}
