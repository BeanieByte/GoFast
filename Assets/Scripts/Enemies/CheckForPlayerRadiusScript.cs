using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForPlayerRadiusScript : MonoBehaviour {

    public event EventHandler OnPlayerDetected;

    public event EventHandler OnPlayerMissing;

    private Vector2 _playerPosition;

    private void OnTriggerStay2D(Collider2D collision) {

        PlayerScript player = collision.GetComponent<PlayerScript>();

        if (player == null) { 
            return;
        }

        _playerPosition = player.transform.position;
        OnPlayerDetected?.Invoke(this, EventArgs.Empty);
    }

    private void OnTriggerExit2D(Collider2D collision) {

        PlayerScript player = collision.GetComponent<PlayerScript>();

        if (player == null) {
            return;
        }

        OnPlayerMissing?.Invoke(this, EventArgs.Empty);
    }

    public float PlayersCurrentXPosition() {
        return _playerPosition.x;
    }
}
