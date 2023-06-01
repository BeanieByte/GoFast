using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

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
    private float _countdownToStartTimer = 3f;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this);

        }
        Instance = this;

        _state = State.WaitingToStart;
    }

    private void FixedUpdate() {
        switch (_state) {
            case State.WaitingToStart:
                _waitingToStartTimer -= Time.deltaTime;
                if (_waitingToStartTimer <= 0f) {
                    _state = State.CountdownToStart;
                }
                break;
            case State.CountdownToStart:
                
                _countdownToStartTimer -= Time.deltaTime;
                if (_countdownToStartTimer <= 0f) {
                    _state = State.GamePlaying;
                }
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
