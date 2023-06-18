using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class PlayerVisualShaderScript : MonoBehaviour {


    private Renderer _playerRenderer;
    private Material _playerMaterial;
    private PlayerVisualScript _playerVisualScript;


    private bool _isPlayerHit;
    private float _hitWhiteBlendAnimationMinValue = 0.2f;
    private float _hitWhiteBlendAnimationMaxValue = 0.7f;
    private float _hitWhiteBlendAnimationSpeed = 3f;


    private bool _isPlayerInvincible;
    [SerializeField] private Texture _invincibilityTexture;
    private float _invincibleAnimationMinValue = -0.52f;
    private float _invincibleAnimationMaxValue = 0.52f;
    private float _invincibleAnimationSpeed = 3f;


    private bool _isPlayerBurning;
    [SerializeField] private Material _playerBurningMaterial;
    private float _burningAnimationSpeed = 3f;
    private float _burningAnimationMinValue = 0.3f;
    private float _burningAnimationMaxValue = 1.5f;

    private bool _isPlayerParalyzed;

    private bool _isPlayerFrozen;
    [SerializeField] private Material _playerFrozenMaterial;
    [SerializeField] private GameObject _freezeAnimationParticleSystemGameObject;
    private float _freezeAnimationSpeed = 3f;
    private float _freezeAnimationCurrentValue;
    private float _freezeAnimationMinValue = 0f;
    private float _freezeAnimationMaxValue = 0.75f;

    private bool _isPlayerPoisoned;
    private bool _isPlayerSlimed;


    private void Awake() {
        _playerRenderer = GetComponent<Renderer>();
        _playerVisualScript = GetComponent<PlayerVisualScript>();
    }

    private void Start() {

        _isPlayerInvincible = false;

        _isPlayerBurning = false;
        _isPlayerParalyzed = false;
        _isPlayerFrozen = false;
        _isPlayerPoisoned = false;
        _isPlayerSlimed = false;

        _playerRenderer.material.SetFloat("_HsvShift", (int)Random.Range(0f, 360f));
        _playerRenderer.material.SetFloat("_HsvSaturation", Random.Range(0.5f, 1.5f));

        _playerMaterial = _playerRenderer.material;

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

    private void _playerVisualScript_OnPlayerHitAnimStarted(object sender, System.EventArgs e) {
        _playerRenderer.material.EnableKeyword("HITEFFECT_ON");
        _playerRenderer.material.SetColor("_HitEffectColor", UnityEngine.Color.white);
        _playerRenderer.material.SetFloat("_HitEffectGlow", 0.9f);
        _playerRenderer.material.EnableKeyword("FLICKER_ON");
        _playerRenderer.material.SetFloat("_FlickerPercent", 0.5f);
        _playerRenderer.material.SetFloat("_FlickerFreq", 1.2f);
        _playerRenderer.material.SetFloat("_FlickerAlpha", 0f);
        _isPlayerHit = true;
    }

    private void _playerVisualScript_OnPlayerHitAnimStopped(object sender, System.EventArgs e) {
        _isPlayerHit = false;
        _playerRenderer.material.DisableKeyword("HITEFFECT_ON");
        _playerRenderer.material.DisableKeyword("FLICKER_ON");
    }

    private void _playerVisualScript_OnPlayerInvincibleAnimStarted(object sender, System.EventArgs e) {
        _playerRenderer.material = _playerMaterial;
        _playerMaterial.EnableKeyword("INNEROUTLINE_ON");
        _playerMaterial.SetColor("_InnerOutlineColor", UnityEngine.Color.black);
        _playerMaterial.SetFloat("_InnerOutlineThickness", 3f);
        _playerMaterial.SetFloat("_InnerOutlineAlpha", 1f);
        _playerMaterial.SetFloat("_InnerOutlineGlow", 250f);
        _playerMaterial.EnableKeyword("COLORRAMP_ON");
        _playerMaterial.SetTexture("_ColorRampTex", _invincibilityTexture);
        _playerMaterial.SetFloat("_ColorRampBlend", 1f);
        _playerMaterial.DisableKeyword("HSV_ON");
        _isPlayerInvincible = true;
    }

    private void _playerVisualScript_OnPlayerInvincibleAnimStopped(object sender, System.EventArgs e) {
        _isPlayerInvincible = false;
        _playerMaterial.EnableKeyword("HSV_ON");
        _playerMaterial.DisableKeyword("INNEROUTLINE_ON");
        _playerMaterial.DisableKeyword("COLORRAMP_ON");
    }

    private void _playerVisualScript_OnPlayerBurnAnimStarted(object sender, System.EventArgs e) {
        _playerRenderer.material = _playerBurningMaterial;
        _isPlayerBurning = true;
    }

    private void _playerVisualScript_OnPlayerBurnAnimStopped(object sender, System.EventArgs e) {
        _playerRenderer.material = _playerMaterial;
        _isPlayerBurning = false;
    }

    private void _playerVisualScript_OnPlayerParalyzedAnimStarted(object sender, System.EventArgs e) {
        throw new System.NotImplementedException();
    }

    private void _playerVisualScript_OnPlayerParalyzedAnimStopped(object sender, System.EventArgs e) {
        throw new System.NotImplementedException();
    }

    private void _playerVisualScript_OnPlayerFreezeAnimStarted(object sender, System.EventArgs e) {
        _playerRenderer.material = _playerFrozenMaterial;
        _freezeAnimationParticleSystemGameObject.SetActive(true);
        _freezeAnimationCurrentValue = _freezeAnimationMinValue;
        PlayerFrozenShaderAnimationStarted();
        _isPlayerFrozen = true;
    }

    private void _playerVisualScript_OnPlayerFreezeAnimStopped(object sender, System.EventArgs e) {
        _isPlayerFrozen = false;
        _freezeAnimationCurrentValue = _freezeAnimationMaxValue;
        PlayerFrozenShaderAnimationEnded();
        _freezeAnimationParticleSystemGameObject.SetActive(false);
        _playerRenderer.material = _playerMaterial;
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

        if (_isPlayerHit) {
            HitShaderAnimation();
        }

        if (_isPlayerInvincible) {
            InvincibleShaderAnimation();
        }

        if (_isPlayerBurning) {
            PlayerBurningShaderAnimation();
        }

        //
    }

    private void HitShaderAnimation() {
        float t = Mathf.PingPong(Time.time * _hitWhiteBlendAnimationSpeed, 1f);
        float interpolatedValue = Mathf.Lerp(_hitWhiteBlendAnimationMinValue, _hitWhiteBlendAnimationMaxValue, t);

        _playerRenderer.material.SetFloat("_HitEffectBlend", interpolatedValue);
    }

    private void InvincibleShaderAnimation() {
        float t = Mathf.PingPong(Time.time * _invincibleAnimationSpeed, 1f);
        float interpolatedValue = Mathf.Lerp(_invincibleAnimationMinValue, _invincibleAnimationMaxValue, t);

        _playerMaterial.SetFloat("_ColorRampLuminosity", interpolatedValue);
    }

    private void PlayerBurningShaderAnimation() {
        float t = Mathf.PingPong(Time.time * _burningAnimationSpeed, 1f);
        float interpolatedValue = Mathf.Lerp(_burningAnimationMinValue, _burningAnimationMaxValue, t);

        _playerBurningMaterial.SetFloat("_OverlayGlow", interpolatedValue);
    }

    private void PlayerParalyzedShaderAnimation() { 
        
    }

    private void PlayerFrozenShaderAnimationStarted() {
        
        if (_freezeAnimationCurrentValue < _freezeAnimationMaxValue) {
            _freezeAnimationCurrentValue += (Time.deltaTime * _freezeAnimationSpeed);
        }

        if (_freezeAnimationCurrentValue >= _freezeAnimationMaxValue) {
            _freezeAnimationCurrentValue = _freezeAnimationMaxValue;
        }

        _playerFrozenMaterial.SetFloat("_OverlayGlow", _freezeAnimationCurrentValue);
    }

    private void PlayerFrozenShaderAnimationEnded() {
        _freezeAnimationCurrentValue -= (Time.deltaTime * _freezeAnimationSpeed * 2);

        _playerFrozenMaterial.SetFloat("_OverlayGlow", _freezeAnimationCurrentValue);

        if (_freezeAnimationCurrentValue > _freezeAnimationMinValue){
            return;
        }
    }
}
