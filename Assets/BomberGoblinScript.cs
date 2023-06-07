using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberGoblinScript : EnemyBaseScript
{
    private CheckForPlayerRadiusScript _myRadiusScript;

    private bool _isPlayerWithinRange;

    [SerializeField] private float _myPersonalAttackCooldownTime = 6f;

    protected override void Awake() {
        base.Awake();
        _myRadiusScript = GetComponentInChildren<CheckForPlayerRadiusScript>();
    }

    protected override void Start() {
        _currentHealth = _mySO.health;
        _canIWalk = false;
        _attackCooldownTime = _myPersonalAttackCooldownTime;

        if (!_isFacingRight) {
            _myVisual.Flip();
        }

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

    protected override void FixedUpdate() {
        base.FixedUpdate();

        if (_isFacingRight && _myRadiusScript.PlayersCurrentXPosition() < transform.position.x) {
            _myVisual.Flip();
            _isFacingRight = !_isFacingRight;
        } else if (!_isFacingRight && _myRadiusScript.PlayersCurrentXPosition() > transform.position.x) {
            _myVisual.Flip();
            _isFacingRight = !_isFacingRight;
        }
    }

    protected override void Walk() {
        
    }

    public override void Attack() {
        if (!_isPlayerWithinRange) { return; }
        base.Attack();
    }
}
