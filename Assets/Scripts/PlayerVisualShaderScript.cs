using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class PlayerVisualShaderScript : MonoBehaviour {
    private Renderer _playerRenderer;

    private Material _playerMaterial;

    private PlayerVisualScript _playerVisualScript;

    private bool _isPlayerHit;

    private float _hitAnimBaseTime = 0f;

    private void Awake() {
        _playerRenderer = GetComponent<Renderer>();
        _playerMaterial = _playerRenderer.material;
        _playerVisualScript = GetComponent<PlayerVisualScript>();
    }

    private void Start() {
        _isPlayerHit = false;

        _playerMaterial.SetFloat("_HsvShift", (int)Random.Range(0f, 360f));
        _playerMaterial.SetFloat("_HsvSaturation", Random.Range(0.5f, 1.5f));

        _playerVisualScript.OnPlayerHitAnimStarted += _playerVisualScript_OnPlayerHitAnimStarted;
        _playerVisualScript.OnPlayerHitAnimStopped += _playerVisualScript_OnPlayerHitAnimStopped;
    }

    private void FixedUpdate() {

        if (_isPlayerHit) {
            _hitAnimBaseTime += (Time.deltaTime * 1f);
            float currentValue = Mathf.PingPong(_hitAnimBaseTime, 0.2f);
            _playerMaterial.SetFloat("_HitEffectBlend", currentValue);
        }
    }

    private void _playerVisualScript_OnPlayerHitAnimStopped(object sender, System.EventArgs e) {
        _playerMaterial.DisableKeyword("HITEFFECT_ON");
        _isPlayerHit = false;
        _hitAnimBaseTime = 0f;
    }

    private void _playerVisualScript_OnPlayerHitAnimStarted(object sender, System.EventArgs e) {
        _playerMaterial.EnableKeyword("HITEFFECT_ON");
        _isPlayerHit = true;
    }
}
