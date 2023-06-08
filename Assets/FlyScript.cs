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

    protected override void Awake() {
        base.Awake();
        _isOriginallyFacingRight = false;
        _myRadiusScript = GetComponentInChildren<CheckForPlayerRadiusScript>();
        _startingXPosition = transform.position.x;
        _maxLeftDistanceICanGo = _startingXPosition - _maxWalkDistance;
        _maxRightDistanceICanGo = _startingXPosition + _maxWalkDistance;
    }

    protected override void Start() {
        base.Start();

        _myRadiusScript.OnPlayerDetected += _myRadiusScript_OnPlayerDetected;
        _myRadiusScript.OnPlayerMissing += _myRadiusScript_OnPlayerMissing;
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
        };
    }

    protected override void Attack() {
        if (!_isPlayerWithinRange) { return; }
        base.Attack();
    }

    public override float PlayersCurrentXLocation() {
        return _myRadiusScript.PlayersCurrentXPosition();
    }

    public override void SetPlayerRadiusCheckTrigger(bool isActive) {
        _myRadiusScript.gameObject.SetActive(isActive);
    }
}
