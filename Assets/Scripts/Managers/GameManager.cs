using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private CountdownTimerUI _countdownTimerUIScript;

    public event EventHandler OnGameWon;
    public event EventHandler OnGameLost;

    private bool _hasGoTextSoundPlayed;

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

        _hasGoTextSoundPlayed = false;
    }

    private void Instance_OnGameStart(object sender, EventArgs e) {
        MusicManagerScript.Instance.PlayMusic();
        _state = State.GamePlaying;
    }

    private void FixedUpdate() {

        //DEBUG ONLY, DELETE ON RELEASE
        //if(Input.GetKeyDown(KeyCode.G))
        //{
        //    _countdownTimerUIScript.gameObject.SetActive(false);
        //    MusicManagerScript.Instance.PlayMusic();
        //    _state = State.GamePlaying;
        //}
        if(Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }

        switch (_state) {
            case State.WaitingToStart:
                _waitingToStartTimer -= Time.deltaTime;
                if (_waitingToStartTimer <= 0f) {
                    _countdownTimerUIScript.gameObject.SetActive(true);
                    SoundManager.Instance.PlayCountdownToStartSound();
                    _state = State.CountdownToStart;
                }
                break;
            case State.CountdownToStart:
                break;
            case State.GamePlaying:
                if (!_hasGoTextSoundPlayed) {
                    SoundManager.Instance.PlayGoTextBeforeStartSound();
                    _hasGoTextSoundPlayed = true;
                }
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
        SoundManager.Instance.PlayGameLostSound();
        MusicManagerScript.Instance.PauseMusic();
        OnGameLost?.Invoke(this, EventArgs.Empty);
    }

    public void SetGameWon() {
        _state = State.GameWon;
        SoundManager.Instance.PlayGameWonSound();
        MusicManagerScript.Instance.PauseMusic();
        OnGameWon?.Invoke(this, EventArgs.Empty);
    }

    private void OnDestroy() {
        _countdownTimerUIScript.OnGameStart -= Instance_OnGameStart;
    }
}
