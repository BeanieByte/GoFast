using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryMedalScript : MonoBehaviour
{
    private Collider2D _myTrigger;
    private VictoryMedalVisualScript _myVisual;

    private void Awake() {
        _myTrigger = GetComponent<Collider2D>();
        _myVisual = GetComponentInChildren<VictoryMedalVisualScript>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        PlayerScript player = collision.GetComponent<PlayerScript>();
        if (player != null) {
            GetVictoryMedal();
        }
    }

    private void GetVictoryMedal() {
        _myTrigger.enabled = false;
        _myVisual.PlayPickUpAnim();
    }
}
