using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AirJumpCounterUI : MonoBehaviour
{
    [SerializeField] private PlayerScript _playerScript;
    private TextMeshProUGUI _myText;

    private void Awake() {
        _myText = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start() {
        _playerScript.OnAirJumpCounterChanged += _playerScript_OnAirJumpCounterChanged;
    }

    private void _playerScript_OnAirJumpCounterChanged(object sender, PlayerScript.OnAirJumpCounterChangedEventArgs e) {
        _myText.text = e.airJumps.ToString();
    }

    private void OnDestroy() {
        _playerScript.OnAirJumpCounterChanged -= _playerScript_OnAirJumpCounterChanged;
    }
}
