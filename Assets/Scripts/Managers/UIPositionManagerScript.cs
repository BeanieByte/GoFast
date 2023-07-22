using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPositionManagerScript : MonoBehaviour
{

    /*

    [SerializeField] private Transform _healthCounterUI;
    private float _healthCounterUILeftXPosition;
    private float _healthCounterUIRightXPosition;

    [SerializeField] private Transform _airJumpCounterUI;
    private Transform _airJumpCounterUILeftPosition;
    private Transform _airJumpCounterUIRightPosition;

    [SerializeField] private Transform _turboBarUI;
    private Transform _turboBarUILeftPosition;
    private Transform _turboBarUIRightPosition;

    [SerializeField] private Transform _coinCounterUI;
    private Transform _coinCounterUILeftPosition;
    private Transform _coinCounterUIRightPosition;

    [SerializeField] private Transform _enemiesKilledCounterUI;
    private Transform _enemiesKilledCounterUILeftPosition;
    private Transform _enemiesKilledCounterUIRightPosition;

    [SerializeField] private Transform _timerUI;
    private Transform _timerUILeftPosition;
    private Transform _timerUIRightPosition;

    [SerializeField] private Transform _recommendedTimeUI;
    private Transform _recommendedTimeUILeftPosition;
    private Transform _recommendedTimeUIRightPosition;

    [SerializeField] private Transform _minimap;
    private Transform _minimapLeftPosition;
    private Transform _minimapRightPosition;


    private void Start() {

        _healthCounterUILeftXPosition = _healthCounterUI.position.x;
        Debug.Log(_healthCounterUI.position.x);
        _healthCounterUIRightXPosition = _healthCounterUILeftXPosition * -1;
        Debug.Log(_healthCounterUIRightXPosition);

        //_healthCounterUILeftPosition = _healthCounterUI;
        //_healthCounterUIRightPosition = _healthCounterUILeftPosition;
        //_healthCounterUIRightPosition.position = new Vector3(_healthCounterUILeftPosition.position.x * -1, _healthCounterUILeftPosition.position.y, _healthCounterUILeftPosition.position.z);

        //_airJumpCounterUILeftPosition = _airJumpCounterUI;
        //_airJumpCounterUIRightPosition = _airJumpCounterUILeftPosition;
        //_airJumpCounterUIRightPosition.position = new Vector3(_airJumpCounterUILeftPosition.position.x * -1, _airJumpCounterUILeftPosition.position.y, _airJumpCounterUILeftPosition.position.z);

        //_turboBarUILeftPosition = _turboBarUI;
        //_turboBarUIRightPosition = _turboBarUILeftPosition;
        //_turboBarUIRightPosition.position = new Vector3(_turboBarUILeftPosition.position.x * -1, _turboBarUILeftPosition.position.y, _turboBarUILeftPosition.position.z);

        //_coinCounterUILeftPosition = _coinCounterUI;
        //_coinCounterUIRightPosition = _coinCounterUILeftPosition;
        //_coinCounterUIRightPosition.position = new Vector3(_coinCounterUILeftPosition.position.x * -1, _coinCounterUILeftPosition.position.y, _coinCounterUILeftPosition.position.z);

        //_enemiesKilledCounterUILeftPosition = _enemiesKilledCounterUI;
        //_enemiesKilledCounterUIRightPosition = _enemiesKilledCounterUILeftPosition;
        //_enemiesKilledCounterUIRightPosition.position = new Vector3(_enemiesKilledCounterUILeftPosition.position.x * -1, _enemiesKilledCounterUILeftPosition.position.y, _enemiesKilledCounterUILeftPosition.position.z);

        //_timerUILeftPosition = _timerUI;
        //_timerUIRightPosition = _timerUILeftPosition;
        //_timerUIRightPosition.position = new Vector3(_timerUILeftPosition.position.x * -1, _timerUILeftPosition.position.y, _timerUILeftPosition.position.z);

        //_recommendedTimeUILeftPosition = _recommendedTimeUI;
        //_recommendedTimeUIRightPosition = _recommendedTimeUILeftPosition;
        //_recommendedTimeUIRightPosition.position = new Vector3(_recommendedTimeUILeftPosition.position.x * -1, _recommendedTimeUILeftPosition.position.y, _recommendedTimeUILeftPosition.position.z);

        //_minimapLeftPosition = _minimap;
        //_minimapRightPosition = _minimapLeftPosition;
        //_minimapRightPosition.position = new Vector3(_minimapLeftPosition.position.x * -1, _minimapLeftPosition.position.y, _minimapLeftPosition.position.z);

    }

    private void FixedUpdate() {
        if (Input.GetKeyDown(KeyCode.O)) {
            SetUIPositionToTheLeft();
        }

        if (Input.GetKeyDown(KeyCode.P)) {
            SetUIPositionToTheRight();
        }
    }

    private void SetUIPositionToTheLeft() {
        _healthCounterUI.position = new Vector3(_healthCounterUILeftXPosition, _healthCounterUI.position.y, _healthCounterUI.position.z);

        //_healthCounterUI = _healthCounterUILeftPosition;
        //_airJumpCounterUI = _airJumpCounterUILeftPosition;
        //_turboBarUI = _turboBarUILeftPosition;
        //_coinCounterUI = _coinCounterUILeftPosition;
        //_enemiesKilledCounterUI = _enemiesKilledCounterUILeftPosition;
        //_timerUI = _timerUILeftPosition;
        //_recommendedTimeUI = _recommendedTimeUILeftPosition;
        //_minimap = _minimapLeftPosition;
    }

    private void SetUIPositionToTheRight() {
        _healthCounterUI.position = new Vector3(_healthCounterUIRightXPosition, _healthCounterUI.position.y, _healthCounterUI.position.z);

        //_healthCounterUI = _healthCounterUIRightPosition;
        //_airJumpCounterUI = _airJumpCounterUIRightPosition;
        //_turboBarUI = _turboBarUIRightPosition;
        //_coinCounterUI = _coinCounterUIRightPosition;
        //_enemiesKilledCounterUI = _enemiesKilledCounterUIRightPosition;
        //_timerUI = _timerUIRightPosition;
        //_recommendedTimeUI = _recommendedTimeUIRightPosition;
        //_minimap = _minimapRightPosition;
    }

    */
}
