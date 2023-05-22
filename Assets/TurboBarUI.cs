using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurboBarUI : MonoBehaviour
{
    [SerializeField] private PlayerScript _playerScript;
    [SerializeField] private Slider _turboBarUI;

    private void Start() {
        _turboBarUI.maxValue = _playerScript._maxTurboTime;
        _playerScript.OnTurboTimeChanged += _playerScript_OnTurboTimeChanged;
    }

    private void _playerScript_OnTurboTimeChanged(object sender, PlayerScript.OnTurboTimeChangedEventArgs e) {
        _turboBarUI.value = e.turboTime;
    }

    private void Update() {
        if (_turboBarUI.maxValue != _playerScript._maxTurboTime){
            _turboBarUI.maxValue = _playerScript._maxTurboTime;
        }
    }
}
