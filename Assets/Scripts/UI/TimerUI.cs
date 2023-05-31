using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    private TextMeshProUGUI _myText;

    private void Awake() {
        _myText = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start() {
        TimerManager.Instance.OnTimerChanged += Instance_OnTimerChanged;
    }

    private void Instance_OnTimerChanged(object sender, TimerManager.OnTimerChangedEventArgs e) {
        _myText.text = e.currentTimeText.Replace("," , ":");
    }
}
