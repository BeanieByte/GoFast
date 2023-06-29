using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class GameInput : MonoBehaviour
{
    public static GameInput Instance { get; private set; }

    private GameInputActions _gameInputActions;

    public event EventHandler OnJumpStarted;
    public event EventHandler OnJumpPerformed;
    public event EventHandler OnJumpCanceled;

    public event EventHandler OnTurboPressed;
    public event EventHandler OnTurboCanceled;

    public event EventHandler OnAttackPressed;

    private KeyCode _leftKey01;
    private KeyCode _leftKey02;
    private KeyCode _rightKey01;
    private KeyCode _rightKey02;

    private bool _gameMoveInputIsTryingToGoRight;

    public event EventHandler OnMovePerformed;
    public event EventHandler OnMoveCancelled;

    private void Awake() {
        if (Instance != null && Instance != this) 
        {
            Destroy(this);

        } 
        Instance = this;


        _gameInputActions = new GameInputActions();
        _gameInputActions.Player.Enable();

        _gameInputActions.Player.Jump.started += Jump_started;
        _gameInputActions.Player.Jump.performed += Jump_performed;
        _gameInputActions.Player.Jump.canceled += Jump_canceled;

        _gameInputActions.Player.Turbo.performed += Turbo_performed;
        _gameInputActions.Player.Turbo.canceled += Turbo_canceled;

        _gameInputActions.Player.Attack.performed += Attack_performed;
    }

    private void Move_performed(InputAction.CallbackContext obj) {
        if (!GameManager.Instance.IsGamePlaying()) return;
        OnMovePerformed?.Invoke(this, EventArgs.Empty);
    }

    private void Move_canceled(InputAction.CallbackContext obj) {
        if (!GameManager.Instance.IsGamePlaying()) return;
        OnMoveCancelled?.Invoke(this, EventArgs.Empty);
    }

    private void Jump_started(InputAction.CallbackContext obj) {
        if (!GameManager.Instance.IsGamePlaying()) return;
        OnJumpStarted?.Invoke(this, EventArgs.Empty);
    }

    private void Jump_performed(InputAction.CallbackContext obj) {
        if (!GameManager.Instance.IsGamePlaying()) return;
        OnJumpPerformed?.Invoke(this, EventArgs.Empty);
    }

    private void Jump_canceled(InputAction.CallbackContext obj) {
        if (!GameManager.Instance.IsGamePlaying()) return;
        OnJumpCanceled?.Invoke(this, EventArgs.Empty);
    }

    private void Turbo_performed(InputAction.CallbackContext obj) {
        if (!GameManager.Instance.IsGamePlaying()) return;
        OnTurboPressed?.Invoke(this, EventArgs.Empty);
    }

    private void Turbo_canceled(InputAction.CallbackContext obj) {
        if (!GameManager.Instance.IsGamePlaying()) return;
        OnTurboCanceled?.Invoke(this, EventArgs.Empty);
    }

    private void Attack_performed(InputAction.CallbackContext obj) {
        if (!GameManager.Instance.IsGamePlaying()) return;
        OnAttackPressed?.Invoke(this, EventArgs.Empty);
    }

    private void Start() {
        UpdateKeyboardLeftAndRightKeys();
    }

    private void UpdateKeyboardLeftAndRightKeys() {
        string currentMoveKeysConfig = _gameInputActions.asset.actionMaps[0].FindAction("Move").ToString();
        currentMoveKeysConfig = currentMoveKeysConfig.TrimStart("Player/Move[");
        currentMoveKeysConfig = currentMoveKeysConfig.TrimEnd("]");
        currentMoveKeysConfig = currentMoveKeysConfig.Replace("/Keyboard/", "");
        string[] independentKeys = currentMoveKeysConfig.Split(',');

        _leftKey01 = (KeyCode)System.Enum.Parse(typeof(KeyCode), independentKeys[0].FirstCharacterToUpper());
        _leftKey02 = (KeyCode)System.Enum.Parse(typeof(KeyCode), independentKeys[1].FirstCharacterToUpper());
        _rightKey01 = (KeyCode)System.Enum.Parse(typeof(KeyCode), independentKeys[2].FirstCharacterToUpper());
        _rightKey02 = (KeyCode)System.Enum.Parse(typeof(KeyCode), independentKeys[3].FirstCharacterToUpper());
    }

    public Vector2 GetMovementVectorNormalized() {
        if (!GameManager.Instance.IsGamePlaying()) return new Vector2(0f, 0f);

        Vector2 inputVector = _gameInputActions.Player.Move.ReadValue<Vector2>();

        if (inputVector.x > 0) {
            _gameMoveInputIsTryingToGoRight = true;
        } else if (inputVector.x < 0) {
            _gameMoveInputIsTryingToGoRight = false;
        }

        if (inputVector == Vector2.zero) {
            if (_gameMoveInputIsTryingToGoRight && (Input.GetKey(_leftKey01) || Input.GetKey(_leftKey02))) {
                inputVector = new Vector2(-1f, 0f);
            } else if (!_gameMoveInputIsTryingToGoRight && (Input.GetKey(_rightKey01) || Input.GetKey(_rightKey02))) {
                inputVector = new Vector2(1f, 0f);
            }
        }

        Vector2 vectorToReturn = new Vector2(inputVector.x, 0f);

        vectorToReturn = vectorToReturn.normalized;

        return vectorToReturn;
    }
}
