using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    public static TimerManager Instance;

    private double _timerStart = 0;
    private double _timeTaken;
    private string _timeText;

    public event EventHandler<OnTimerChangedEventArgs> OnTimerChanged;

    public class OnTimerChangedEventArgs : EventArgs
    {
        public string currentTimeText;
        public double timeTaken;
    }

    private string _activeTimerMinutes;
    private string _activeTimerSeconds;

    private string _recommendedTimeText;
    [SerializeField] private int _recommendedTimeSeconds;
    private bool _setRecommendedTime;

    public event EventHandler<OnRecommendedTimeChangedEventArgs> OnRecommendedTimeChanged;

    public class OnRecommendedTimeChangedEventArgs : EventArgs {
        public string recommendedTimeText;
        public double recommendedTime;
    }

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this);

        }
        Instance = this;
    }

    private void Start() {
        _timerStart = 0f;
        _timeTaken = _timerStart;

        _setRecommendedTime = false;
        
    }

    private void FixedUpdate() {
        if (!_setRecommendedTime)
        {
            FormatRecommendedTime();
        }

        if (!GameManager.Instance.IsGamePlaying()) return;
        
        _timeTaken += Time.deltaTime;

        FormatActiveTime(_timeTaken);
    }

    private void FormatActiveTime(double currentTime) {

        _activeTimerMinutes = "0" + ((int)currentTime / 60).ToString();
        
        if (((int)currentTime / 60) > 9.98) {
            _activeTimerMinutes = ((int)currentTime / 60).ToString();
        }

        _activeTimerSeconds = "0" + (currentTime % 60).ToString("f2");
        
        if ((currentTime % 60) > 9.98) {
            _activeTimerSeconds = (currentTime % 60).ToString("f2");
        }

        _timeText = _activeTimerMinutes + "," + _activeTimerSeconds;

        OnTimerChanged?.Invoke(this, new OnTimerChangedEventArgs {
            timeTaken = currentTime,
            currentTimeText = _timeText
        }) ;
    }

    private void FormatRecommendedTime() {
        string recommendedTimeMinutes = "0" + ((int)_recommendedTimeSeconds / 60).ToString();

        if (((int)_recommendedTimeSeconds / 60) > 10) {
            recommendedTimeMinutes = ((int)_recommendedTimeSeconds / 60).ToString();
        }

        string recommendedTimeSeconds = "0" + ((int)_recommendedTimeSeconds % 60).ToString("f2");

        if (((int)_recommendedTimeSeconds % 60) > 10) {
            recommendedTimeSeconds = ((int)_recommendedTimeSeconds % 60).ToString("f2");
        }

        _recommendedTimeText = recommendedTimeMinutes + "," + recommendedTimeSeconds;

        OnRecommendedTimeChanged?.Invoke(this, new OnRecommendedTimeChangedEventArgs
        {
            recommendedTime = _recommendedTimeSeconds,
            recommendedTimeText = _recommendedTimeText
        }) ;

        _setRecommendedTime = true;
    }
}
