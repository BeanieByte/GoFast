using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeScript : EnemyBaseScript 
{

    private float _minTimeBeforeWalking = 1f;
    private float _maxTimeBeforeWalking = 5f;
    private float _currentWalkCooldown;

    private float _minWalkingTime = 1.5f;
    private float _maxWalkingTime = 4f;
    private float _currentWalkTime;

    protected override void Start() {
        _isOriginallyFacingRight = false;
        _currentWalkCooldown = 0f;
        CanIWalk(true);
        base.Start();
    }

    protected override void Walk() {

        if (_currentWalkTime <= 0f) {
            _currentWalkCooldown -= Time.deltaTime;
        }

        if (_currentWalkCooldown <= 0f) {
            _currentWalkTime = Random.Range(_minWalkingTime, _maxWalkingTime);
            _myVisual.SetIsWalkingBoolTrue();
            CanIWalk(true);
            _currentWalkCooldown = Random.Range(_minTimeBeforeWalking, _maxTimeBeforeWalking);
        }

        if (!_canIWalk) {
            return;
        }

        _currentWalkTime -= Time.deltaTime;

        if (_currentWalkTime <= 0f) {
            _myVisual.SetIsWalkingBoolFalse();
            CanIWalk(false);
        }

        base.Walk();
    }
}
