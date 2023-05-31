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

    private bool _isTiming = false;

    public event EventHandler<OnTimerChangedEventArgs> OnTimerChanged;

    private string minutes;
    private string seconds;

    public class OnTimerChangedEventArgs : EventArgs {
        public string currentTimeText; 
    }

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this);

        }
        Instance = this;
    }

    private void Start() {
        _isTiming = false;
        _timerStart = 0f;
    }

    private void FixedUpdate() {
        //if (!_isTiming) {
        //    return;
        //}

        _timeTaken = _timerStart + Time.fixedTimeAsDouble;

        FormatTime(_timeTaken);
    }

    private void FormatTime(double currentTime) {

        minutes = "0" + ((int)currentTime / 60).ToString();
        
        if (((int)currentTime / 60) > 9.98) {
            minutes = ((int)currentTime / 60).ToString();
        }

        seconds = "0" + (currentTime % 60).ToString("f2");
        
        if ((currentTime % 60) > 9.98) {
            seconds = (currentTime % 60).ToString("f2");
        }

        _timeText = minutes + "," + seconds;

        OnTimerChanged?.Invoke(this, new OnTimerChangedEventArgs {
            currentTimeText = _timeText
        });
    }

    private void StartTimer() {
        _isTiming = true;
    }
}
