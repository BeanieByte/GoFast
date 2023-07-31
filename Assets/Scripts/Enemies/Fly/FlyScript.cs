using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyScript : EnemyBaseScript
{
    private CheckForPlayerRadiusScript _myRadiusScript;

    private float _maxWalkDistance = 5f;
    private float _startingXPosition;
    private float _maxLeftDistanceICanGo;
    private float _maxRightDistanceICanGo;

    private bool _isPlayerWithinRange;

    private Vector3 _positionBeforeAttacking;

    protected override void Awake() {
        base.Awake();
        _myRadiusScript = GetComponentInChildren<CheckForPlayerRadiusScript>();
    }

    protected override void Start() {
        _isOriginallyFacingRight = false;
        CanIWalk(true);
        base.Start();

        _startingXPosition = transform.position.x;
        _maxLeftDistanceICanGo = _startingXPosition - _maxWalkDistance;
        _maxRightDistanceICanGo = _startingXPosition + _maxWalkDistance;

        _myRadiusScript.OnPlayerDetected += _myRadiusScript_OnPlayerDetected;
        _myRadiusScript.OnPlayerMissing += _myRadiusScript_OnPlayerMissing;

        _myVisual.GetComponent<FlyVisualScript>().OnEnemyAttackAnimStarted += FlyScript_OnEnemyAttackAnimStarted;
        _myVisual.GetComponent<FlyVisualScript>().OnEnemyAttackAnimStopped += FlyScript_OnEnemyAttackAnimStopped;
    }

    private void FlyScript_OnEnemyAttackAnimStarted(object sender, System.EventArgs e) {
        SetTouchAttackTrigger(false);
        SetAttackTrigger(true);
        CanIWalk(false);
        SetPlayerRadiusCheckTrigger(false);
        _positionBeforeAttacking = transform.position;
        transform.position = new Vector3(PlayersCurrentXLocation(), _positionBeforeAttacking.y, _positionBeforeAttacking.z);
    }

    private void FlyScript_OnEnemyAttackAnimStopped(object sender, System.EventArgs e) {
        transform.position = _positionBeforeAttacking;
        SetTouchAttackTrigger(true);
        CanIWalk(true);
        SetAttackTrigger(false);
        SetPlayerRadiusCheckTrigger(true);
    }

    private void _myRadiusScript_OnPlayerMissing(object sender, System.EventArgs e) {
        _isPlayerWithinRange = false;
    }

    private void _myRadiusScript_OnPlayerDetected(object sender, System.EventArgs e) {
        _isPlayerWithinRange = true;
        Attack();
    }

    protected override void CheckToFlipIfIsEdgeOrHasWall() {
        if (Physics2D.OverlapCircle(_wallCheck.position, _checkRadius, _layerIsGrounded) || transform.position.x <= _maxLeftDistanceICanGo || transform.position.x >= _maxRightDistanceICanGo) {
            _isFacingRight = !_isFacingRight;
            _myVisual.Flip();
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

    protected override void OnDestroy() {
        base.OnDestroy();

        _myRadiusScript.OnPlayerDetected -= _myRadiusScript_OnPlayerDetected;
        _myRadiusScript.OnPlayerMissing -= _myRadiusScript_OnPlayerMissing;

        _myVisual.GetComponent<FlyVisualScript>().OnEnemyAttackAnimStarted -= FlyScript_OnEnemyAttackAnimStarted;
        _myVisual.GetComponent<FlyVisualScript>().OnEnemyAttackAnimStopped -= FlyScript_OnEnemyAttackAnimStopped;
    }
}
