using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinScript : MonoBehaviour
{
    [SerializeField] private CoinSO _mySO;
    private Collider2D _myTrigger;
    private CoinVisualScript _myVisual;

    private void Awake() {
        _myTrigger = GetComponent<Collider2D>();
        _myVisual = GetComponentInChildren<CoinVisualScript>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        PlayerScript player = collision.GetComponent<PlayerScript>();
        if (player != null) {
            PickUpCoin();
        }
    }

    private void PickUpCoin() {
        _myTrigger.enabled = false;
        CoinManager.Instance.IncreaseCoinCounter(_mySO.value);
        _myVisual.PlayPickUpAnim();
    }
}
