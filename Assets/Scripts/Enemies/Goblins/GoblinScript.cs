using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinScript : EnemyBaseScript
{
    private CheckForPlayerRadiusScript _myRadiusScript;

    private bool _isPlayerWithinRange;

    [SerializeField] private Transform _checkIfCanAttackRadiusOrigin;
    [SerializeField] private LayerMask _layerPlayer;
    private float _checkIfCanAttackRadius;
    private float _checkIfCanAttackRadiusDivider = 10f;

    protected override void Awake() {
        base.Awake();
        _myRadiusScript = GetComponentInChildren<CheckForPlayerRadiusScript>();
    }

    protected override void Start() {
        _isOriginallyFacingRight = true;
        CanIWalk(true);

        base.Start();

        _isPlayerWithinRange = false;

        _checkIfCanAttackRadius = transform.localScale.x / _checkIfCanAttackRadiusDivider;

        _myRadiusScript.OnPlayerDetected += _myRadiusScript_OnPlayerDetected;
        _myRadiusScript.OnPlayerMissing += _myRadiusScript_OnPlayerMissing;

        _myVisual.GetComponent<GoblinVisualScript>().OnEnemyAttackAnimStarted += GoblinScript_OnEnemyAttackAnimStarted;
        _myVisual.GetComponent<GoblinVisualScript>().OnEnemyAttackAnimStopped += GoblinScript_OnEnemyAttackAnimStopped;
    }

    private void GoblinScript_OnEnemyAttackAnimStarted(object sender, System.EventArgs e) {
        SetTouchAttackTrigger(false);
        CanIWalk(false);
    }

    private void GoblinScript_OnEnemyAttackAnimStopped(object sender, System.EventArgs e) {
        SetTouchAttackTrigger(true);
        CanIWalk(true);
    }


    private void _myRadiusScript_OnPlayerMissing(object sender, System.EventArgs e) {
        _isPlayerWithinRange = false;
    }

    private void _myRadiusScript_OnPlayerDetected(object sender, System.EventArgs e) {
        _isPlayerWithinRange = true;
    }

    private void OnDrawGizmos() {
        Gizmos.DrawSphere(_checkIfCanAttackRadiusOrigin.position, _checkIfCanAttackRadius);
    }

    protected override void Walk() {

        CanIWalk(true);

        if (!_isPlayerWithinRange) {
            CanIWalk(false);
            _moveDir = Vector2.zero;
            _myVisual.SetIsWalkingBoolFalse();
            return;
        }

        CheckToFlipIfIsEdgeOrHasWall();

        if (_isFacingRight && _myRadiusScript.PlayersCurrentXPosition() < transform.position.x) {
            _myVisual.Flip();
            _isFacingRight = !_isFacingRight;
        } else if (!_isFacingRight && _myRadiusScript.PlayersCurrentXPosition() > transform.position.x) {
            _myVisual.Flip();
            _isFacingRight = !_isFacingRight;
        }

        if (Physics2D.OverlapCircle(_checkIfCanAttackRadiusOrigin.position, _checkIfCanAttackRadius, _layerPlayer)) {
            CanIWalk(false);
            _myVisual.SetIsWalkingBoolFalse();
            Attack();
        }

        if (!_canIWalk) {
            _moveDir = Vector2.zero;
            _myVisual.SetIsWalkingBoolFalse();
            return;
        }

        if (_isFacingRight) {
            _moveDir = new Vector2(1f, 0f);
        } else if (!_isFacingRight) {
            _moveDir = new Vector2(-1f, 0f);
        }

        transform.position = (Vector2)transform.position + _mySO.speed * Time.deltaTime * _moveDir;

        _myVisual.SetIsWalkingBoolTrue();
    }


    protected override void CheckToFlipIfIsEdgeOrHasWall() {
        if (!Physics2D.OverlapCircle(_edgeCheck.position, _checkRadius, _layerIsGrounded) || Physics2D.OverlapCircle(_wallCheck.position, _checkRadius, _layerIsGrounded)) {
            CanIWalk(false);
            _myVisual.SetIsWalkingBoolFalse();
        };
    }

    protected override void Attack() {
        if (!_isPlayerWithinRange) { return; }
        base.Attack();
    }

    public override float PlayersCurrentXLocation() {
        return _myRadiusScript.PlayersCurrentXPosition();
    }

    protected override void SetPlayerRadiusCheckTrigger(bool isActive) {
        _myRadiusScript.gameObject.SetActive(isActive);
    }

    public override bool IsEnemyWalking() {
        return _isEnemyWalking;
    }
}
