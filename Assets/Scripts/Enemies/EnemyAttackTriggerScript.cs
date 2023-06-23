using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackTriggerScript : MonoBehaviour
{
    private EnemyBaseScript _myEnemyBaseGameObject;

    private void Awake() {
        _myEnemyBaseGameObject = GetComponentInParent<EnemyBaseScript>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {

        PlayerScript player = collision.GetComponent<PlayerScript>();
        if (player != null) {

            if (_myEnemyBaseGameObject.CanBurn()) {
                player.PlayerWasBurned();
            }

            if (_myEnemyBaseGameObject.CanParalyze()) {
                player.PlayerWasParalyzed();
            }

            if (_myEnemyBaseGameObject.CanFreeze()) {
                player.PlayerWasFrozen();
            }

            if (_myEnemyBaseGameObject.CanPoison()) {
                player.PlayerWasPoisoned();
            }

            if (_myEnemyBaseGameObject.CanSlime()) {
                player.PlayerWasSlimed();
            }

            player.Damage(_myEnemyBaseGameObject.AttackPower());
        }
    }
}
