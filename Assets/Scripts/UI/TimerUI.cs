using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _activeTimerText;

    [SerializeField] private TextMeshProUGUI _recommendedTimeText;

    private void Awake() {
        _activeTimerText = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start() {
        TimerManager.Instance.OnTimerChanged += Instance_OnTimerChanged;

        TimerManager.Instance.OnRecommendedTimeChanged += Instance_OnRecommendedTimeChanged;
    }

    private void Instance_OnRecommendedTimeChanged(object sender, TimerManager.OnRecommendedTimeChangedEventArgs e)
    {
        string temp = e.recommendedTimeText.Replace(",", ":");
        //temp = e.recommendedTimeText.Replace(".", ":");
        _recommendedTimeText.text = temp;
    }

    private void Instance_OnTimerChanged(object sender, TimerManager.OnTimerChangedEventArgs e) {
        
        string temp = e.currentTimeText.Replace(",", ":");
        //temp = e.currentTimeText.Replace(".", ":");
        _activeTimerText.text = temp;
    }

    private void OnDestroy() {
        TimerManager.Instance.OnTimerChanged -= Instance_OnTimerChanged;

        TimerManager.Instance.OnRecommendedTimeChanged -= Instance_OnRecommendedTimeChanged;
    }
}
