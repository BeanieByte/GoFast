using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class PlayerVisualShaderScript : MonoBehaviour {
    private Renderer _playerRenderer;

    private Material _playerMaterial;

    private PlayerVisualScript _playerVisualScript;

    private bool _isPlayerFlashing;
    private bool _isPlayerShining;

    private float _flashingBaseTime = 0f;

    private void Awake() {
        _playerRenderer = GetComponent<Renderer>();
        _playerMaterial = _playerRenderer.material;
        _playerVisualScript = GetComponent<PlayerVisualScript>();
    }

    private void Start() {
        _isPlayerFlashing = false;
        _isPlayerShining = false;

        _playerMaterial.SetFloat("_HsvShift", (int)Random.Range(0f, 360f));
        _playerMaterial.SetFloat("_HsvSaturation", Random.Range(0.5f, 1.5f));

        _playerVisualScript.OnPlayerHitAnimStarted += _playerVisualScript_OnPlayerHitAnimStarted;
        _playerVisualScript.OnPlayerHitAnimStopped += _playerVisualScript_OnPlayerHitAnimStopped;

        _playerVisualScript.OnPlayerInvincibleAnimStarted += _playerVisualScript_OnPlayerInvincibleAnimStarted;
        _playerVisualScript.OnPlayerInvincibleAnimStopped += _playerVisualScript_OnPlayerInvincibleAnimStopped;
    }

    private void _playerVisualScript_OnPlayerInvincibleAnimStopped(object sender, System.EventArgs e) {
        _playerMaterial.DisableKeyword("SHINE_ON");
        _isPlayerShining = false;
        DisableFlashingHitEffect();
    }

    private void _playerVisualScript_OnPlayerInvincibleAnimStarted(object sender, System.EventArgs e) {
        _playerMaterial.EnableKeyword("SHINE_ON");
        _isPlayerShining = true;
        EnableFlashingHitEffect();
    }

    private void FixedUpdate() {

        if (_isPlayerFlashing) {
            _flashingBaseTime += (Time.deltaTime * 1f);
            float currentValue = Mathf.PingPong(_flashingBaseTime, 0.1f);
            _playerMaterial.SetFloat("_HitEffectBlend", currentValue);

            if (!_isPlayerShining) { return; }

            float currentShineValue = Mathf.PingPong(_flashingBaseTime, 0.7f);
            _playerMaterial.SetFloat("_ShineLocation", currentShineValue);
        }
    }

    private void _playerVisualScript_OnPlayerHitAnimStopped(object sender, System.EventArgs e) {
        DisableFlashingHitEffect();
    }

    private void _playerVisualScript_OnPlayerHitAnimStarted(object sender, System.EventArgs e) {
        EnableFlashingHitEffect();
    }

    private void EnableFlashingHitEffect() {
        _playerMaterial.EnableKeyword("HITEFFECT_ON");
        _playerMaterial.EnableKeyword("FLICKER_ON");
        _isPlayerFlashing = true;
    }

    private void DisableFlashingHitEffect() {
        _playerMaterial.DisableKeyword("HITEFFECT_ON");
        _playerMaterial.DisableKeyword("FLICKER_ON");
        _isPlayerFlashing = false;
        _flashingBaseTime = 0f;
    }
}
