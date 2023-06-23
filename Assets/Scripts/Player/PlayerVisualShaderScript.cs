using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Purchasing;

public class PlayerVisualShaderScript : MonoBehaviour {

    #region Variables

    private Renderer _playerRenderer;
    private Material _playerDefaultMaterial;
    private PlayerVisualScript _playerVisualScript;


    private bool _isPlayerHit;
    private float _hitWhiteBlendAnimationMinValue = 0.2f;
    private float _hitWhiteBlendAnimationMaxValue = 0.7f;
    private float _hitWhiteBlendAnimationSpeed = 3f;


    private bool _isPlayerInvincible;
    private bool _isPlayerInvincibleAlmostOver;
    [SerializeField] private Texture _invincibilityTexture;
    private float _invincibleAnimationMinValue = -0.52f;
    private float _invincibleAnimationMaxValue = 0.52f;
    private float _invincibleAnimationSpeed = 3f;


    private bool _isPlayerBurning;
    [SerializeField] private Material _playerBurningMaterial;
    [SerializeField] private GameObject _burningAnimationParticleSystemGameObject;
    private float _burningAnimationSpeed = 3f;
    private float _burningAnimationMinValue = 0.3f;
    private float _burningAnimationMaxValue = 1.5f;

    [SerializeField] private Material _playerParalyzedMaterial;
    [SerializeField] private GameObject _paralyzedAnimationParticleSystemGameObject;

    [SerializeField] private Material _playerFrozenMaterial;
    [SerializeField] private GameObject _frozenAnimationParticleSystemGameObject;
    private float _freezeAnimationSpeed = 2f;
    private float _freezeAnimationCurrentValue;
    private float _freezeAnimationMinValue = 0f;
    private float _freezeAnimationMaxValue = 0.75f;

    private bool _isPlayerPoisoned;
    [SerializeField] private Material _playerPoisonedMaterial;
    [SerializeField] private GameObject _poisonedAnimationParticleSystemGameObject;
    private float _poisonedAnimationSpeed = 1f;
    private float _poisonedAnimationMinValue = 0.8f;
    private float _poisonedAnimationMaxValue = 2.5f;

    [SerializeField] private Material _playerSlimedMaterial;
    [SerializeField] private GameObject _slimedAnimationParticleSystemGameObject;

    #endregion

    private void Awake() {
        _playerRenderer = GetComponent<Renderer>();
        _playerVisualScript = GetComponent<PlayerVisualScript>();
    }

