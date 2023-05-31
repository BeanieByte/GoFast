using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpScript : MonoBehaviour
{
    [SerializeField] private PowerUpSO _mySO;
    private Collider2D _myTrigger;
    private PowerUpVisualScript _myVisual;
    private SpriteRenderer _myRenderer;

    private void Awake() {
        _myTrigger = GetComponent<Collider2D>();
        _myVisual = GetComponentInChildren<PowerUpVisualScript>();
        _myRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Start() {
        _myRenderer.sprite = _mySO.sprite;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        PlayerScript player = collision.GetComponent<PlayerScript>();
        if (player != null) {

            PickUpPowerUp();
        }
    }

    private void PickUpPowerUp() {
        _myTrigger.enabled = false;
        _myVisual.PlayPickUpAnim();
    }
}
