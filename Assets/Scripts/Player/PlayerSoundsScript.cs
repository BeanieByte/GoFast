using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundsScript : MonoBehaviour
{
    private PlayerScript _playerLogicScript;

    private float _footstepsCurrentTime;
    private float _footstepsMaxTime = 0.1f;
    private bool _playerIsRunningAndGrounded;

    private void Awake()
    {
        _playerLogicScript = GetComponentInParent<PlayerScript>();
    }

    private void Start()
    {
        _footstepsCurrentTime = _footstepsMaxTime;
    }

    private void FixedUpdate()
    {
        if (_playerLogicScript.IsPlayerRunning() && _playerLogicScript.IsPlayerGrounded())
        {
            _playerIsRunningAndGrounded = true;
        }
        else {
            _playerIsRunningAndGrounded = false;
        }

        if (_playerIsRunningAndGrounded)
        {
            if (_footstepsCurrentTime == _footstepsMaxTime)
            {
                SoundManager.Instance.PlayPlayerFootstepsSound(_playerLogicScript.transform.position);
            }

            _footstepsCurrentTime -= Time.deltaTime;

            if (_footstepsCurrentTime <= 0) {
                _footstepsCurrentTime = _footstepsMaxTime;
            }
        }

        if (!_playerIsRunningAndGrounded && _footstepsCurrentTime != _footstepsMaxTime)
        {
            _footstepsCurrentTime = _footstepsMaxTime;
        }
        
    }
}
