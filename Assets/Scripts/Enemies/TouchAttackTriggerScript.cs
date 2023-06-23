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

            if (_myEnemyBaseGameObject.CanBurn()) {
                player.PlayerWasBurned();
            }

            else if (_myEnemyBaseGameObject.CanParalyze()) {
                player.PlayerWasParalyzed();
            }

            else if (_myEnemyBaseGameObject.CanFreeze()) {
                player.PlayerWasFrozen();
            }

            else if (_myEnemyBaseGameObject.CanPoison()) {
                player.PlayerWasPoisoned();
            }

            else if (_myEnemyBaseGameObject.CanSlime()) {
                player.PlayerWasSlimed();
            }

            player.Damage(_myEnemyBaseGameObject.TouchAttackPower());
        }
    }
}
