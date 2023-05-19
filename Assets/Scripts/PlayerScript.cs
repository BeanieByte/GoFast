using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{

    private float _playerMoveSpeed = 8f;

    private float _maxJumpForce = 8f;
    private float _minJumpForce = 2f;
    private float _instantJumpForce = 16f;
    private float _currentJumpForce;
    private float _maxJumpTime = 0.5f;
    private float _jumpTime = 0f;
    private bool _isTryingToJump = false;
    private bool _isGrounded;
    [SerializeField] private Transform _playerFeetPos;
    [SerializeField] private LayerMask _layerIsGrounded;
    private float _checkFeetRadius = 0.3f;
    private int _maxAvailableAirJumps = 1;
    private int _currentAvailableAirJumps;

    private Vector2 _currentPlayerVelocity;


    enum JumpState { 
        Grounded,
        Jumping,
        AirJumping,
        Falling
    }

    private JumpState _jumpState;

    [SerializeField] private PlayerVisualScript _playerVisual;

    [SerializeField] private bool _playerFacingRight;

    private Rigidbody2D _playerRigidBody;

    private void Awake() {
        _playerRigidBody = GetComponent<Rigidbody2D>();

        _currentPlayerVelocity = _playerRigidBody.velocity;
    }

    private void Start() {
        GameInput.Instance.OnJumpStarted += Instance_OnJumpStarted;
        GameInput.Instance.OnJumpPerformed += Instance_OnJumpPressed;
        GameInput.Instance.OnJumpCanceled += Instance_OnJumpCanceled;

        GameInput.Instance.OnAttackPressed += Instance_OnAttackPressed;
        GameInput.Instance.OnTurboPressed += Instance_OnTurboPressed;
        
        if (!_playerFacingRight) {
            _playerVisual.Flip();
        }

        _jumpState = JumpState.Grounded;
    }

    private void Instance_OnJumpStarted(object sender, System.EventArgs e) {
        if (_jumpState == JumpState.Grounded) {
            HandleJumpFirstFrame();
            _jumpState = JumpState.Jumping;
        } else if (_jumpState == JumpState.Falling && _currentAvailableAirJumps > 0) {
            StopYVelocity();
            HandleJumpFirstFrame();
            _jumpState = JumpState.AirJumping;
            _currentAvailableAirJumps--;
        }
    }

    private void Instance_OnJumpPressed(object sender, System.EventArgs e) {
        if (!_isTryingToJump) {
            _isTryingToJump = true;
            _jumpTime = 0f;
        }
    }

    private void Instance_OnJumpCanceled(object sender, System.EventArgs e) {
        if (_isTryingToJump) {
            _isTryingToJump = false;
            _jumpTime = 0f;
        }
    }

    private void Instance_OnTurboPressed(object sender, System.EventArgs e) {
        throw new System.NotImplementedException();
    }

    private void Instance_OnAttackPressed(object sender, System.EventArgs e) {
        throw new System.NotImplementedException();
    }

    private void Update() {
        Vector2 inputVector = GameInput.Instance.GetMovementVectorNormalized();

        Vector2 moveDir = new Vector2(inputVector.x, inputVector.y);

        transform.position = (Vector2)transform.position + _playerMoveSpeed * Time.deltaTime * moveDir;

        _isGrounded = Physics2D.OverlapCircle(_playerFeetPos.position, _checkFeetRadius, _layerIsGrounded);

        if (!_isGrounded && !_isTryingToJump) {
            _jumpState = JumpState.Falling;
        }

        if (moveDir.x < 0 && _playerFacingRight) {
            _playerVisual.Flip();
            _playerFacingRight = !_playerFacingRight;
        } else if (moveDir.x > 0 && !_playerFacingRight){
            _playerVisual.Flip();
            _playerFacingRight = !_playerFacingRight;
        }
    }

    private void FixedUpdate() {

        switch (_jumpState) {
            case JumpState.Grounded:
                if (_currentAvailableAirJumps < _maxAvailableAirJumps) {
                    for (int i = 0; i < _maxAvailableAirJumps; i++) {
                        _currentAvailableAirJumps++;
                    }
                }
                break;
            case JumpState.Jumping:
                _jumpTime += Time.deltaTime;
                if (_jumpTime <= _maxJumpTime) {
                    HandleJumpPressingOverTime();
                } else _jumpState = JumpState.Falling;
                break;
            case JumpState.AirJumping:
                _jumpTime += Time.deltaTime;
                if (_jumpTime <= _maxJumpTime) {
                    HandleJumpPressingOverTime();
                } else
                    _jumpState = JumpState.Falling;
                break;
            case JumpState.Falling:
                if (_isGrounded) {
                    _jumpState = JumpState.Grounded;
                }
                break;
        }
    }

    private void HandleJumpFirstFrame() {
        //_playerRigidBody.AddForce(Vector2.up * _instantJumpForce, ForceMode2D.Impulse);
        _currentPlayerVelocity.y = _instantJumpForce;
        SetPlayerVelocity(_currentPlayerVelocity);
    }

    private void HandleJumpPressingOverTime() {
        float normalizedJumpTime = Mathf.Clamp01(_jumpTime / _maxJumpTime);
        _currentJumpForce = Mathf.Lerp(_minJumpForce, _maxJumpForce, normalizedJumpTime);

        _currentPlayerVelocity.y = _currentJumpForce;
        SetPlayerVelocity(_currentPlayerVelocity);
    }

    private void StopYVelocity() {
        _currentPlayerVelocity.y = 0f;
        SetPlayerVelocity(_currentPlayerVelocity);
    }

    private void SetPlayerVelocity(Vector2 newPlayerVelocity) {
        _playerRigidBody.velocity = newPlayerVelocity;
    }
}
