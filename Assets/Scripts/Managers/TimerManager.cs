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
        _timerStart = 0f;
        _timeTaken = _timerStart;
    }

    private void FixedUpdate() {
        if (!GameManager.Instance.IsGamePlaying()) return;
        
        _timeTaken += Time.deltaTime;

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
}
