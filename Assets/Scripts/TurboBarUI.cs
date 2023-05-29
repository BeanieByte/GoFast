using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurboBarUI : MonoBehaviour
{
    [SerializeField] private PlayerScript _playerScript;
    [SerializeField] private Slider _turboBarUI;

    private void Start() {
        _playerScript.OnTurboTimeChanged += _playerScript_OnTurboTimeChanged;
        _turboBarUI.maxValue = _playerScript.MaxTurboTime();
    }

    private void _playerScript_OnTurboTimeChanged(object sender, PlayerScript.OnTurboTimeChangedEventArgs e) {
        _turboBarUI.value = e.turboTime;
    }

    private void Update() {
        if (_turboBarUI.maxValue != _playerScript.MaxTurboTime()) {
            _turboBarUI.maxValue = _playerScript.MaxTurboTime();
        }
    }
}
