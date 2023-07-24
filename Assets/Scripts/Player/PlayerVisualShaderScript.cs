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

    private UnityEngine.Color _hitColorBeforeDamage;
    private float _hitGlowBeforeDamange;
    private float _hitBlendBeforeDamage;
    private float _flickerPercentBeforeDamage;
    private float _flickerFreqBeforeDamage;
    private float _flickerAlphaBeforeDamage;

    private UnityEngine.Color _hitColorAfterDamage;
    private float _hitGlowAfterDamange;
    private float _hitBlendAfterDamage;
    private float _flickerPercentAfterDamage;
    private float _flickerFreqAfterDamage;
    private float _flickerAlphaAfterDamage;

    private bool _isPlayerHit;
    private float _hitWhiteBlendAnimationMinValue = 0.2f;
    private float _hitWhiteBlendAnimationMaxValue = 0.7f;
    private float _hitWhiteBlendAnimationSpeed = 3f;


    private float _innerOutlineThicknessBeforeInvincible;
    private float _innerOutlineThicknessAfterInvincible;
    private float _colorRampBlendBeforeInvicinble;
    private float _colorRampBlendAfterInvicinble;
    private float _hueShiftBeforeInvincible;
    private float _hueShiftAfterInvincible;
    private float _hueShiftSaturationBeforeInvincible;
    private float _hueShiftSaturationAfterInvincible;

    private bool _isPlayerInvincible;
    private bool _isPlayerInvincibleAlmostOver;
    [SerializeField] private Texture _invincibilityTexture;
    [SerializeField] private Material _gradientScroll;
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

        _playerRenderer.material.SetColor("_InnerOutlineColor", UnityEngine.Color.black);
        _innerOutlineThicknessBeforeInvincible = _playerRenderer.material.GetFloat("_InnerOutlineThickness");
        _playerRenderer.material.SetFloat("_InnerOutlineThickness", 3f);
        _innerOutlineThicknessAfterInvincible = _playerRenderer.material.GetFloat("_InnerOutlineThickness");
        _playerRenderer.material.SetFloat("_InnerOutlineAlpha", 1f);
        _playerRenderer.material.SetFloat("_InnerOutlineGlow", 250f);

        _playerRenderer.material.SetTexture("_ColorRampTex", _invincibilityTexture);
        _colorRampBlendBeforeInvicinble = _playerRenderer.material.GetFloat("_ColorRampBlend");
        _playerRenderer.material.SetFloat("_ColorRampBlend", 1f);
        _colorRampBlendAfterInvicinble = _playerRenderer.material.GetFloat("_ColorRampBlend");

        _hueShiftBeforeInvincible = _playerRenderer.material.GetFloat("_HsvShift");
        _playerRenderer.material.SetFloat("_HsvShift", 0f);
        _hueShiftAfterInvincible = _playerRenderer.material.GetFloat("_HsvShift");
        _hueShiftSaturationBeforeInvincible = _playerRenderer.material.GetFloat("_HsvSaturation");
        _playerRenderer.material.SetFloat("_HsvSaturation", 1f);
        _hueShiftSaturationAfterInvincible = _playerRenderer.material.GetFloat("_HsvSaturation");
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

        //float currentHueShift = _playerRenderer.material.GetFloat("_HsvShift");
        //if (currentHueShift == _hueShiftAfterInvincible) {
        _playerRenderer.material.SetFloat("_HsvShift", _hueShiftBeforeInvincible);
        if (_isPlayerBurning) {
            _playerRenderer.material.SetFloat("_HsvShift", 100f);
        }
        //}
        //float currentHueShiftSaturation = _playerRenderer.material.GetFloat("_HsvSaturation");
        //if (currentHueShiftSaturation == _hueShiftSaturationAfterInvincible) {
            _playerRenderer.material.SetFloat("_HsvSaturation", _hueShiftSaturationBeforeInvincible);
        //}

        //float currentInnerOutlineThickness = _playerRenderer.material.GetFloat("_InnerOutlineThickness");
        //if (currentInnerOutlineThickness == _innerOutlineThicknessAfterInvincible) {
            _playerRenderer.material.SetFloat("_InnerOutlineThickness", _innerOutlineThicknessBeforeInvincible);
        //}

        //float currentColorRampBlend = _playerRenderer.material.GetFloat("_ColorRampBlend");
        //if (currentColorRampBlend == _colorRampBlendAfterInvicinble) {
            _playerRenderer.material.SetFloat("_ColorRampBlend", _colorRampBlendBeforeInvicinble);
        //}
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
        _hitColorBeforeDamage = _playerRenderer.material.GetColor("_HitEffectColor");
        _playerRenderer.material.SetColor("_HitEffectColor", UnityEngine.Color.white);
        _hitColorAfterDamage = _playerRenderer.material.GetColor("_HitEffectColor");

        _hitGlowBeforeDamange = _playerRenderer.material.GetFloat("_HitEffectGlow");
        _playerRenderer.material.SetFloat("_HitEffectGlow", 0.9f);
        _hitGlowAfterDamange = _playerRenderer.material.GetFloat("_HitEffectGlow");

        _hitBlendBeforeDamage = _playerRenderer.material.GetFloat("_HitEffectBlend");
        _playerRenderer.material.SetFloat("_HitEffectBlend", 1f);
        _hitBlendAfterDamage = _playerRenderer.material.GetFloat("_HitEffectBlend");

        _flickerPercentBeforeDamage = _playerRenderer.material.GetFloat("_FlickerPercent");
        _playerRenderer.material.SetFloat("_FlickerPercent", 0.5f);
        _flickerPercentAfterDamage = _playerRenderer.material.GetFloat("_FlickerPercent");

        _flickerFreqBeforeDamage = _playerRenderer.material.GetFloat("_FlickerFreq");
        _playerRenderer.material.SetFloat("_FlickerFreq", 1.2f);
        _flickerFreqAfterDamage = _playerRenderer.material.GetFloat("_FlickerFreq");

        _flickerAlphaBeforeDamage = _playerRenderer.material.GetFloat("_FlickerAlpha");
        _playerRenderer.material.SetFloat("_FlickerAlpha", 0f);
        _flickerAlphaAfterDamage = _playerRenderer.material.GetFloat("_FlickerAlpha");
    }

    private void DisableHitEffect() {
        //UnityEngine.Color currentHitColor = _playerRenderer.material.GetColor("_HitEffectColor");
        //if (currentHitColor == _hitColorAfterDamage) {
            //_playerRenderer.material.SetColor("_HitEffectColor", _hitColorBeforeDamage);
        //}

        //float currentHitGlow = _playerRenderer.material.GetFloat("_HitEffectGlow");
        //if (currentHitGlow == _hitGlowAfterDamange) {
            _playerRenderer.material.SetFloat("_HitEffectGlow", 0f);
        //}

        //float currentHitBlend = _playerRenderer.material.GetFloat("_HitEffectBlend");
        //if (currentHitBlend == _hitBlendAfterDamage) {
            _playerRenderer.material.SetFloat("_HitEffectBlend", 0f);
        //}

        //float currentFlickerPercent = _playerRenderer.material.GetFloat("_FlickerPercent");
        //if (currentFlickerPercent == _flickerPercentAfterDamage) {
            _playerRenderer.material.SetFloat("_FlickerPercent", 0f);
        //}

        //float currentFlickerFreq = _playerRenderer.material.GetFloat("_FlickerFreq");
        //if (currentFlickerFreq == _flickerFreqAfterDamage) {
            _playerRenderer.material.SetFloat("_FlickerFreq", 0f);
        //}

        //float currentFlickerAlpha = _playerRenderer.material.GetFloat("_FlickerAlpha");
        //if (currentFlickerAlpha == _flickerAlphaAfterDamage) {
            _playerRenderer.material.SetFloat("_FlickerAlpha", 0f);
        //}
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
