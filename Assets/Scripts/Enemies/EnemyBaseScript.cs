using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBaseScript : MonoBehaviour, IDamageable
{
    [SerializeField] protected EnemySO _mySO;
    [SerializeField] protected EnemyVisualBaseScript _myVisual;
    [SerializeField] private Collider2D _touchAttackTrigger;


    protected Rigidbody2D _myRigidBody;
    protected Collider2D _myCollider;

    [SerializeField] protected bool _isFacingRight;
    private Vector2 _moveDir;

    protected int _currentHealth;

    protected bool _canIWalk;

    [SerializeField] private Transform _wallCheck;
    [SerializeField] private Transform _edgeCheck;
    [SerializeField] private LayerMask _layerIsGrounded;
    private float _checkRadius = 0.1f;


    private bool _canAttack;
    protected float _attackCooldownTime = 3f;
    protected float _currentAttackCooldownTime;


    protected virtual void Awake() {
        _myRigidBody = GetComponent<Rigidbody2D>();
        _canAttack = true;
    }

    protected virtual void Start() {
        
        _currentHealth = _mySO.health;

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

        if (!Physics2D.OverlapCircle(_edgeCheck.position, _checkRadius, _layerIsGrounded) || Physics2D.OverlapCircle(_wallCheck.position, _checkRadius, _layerIsGrounded))
        {
            _isFacingRight = !_isFacingRight;
        };
    }

    public virtual void Attack() {
        if (!_canAttack) { return; }
        _myVisual.Attack();
        _currentAttackCooldownTime = 0f;
        _canAttack = false;
    }

    public virtual void Damage(int attackPower) {
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

    public bool IsFacingRight() {
        return _isFacingRight;
    }
}
