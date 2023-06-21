using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private CountdownTimerUI _countdownTimerUIScript;

    public enum State { 
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GameWon,
        GameOver,
        GamePaused,
    }

    private State _state;

    private float _waitingToStartTimer = 1f;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this);

        }
        Instance = this;

        _state = State.WaitingToStart;
    }

    private void Start() {
        _countdownTimerUIScript.OnGameStart += Instance_OnGameStart;
    }

    private void Instance_OnGameStart(object sender, EventArgs e) {
        _state = State.GamePlaying;
    }

    private void FixedUpdate() {
        
        //DEBUG ONLY, DELETE ON RELEASE
        if(Input.GetKeyDown(KeyCode.G))
        {
            _countdownTimerUIScript.gameObject.SetActive(false);
            _state = State.GamePlaying;
        }

        switch (_state) {
            case State.WaitingToStart:
                _waitingToStartTimer -= Time.deltaTime;
                if (_waitingToStartTimer <= 0f) {
                    _countdownTimerUIScript.gameObject.SetActive(true);
                    _state = State.CountdownToStart;
                }
                break;
            case State.CountdownToStart:
                break;
            case State.GamePlaying:
                break;
            case State.GameWon:
                break;
            case State.GameOver:
                break;
            case State.GamePaused:
                break;
        }
    }

    public bool IsGamePlaying() {
        return _state == State.GamePlaying;
    }

    public void SetGameOver() {
        _state = State.GameOver;
    }

    public void SetGameWon() {
        _state = State.GameWon;
    }
}
