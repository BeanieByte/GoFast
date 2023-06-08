using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBaseScript : MonoBehaviour, IDamageable
{
    [SerializeField] protected EnemySO _mySO;
    [SerializeField] protected EnemyVisualBaseScript _myVisual;
    [SerializeField] private Collider2D _touchAttackTrigger;
    [SerializeField] private Collider2D _attackTrigger;


    protected Rigidbody2D _myRigidBody;
    protected Collider2D _myCollider;

    protected bool _isOriginallyFacingRight;
    [SerializeField] protected bool _isFacingRight;
    protected Vector2 _moveDir;

    protected int _currentHealth;

    protected bool _canIWalk;

    [SerializeField] protected Transform _wallCheck;
    [SerializeField] protected Transform _edgeCheck;
    [SerializeField] protected LayerMask _layerIsGrounded;
    protected float _checkRadius = 0.1f;

    protected bool _isEnemyWalking;

    protected bool _canAttack;
    protected float _attackCooldownTime = 3f;
    protected float _currentAttackCooldownTime;


    protected virtual void Awake() {
        _myRigidBody = GetComponent<Rigidbody2D>();
        _canAttack = true;
    }

    protected virtual void Start() {
        
        _currentHealth = _mySO.health;

        if (!_isOriginallyFacingRight) {
            _myVisual.Flip();
        }

        if (!_isFacingRight) {
            _myVisual.Flip();
        }

        CanIWalk(true);
    }

    protected virtual void FixedUpdate() {
        if (!GameManager.Instance.IsGamePlaying()) return;

        //if (_isFacingRight && _moveDir.x <= 0) {
        //    _moveDir = new Vector2(1f, 0f);
        //    _myVisual.Flip();
        //} else if (!_isFacingRight && _moveDir.x >= 0) {
        //    _moveDir = new Vector2(-1f, 0f);
        //    _myVisual.Flip();
        //}

        if (_currentAttackCooldownTime >= _attackCooldownTime) {
            _canAttack = true;
        }


        if (!_canAttack) {
            _currentAttackCooldownTime += Time.deltaTime;
        }


        if (!_canIWalk) {
            return;
        }

        if (_moveDir.x == 0f) {
            _isEnemyWalking = false;
        } else _isEnemyWalking = true;

        Walk();
        
        
    }


    protected virtual void Walk()
    {
        if (!_isFacingRight && _moveDir.x <= 0) {
            _moveDir = new Vector2(1f, 0f);
            _myVisual.Flip();
        } else if (_isFacingRight && _moveDir.x >= 0) {
            _moveDir = new Vector2(-1f, 0f);
            _myVisual.Flip();
        }

        transform.position = (Vector2)transform.position + _mySO.speed * Time.deltaTime * _moveDir;

        CheckToFlipIfIsEdgeOrHasWall();
    }

    protected virtual void CheckToFlipIfIsEdgeOrHasWall() {
        if (!Physics2D.OverlapCircle(_edgeCheck.position, _checkRadius, _layerIsGrounded) || Physics2D.OverlapCircle(_wallCheck.position, _checkRadius, _layerIsGrounded)) {
            _isFacingRight = !_isFacingRight;
        };
    }

    protected virtual void Attack() {
        if (!_canAttack) { return; }
        _myVisual.Attack();
        _currentAttackCooldownTime = 0f;
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
            return false;
        }
        _myRigidBody.constraints = RigidbodyConstraints2D.FreezePosition;
        EnemyManager.Instance.IncreaseKilledEnemiesCounter();

        _myVisual.PlayDeadAnim();
        return true;
    }

    public void CanIWalk(bool canIWalk) {
        _canIWalk = canIWalk;
    }

    public void SetTouchAttackTrigger(bool isActive) {
        _touchAttackTrigger.gameObject.SetActive(isActive);
    }

    public void SetAttackTrigger(bool isActive) {
        _attackTrigger.gameObject.SetActive(isActive);
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

    public bool IsFacingRight() {
        return _isFacingRight;
    }

    public virtual float PlayersCurrentXLocation() {
        return transform.position.x;
    }

    public virtual void SetPlayerRadiusCheckTrigger(bool isActive) {
    }

    public virtual bool IsEnemyWalking() {
        return _isEnemyWalking;
    }
}
