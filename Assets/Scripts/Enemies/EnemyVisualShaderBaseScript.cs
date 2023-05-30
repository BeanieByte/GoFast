using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVisualShaderBaseScript : MonoBehaviour
{
    private Renderer _myRenderer;

    private Material _myMaterial;

    private EnemyVisualBaseScript _myVisualScript;

    private bool _amIHit;

    private float _hitAnimBaseTime = 0f;

    private void Awake() {
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
        _myMaterial.DisableKeyword("HITEFFECT_ON");
        _amIHit = false;
        _hitAnimBaseTime = 0f;
    }

    private void _myVisualScript_OnEnemyHitAnimStarted(object sender, System.EventArgs e) {
        _myMaterial.EnableKeyword("HITEFFECT_ON");
        _amIHit = true;
    }

    private void FixedUpdate() {

        if (_amIHit) {
            _hitAnimBaseTime += (Time.deltaTime * 1f);
            float currentValue = Mathf.PingPong(_hitAnimBaseTime, 0.2f);
            _myMaterial.SetFloat("_HitEffectBlend", currentValue);
        }
    }
}
