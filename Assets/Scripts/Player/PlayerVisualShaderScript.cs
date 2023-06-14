using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class PlayerVisualShaderScript : MonoBehaviour {

    private Renderer _playerRenderer;
    private Material _playerMaterial;
    private SpriteMask _playerSpriteMask;

    private PlayerVisualScript _playerVisualScript;

    private bool _isPlayerFlashing;
    private bool _isPlayerShining;

    private float _flashingBaseTime = 0f;

    private bool _isPlayerBurning;
    private bool _isPlayerParalyzed;
    private bool _isPlayerFrozen;
    private bool _isPlayerPoisoned;
    private bool _isPlayerSlimed;

    private UnityEngine.Color _burningOrangeRGB = new (255, 74, 0, 255);

    private void Awake() {
        _playerRenderer = GetComponent<Renderer>();
        _playerMaterial = _playerRenderer.material;
        _playerSpriteMask = GetComponent<SpriteMask>();

        _playerVisualScript = GetComponent<PlayerVisualScript>();
    }

    private void Start() {
        _playerSpriteMask.enabled = false;

        _isPlayerBurning = false;
        _isPlayerParalyzed = false;
        _isPlayerFrozen = false;
        _isPlayerPoisoned = false;
        _isPlayerSlimed = false;

        _isPlayerFlashing = false;
        _isPlayerShining = false;

        _playerMaterial.SetFloat("_HsvShift", (int)Random.Range(0f, 360f));
        _playerMaterial.SetFloat("_HsvSaturation", Random.Range(0.5f, 1.5f));

        _playerVisualScript.OnPlayerHitAnimStarted += _playerVisualScript_OnPlayerHitAnimStarted;
        _playerVisualScript.OnPlayerHitAnimStopped += _playerVisualScript_OnPlayerHitAnimStopped;

        _playerVisualScript.OnPlayerInvincibleAnimStarted += _playerVisualScript_OnPlayerInvincibleAnimStarted;
        _playerVisualScript.OnPlayerInvincibleAnimStopped += _playerVisualScript_OnPlayerInvincibleAnimStopped;

        _playerVisualScript.OnPlayerBurnAnimStarted += _playerVisualScript_OnPlayerBurnAnimStarted;
        _playerVisualScript.OnPlayerBurnAnimStopped += _playerVisualScript_OnPlayerBurnAnimStopped;

        _playerVisualScript.OnPlayerParalyzedAnimStarted += _playerVisualScript_OnPlayerParalyzedAnimStarted;
        _playerVisualScript.OnPlayerParalyzedAnimStopped += _playerVisualScript_OnPlayerParalyzedAnimStopped;

        _playerVisualScript.OnPlayerFreezeAnimStarted += _playerVisualScript_OnPlayerFreezeAnimStarted;
        _playerVisualScript.OnPlayerFreezeAnimStopped += _playerVisualScript_OnPlayerFreezeAnimStopped;

        _playerVisualScript.OnPlayerPoisonedAnimStarted += _playerVisualScript_OnPlayerPoisonedAnimStarted;
        _playerVisualScript.OnPlayerPoisonedAnimStopped += _playerVisualScript_OnPlayerPoisonedAnimStopped;

        _playerVisualScript.OnPlayerSlimedAnimStarted += _playerVisualScript_OnPlayerSlimedAnimStarted;
        _playerVisualScript.OnPlayerSlimedAnimStopped += _playerVisualScript_OnPlayerSlimedAnimStopped;
    }

    private void _playerVisualScript_OnPlayerInvincibleAnimStarted(object sender, System.EventArgs e) {
        _playerMaterial.EnableKeyword("SHINE_ON");
        _isPlayerShining = true;
        EnableFlashingHitEffect();
    }

    private void _playerVisualScript_OnPlayerInvincibleAnimStopped(object sender, System.EventArgs e) {
        _playerMaterial.DisableKeyword("SHINE_ON");
        _isPlayerShining = false;
        DisableFlashingHitEffect();
    }

    private void _playerVisualScript_OnPlayerHitAnimStarted(object sender, System.EventArgs e) {
        EnableFlashingHitEffect();
    }

    private void _playerVisualScript_OnPlayerHitAnimStopped(object sender, System.EventArgs e) {
        DisableFlashingHitEffect();
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

    private void _playerVisualScript_OnPlayerBurnAnimStarted(object sender, System.EventArgs e) {
        _playerMaterial.EnableKeyword("OUTBASE_ON");
        _playerMaterial.SetColor("_OutlineColor", _burningOrangeRGB);
        _playerMaterial.SetFloat("_OutlineAlpha", 1);
        _playerMaterial.SetFloat("_OutlineGlow", 5);
        _playerMaterial.EnableKeyword("_Outline8Directions");
        _playerMaterial.EnableKeyword("_OutlineIsPixel");
        _playerMaterial.SetFloat("_OutlinePixelWidth", 2);
        _playerSpriteMask.enabled = true;
        _isPlayerBurning = true;
    }

    private void _playerVisualScript_OnPlayerBurnAnimStopped(object sender, System.EventArgs e) {
        _isPlayerBurning = false;
        _playerMaterial.DisableKeyword("_OutlineIsPixel");
        _playerMaterial.DisableKeyword("_Outline8Directions");
        _playerMaterial.DisableKeyword("OUTBASE_ON");
        _playerSpriteMask.enabled = false;
    }

    private void _playerVisualScript_OnPlayerParalyzedAnimStarted(object sender, System.EventArgs e) {
        throw new System.NotImplementedException();
    }

    private void _playerVisualScript_OnPlayerParalyzedAnimStopped(object sender, System.EventArgs e) {
        throw new System.NotImplementedException();
    }

    private void _playerVisualScript_OnPlayerFreezeAnimStarted(object sender, System.EventArgs e) {
        throw new System.NotImplementedException();
    }

    private void _playerVisualScript_OnPlayerFreezeAnimStopped(object sender, System.EventArgs e) {
        throw new System.NotImplementedException();
    }

    private void _playerVisualScript_OnPlayerPoisonedAnimStarted(object sender, System.EventArgs e) {
        throw new System.NotImplementedException();
    }

    private void _playerVisualScript_OnPlayerPoisonedAnimStopped(object sender, System.EventArgs e) {
        throw new System.NotImplementedException();
    }

    private void _playerVisualScript_OnPlayerSlimedAnimStarted(object sender, System.EventArgs e) {
        throw new System.NotImplementedException();
    }

    private void _playerVisualScript_OnPlayerSlimedAnimStopped(object sender, System.EventArgs e) {
        throw new System.NotImplementedException();
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

        if (_isPlayerBurning) {
            PlayerBurningShader();
        }
    }

    private void PlayerBurningShader() { 
        
    }
}
