using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCrush : MonoBehaviour
{
    private EnemyBaseScript _myEnemyBaseGameObject;

    public event EventHandler OnEnemyCrushed;

    private void Awake() {
        _myEnemyBaseGameObject = GetComponentInParent<EnemyBaseScript>();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        PlayerScript player = collision.gameObject.GetComponent<PlayerScript>();

        if (player != null) {
            float currentPlayerYPosition = player.PlayersYPosition();
            bool isPlayerInTheAir = (player.PlayersYVelocity() != 0f);

            if (currentPlayerYPosition > transform.position.y && isPlayerInTheAir) {
                OnEnemyCrushed?.Invoke(this, EventArgs.Empty);
                player.BounceOffCrush(_myEnemyBaseGameObject.EnemyBounceOffMultiplier());
            }

            if (_myEnemyBaseGameObject.CanBurn()) {
                player.Damage(_myEnemyBaseGameObject.TouchAttackPower());
                player.PlayerWasBurned();
                return;
            }

            if (_myEnemyBaseGameObject.CanParalyze())
            {
                player.PlayerWasParalyzed();
                return;
            }

            if (_myEnemyBaseGameObject.CanFreeze())
            {
                player.PlayerWasFrozen();
                return;
            }

            if (_myEnemyBaseGameObject.CanPoison())
            {
                player.PlayerWasPoisoned();
                return;
            }

            if (_myEnemyBaseGameObject.CanSlime())
            {
                player.PlayerWasSlimed();
                return;
            }
        }
    }
}
