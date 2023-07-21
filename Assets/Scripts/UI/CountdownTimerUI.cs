using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountdownTimerUI : MonoBehaviour
{
    private TextMeshProUGUI _myText;
    private float _startingFontSize = 400f;
    private float _fontSizeDecreaseSpeed = 280f;
    private float _fontFadeDecreaseSpeed = 500f;
    private float _fontAlphaDefault = 255f;
    private float _fontAlpha;

    private float _countdownToStartTimer = 3f;

    public event EventHandler OnGameStart;

    private void Awake() {
        _myText = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start() {
        _countdownToStartTimer = 3f;
        _myText.fontSize = _startingFontSize;
        _fontAlpha = _fontAlphaDefault;
    }

    private void FixedUpdate() {

        _myText.faceColor = new Color32(_myText.faceColor.r, _myText.faceColor.g, _myText.faceColor.b, (byte)_fontAlpha);

        _countdownToStartTimer -= Time.deltaTime;
        _myText.fontSize -= (Time.deltaTime * _fontSizeDecreaseSpeed);

        if (_countdownToStartTimer <= 0) {

            if (_countdownToStartTimer >= -0.02f) {
                _myText.fontSize = _startingFontSize;
            }

            EndingCountdownTimer();
            return;
        }

        float currentValue = Mathf.Ceil(_countdownToStartTimer);

        _myText.text = currentValue.ToString();

        if (_countdownToStartTimer <= 1 && _countdownToStartTimer >= 0.98) {
            SoundManager.Instance.PlayCountdownToStartSound();
            _myText.fontSize = _startingFontSize;
            return;
        }

        if (_countdownToStartTimer <= 2 && _countdownToStartTimer >= 1.98) {
            SoundManager.Instance.PlayCountdownToStartSound();
            _myText.fontSize = _startingFontSize;
        }
    }

    private void EndingCountdownTimer() {
        _myText.text = "GO!";
        OnGameStart?.Invoke(this, EventArgs.Empty);
        _fontAlpha -= (Time.deltaTime * _fontFadeDecreaseSpeed);
        if (_fontAlpha > 0) {
            return;
        }
        gameObject.SetActive(false);
    }
}
