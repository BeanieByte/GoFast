using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchAttackTriggerScript : MonoBehaviour
{
    private EnemyBaseScript _myEnemyBaseGameObject;

    private void Awake() {
        _myEnemyBaseGameObject = GetComponentInParent<EnemyBaseScript>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {

        PlayerScript player = collision.GetComponent<PlayerScript>();
        if (player != null) {
            player.Damage(_myEnemyBaseGameObject.TouchAttackPower());

            if (_myEnemyBaseGameObject.CanBurn()) {
                player.PlayerWasBurned();
                return;
            }

            if (_myEnemyBaseGameObject.CanParalyze()) {
                player.PlayerWasParalyzed();
                return;
            }

            if (_myEnemyBaseGameObject.CanFreeze()) {
                player.PlayerWasFrozen();
                return;
            }

            if (_myEnemyBaseGameObject.CanPoison()) {
                player.PlayerWasPoisoned();
                return;
            }

            if (_myEnemyBaseGameObject.CanSlime()) {
                player.PlayerWasSlimed();
                return;
            }
        }
    }
}
