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

    [SerializeField] private bool _isFacingRight;
    private Vector2 _moveDir;

    protected int _currentHealth;

    private bool _canIWalk;

    [SerializeField] private Transform _wallCheck;
    [SerializeField] private Transform _edgeCheck;
    [SerializeField] private LayerMask _layerIsGrounded;
    private float _checkRadius = 0.1f;

    private void Awake() {
        _myRigidBody = GetComponent<Rigidbody2D>();
    }

    protected virtual void Start() {
        
        _currentHealth = _mySO.health;

        if (_isFacingRight) {
            _myVisual.Flip();
        }
        CanIWalk(true);
    }

    private void FixedUpdate() {

        if (_isFacingRight && _moveDir.x <= 0) {
            _moveDir = new Vector2(1f, 0f);
            _myVisual.Flip();
        } else if (!_isFacingRight && _moveDir.x >= 0) {
            _moveDir = new Vector2(-1f, 0f);
            _myVisual.Flip();
        }

        if (!_canIWalk) {
            
            return;
        }


        transform.position = (Vector2)transform.position + _mySO.speed * Time.deltaTime * _moveDir;


        if (!Physics2D.OverlapCircle(_edgeCheck.position, _checkRadius, _layerIsGrounded) || Physics2D.OverlapCircle(_wallCheck.position, _checkRadius, _layerIsGrounded)) {
            _isFacingRight = !_isFacingRight;
        };

    }

    private void OnTriggerEnter2D(Collider2D collision) {
        PlayerScript player = collision.GetComponent<PlayerScript>();
        if (player != null) {
            player.Damage(_mySO.touchPower);
        }
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

    public int TouchAttackPower() {
        return _mySO.touchPower;
    }
}
