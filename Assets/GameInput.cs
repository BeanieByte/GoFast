using System;
using System.Collections;
using System.Collections.Generic;
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


    public event EventHandler OnAttackPressed;
    public event EventHandler OnTurboPressed;


    private void Awake() {
        // If there is an instance, and it's not me, delete myself.
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

        _gameInputActions.Player.Attack.performed += Attack_performed;
        _gameInputActions.Player.Turbo.performed += Turbo_performed;
    }

    private void Jump_started(InputAction.CallbackContext obj) {
        OnJumpStarted?.Invoke(this, EventArgs.Empty);
    }

    private void Jump_performed(InputAction.CallbackContext obj) {
        OnJumpPerformed?.Invoke(this, EventArgs.Empty);
    }

    private void Jump_canceled(InputAction.CallbackContext obj) {
        OnJumpCanceled?.Invoke(this, EventArgs.Empty);
    }

    private void Turbo_performed(InputAction.CallbackContext obj) {
        OnTurboPressed?.Invoke(this, EventArgs.Empty);
    }

    private void Attack_performed(InputAction.CallbackContext obj) {
        OnAttackPressed?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized() {
        Vector2 inputVector = _gameInputActions.Player.Move.ReadValue<Vector2>();

        inputVector = inputVector.normalized;

        return inputVector;
    }

}
