using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVisualShaderBaseScript : MonoBehaviour
{
    private Renderer _myRenderer;
    private Material _myMaterial;

    private EnemyBaseScript _myLogicScript;
    private EnemyVisualBaseScript _myVisualScript;

    private bool _amIHit;
    private float _hitAnimBaseTime = 0f;

    private float _burningAnimationSpeed = 3f;
    private float _burningAnimationMinValue = 0.2f;
    private float _burningAnimationMaxValue = 1.8f;

    private float _poisonedAnimationSpeed = 1f;
    private float _poisonedAnimationMinValue = 0.5f;
    private float _poisonedAnimationMaxValue = 3f;

    private void Awake() {
        _myLogicScript = GetComponentInParent<EnemyBaseScript>();
        _myVisualScript = GetComponent<EnemyVisualBaseScript>();
        _myRenderer = GetComponent<Renderer>();
        _myMaterial = _myRenderer.material;
    }

    private void Start() {
        _amIHit = false;

        _myVisualScript.OnEnemyHitAnimStarted += _myVisualScript_OnEnemyHitAnimStarted;
        _myVisualScript.OnEnemyHitAnimStopped += _myVisualScript_OnEnemyHitAnimStopped;
    }

    private void _myVisualScript_OnEnemyHitAnimStopped(object sender, System.EventArgs e) {
        _myMaterial.SetFloat("_HitEffectBlend", 0f);
        _myMaterial.SetFloat("_HitEffectGlow", 1f);
        _amIHit = false;
        _hitAnimBaseTime = 0f;
    }

    private void _myVisualScript_OnEnemyHitAnimStarted(object sender, System.EventArgs e) {
        _myMaterial.SetFloat("_HitEffectBlend", 1f);
        _myMaterial.SetFloat("_HitEffectGlow", 1.5f);
        _amIHit = true;
    }

    private void FixedUpdate() {

        if (_amIHit) {
            _hitAnimBaseTime += (Time.deltaTime * 1f);
            float currentValue = Mathf.PingPong(_hitAnimBaseTime, 0.2f);
            _myMaterial.SetFloat("_HitEffectBlend", currentValue);
        }

        if (_myLogicScript.CanBurn()) {
            float t = Mathf.PingPong(Time.time * _burningAnimationSpeed, 1f);
            float interpolatedValue = Mathf.Lerp(_burningAnimationMinValue, _burningAnimationMaxValue, t);

            _myMaterial.SetFloat("_OverlayGlow", interpolatedValue);
        }

        if(_myLogicScript.CanPoison())
        {
            float t = Mathf.PingPong(Time.time * _poisonedAnimationSpeed, 1f);
            float interpolatedValue = Mathf.Lerp(_poisonedAnimationMinValue, _poisonedAnimationMaxValue, t);

            _myMaterial.SetFloat("_GradBoostY", interpolatedValue);
        }
    }
    protected virtual void OnDestroy() {

        _myVisualScript.OnEnemyHitAnimStarted -= _myVisualScript_OnEnemyHitAnimStarted;
        _myVisualScript.OnEnemyHitAnimStopped -= _myVisualScript_OnEnemyHitAnimStopped;
    }
}