    private void Start() {

        _isPlayerInvincible = false;
        _isPlayerInvincibleAlmostOver = false;

        _isPlayerBurning = false;
        _isPlayerPoisoned = false;

        _playerRenderer.material.SetFloat("_HsvShift", (int)Random.Range(0f, 360f));
        _playerRenderer.material.SetFloat("_HsvSaturation", Random.Range(0.5f, 1.5f));

        _playerDefaultMaterial = _playerRenderer.material;

        _playerVisualScript.OnPlayerHitAnimStarted += _playerVisualScript_OnPlayerHitAnimStarted;
        _playerVisualScript.OnPlayerHitAnimStopped += _playerVisualScript_OnPlayerHitAnimStopped;

        _playerVisualScript.OnPlayerInvincibleAnimStarted += _playerVisualScript_OnPlayerInvincibleAnimStarted;
        _playerVisualScript.OnPlayerInvincibleAnimAlmostOver += _playerVisualScript_OnPlayerInvincibleAnimAlmostOver;
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

    #region EventReceiverFunctions

    private void _playerVisualScript_OnPlayerHitAnimStarted(object sender, System.EventArgs e) {
        EnableHitEffect();
        _isPlayerHit = true;
    }

    private void _playerVisualScript_OnPlayerHitAnimStopped(object sender, System.EventArgs e) {
        _isPlayerHit = false;
        DisableHitEffect();
    }

    private void _playerVisualScript_OnPlayerInvincibleAnimStarted(object sender, System.EventArgs e) {
        _isPlayerInvincible = true;
        OnMaterialChange(_playerDefaultMaterial);
        _playerDefaultMaterial.EnableKeyword("INNEROUTLINE_ON");
        _playerDefaultMaterial.SetColor("_InnerOutlineColor", UnityEngine.Color.black);
        _playerDefaultMaterial.SetFloat("_InnerOutlineThickness", 3f);
        _playerDefaultMaterial.SetFloat("_InnerOutlineAlpha", 1f);
        _playerDefaultMaterial.SetFloat("_InnerOutlineGlow", 250f);
        _playerDefaultMaterial.EnableKeyword("COLORRAMP_ON");
        _playerDefaultMaterial.SetTexture("_ColorRampTex", _invincibilityTexture);
        _playerDefaultMaterial.SetFloat("_ColorRampBlend", 1f);
        _playerDefaultMaterial.DisableKeyword("HSV_ON");
    }

    private void _playerVisualScript_OnPlayerInvincibleAnimAlmostOver(object sender, System.EventArgs e)
    {
        EnableHitEffect();
        _isPlayerInvincibleAlmostOver = true;
    }

    private void _playerVisualScript_OnPlayerInvincibleAnimStopped(object sender, System.EventArgs e) {
        DisableHitEffect();
        _isPlayerInvincible = false;
        _isPlayerInvincibleAlmostOver = false;
        _playerDefaultMaterial.EnableKeyword("HSV_ON");
        _playerDefaultMaterial.DisableKeyword("INNEROUTLINE_ON");
        _playerDefaultMaterial.DisableKeyword("COLORRAMP_ON");
    }

    private void _playerVisualScript_OnPlayerBurnAnimStarted(object sender, System.EventArgs e) {
        OnMaterialChange(_playerBurningMaterial);
        _burningAnimationParticleSystemGameObject.SetActive(true);
        _isPlayerBurning = true;
    }

    private void _playerVisualScript_OnPlayerBurnAnimStopped(object sender, System.EventArgs e) {
        _burningAnimationParticleSystemGameObject.SetActive(false);
        OnMaterialChange(_playerDefaultMaterial);
        _isPlayerBurning = false;
    }

    private void _playerVisualScript_OnPlayerParalyzedAnimStarted(object sender, System.EventArgs e) {
        OnMaterialChange(_playerParalyzedMaterial);
        _paralyzedAnimationParticleSystemGameObject.SetActive(true);
    }

    private void _playerVisualScript_OnPlayerParalyzedAnimStopped(object sender, System.EventArgs e) {
        _paralyzedAnimationParticleSystemGameObject.SetActive(false);
        OnMaterialChange(_playerDefaultMaterial);

    }

    private void _playerVisualScript_OnPlayerFreezeAnimStarted(object sender, System.EventArgs e) {
        OnMaterialChange(_playerFrozenMaterial);
        _frozenAnimationParticleSystemGameObject.SetActive(true);
        _freezeAnimationCurrentValue = _freezeAnimationMinValue;
        PlayerFrozenShaderAnimationStarted();
    }

    private void _playerVisualScript_OnPlayerFreezeAnimStopped(object sender, System.EventArgs e) {
        _freezeAnimationCurrentValue = _freezeAnimationMaxValue;
        PlayerFrozenShaderAnimationEnded();
        _frozenAnimationParticleSystemGameObject.SetActive(false);
        OnMaterialChange(_playerDefaultMaterial);
    }

    private void _playerVisualScript_OnPlayerPoisonedAnimStarted(object sender, System.EventArgs e) {
        OnMaterialChange(_playerPoisonedMaterial);
        _poisonedAnimationParticleSystemGameObject.SetActive(true);
        _isPlayerPoisoned = true;
    }

    private void _playerVisualScript_OnPlayerPoisonedAnimStopped(object sender, System.EventArgs e) {
        OnMaterialChange(_playerDefaultMaterial);
        _poisonedAnimationParticleSystemGameObject.SetActive(false);
        _isPlayerPoisoned = false;

    }

    private void _playerVisualScript_OnPlayerSlimedAnimStarted(object sender, System.EventArgs e) {
        OnMaterialChange(_playerSlimedMaterial);
        _slimedAnimationParticleSystemGameObject.SetActive(true);
    }

    private void _playerVisualScript_OnPlayerSlimedAnimStopped(object sender, System.EventArgs e) {
        _slimedAnimationParticleSystemGameObject.SetActive(false);
        OnMaterialChange(_playerDefaultMaterial);
    }

    #endregion

    private void FixedUpdate() {

        if (_isPlayerHit) {
            HitShaderAnimation();
        }

        if (_isPlayerInvincible) {
            if (_isPlayerInvincibleAlmostOver) {
                HitShaderAnimation();
            } 
            InvincibleShaderAnimation();
        }

        if (_isPlayerBurning) {
            PlayerBurningShaderAnimation();
        }

        if (_isPlayerPoisoned)
        {
            PlayerPoisonedShaderAnimation();
        }
    }

    private void OnMaterialChange(Material newMaterial)
    {
        if (_isPlayerHit) { 
            DisableHitEffect();
        }

        _playerRenderer.material = newMaterial;

        if (_isPlayerHit && !_isPlayerInvincible)
        {
            EnableHitEffect();
        }
    }

    private void EnableHitEffect() {
        _playerRenderer.material.EnableKeyword("HITEFFECT_ON");
        _playerRenderer.material.SetColor("_HitEffectColor", UnityEngine.Color.white);
        _playerRenderer.material.SetFloat("_HitEffectGlow", 0.9f);
        _playerRenderer.material.EnableKeyword("FLICKER_ON");
        _playerRenderer.material.SetFloat("_FlickerPercent", 0.5f);
        _playerRenderer.material.SetFloat("_FlickerFreq", 1.2f);
        _playerRenderer.material.SetFloat("_FlickerAlpha", 0f);
    }

    private void DisableHitEffect() {
        _playerRenderer.material.DisableKeyword("HITEFFECT_ON");
        _playerRenderer.material.DisableKeyword("FLICKER_ON");
    }

    #region ShaderAnimations

    private void HitShaderAnimation() {
        float t = Mathf.PingPong(Time.time * _hitWhiteBlendAnimationSpeed, 1f);
        float interpolatedValue = Mathf.Lerp(_hitWhiteBlendAnimationMinValue, _hitWhiteBlendAnimationMaxValue, t);

        _playerRenderer.material.SetFloat("_HitEffectBlend", interpolatedValue);
    }

    private void InvincibleShaderAnimation() {
        float t = Mathf.PingPong(Time.time * _invincibleAnimationSpeed, 1f);
        float interpolatedValue = Mathf.Lerp(_invincibleAnimationMinValue, _invincibleAnimationMaxValue, t);

        _playerDefaultMaterial.SetFloat("_ColorRampLuminosity", interpolatedValue);
    }

    private void PlayerBurningShaderAnimation() {
        float t = Mathf.PingPong(Time.time * _burningAnimationSpeed, 1f);
        float interpolatedValue = Mathf.Lerp(_burningAnimationMinValue, _burningAnimationMaxValue, t);

        _playerBurningMaterial.SetFloat("_OverlayGlow", interpolatedValue);
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

    private void PlayerPoisonedShaderAnimation() {
        float t = Mathf.PingPong(Time.time * _poisonedAnimationSpeed, 1f);
        float interpolatedValue = Mathf.Lerp(_poisonedAnimationMinValue, _poisonedAnimationMaxValue, t);

        _playerPoisonedMaterial.SetFloat("_GradBoostY", interpolatedValue);
    }

    #endregion
}
