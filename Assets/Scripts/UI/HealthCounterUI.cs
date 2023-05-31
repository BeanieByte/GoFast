using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthCounterUI : MonoBehaviour
{
    [SerializeField] private PlayerScript _playerScript;
    private TextMeshProUGUI _myText;

    private void Awake() {
        _myText = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start() {
        _playerScript.OnHealthChanged += _playerScript_OnHealthChanged;
    }

    private void _playerScript_OnHealthChanged(object sender, PlayerScript.OnHealthChangedEventArgs e) {
        _myText.text = e.health.ToString();
    }
}
