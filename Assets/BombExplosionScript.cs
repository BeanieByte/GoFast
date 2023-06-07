using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombExplosionScript : MonoBehaviour
{
    [SerializeField] private BombSO _mySO;

    private void OnTriggerEnter2D(Collider2D collision) {

        PlayerScript player = collision.GetComponent<PlayerScript>();
        EnemyBaseScript enemy = collision.GetComponent<EnemyBaseScript>();

        int halvedDamage = _mySO.attackPower / 2;

        if (player != null) {
            player.Damage(halvedDamage);

            if (_mySO.canBurn) {
                player.PlayerWasBurned();
            }
            if (_mySO.canParalyze) {
                player.PlayerWasParalyzed();
            }
            if (_mySO.canFreeze) {
                player.PlayerWasFrozen();
            }
            if (_mySO.canPoison) {
                player.PlayerWasPoisoned();
            }
            if (_mySO.canSlime) {
                player.PlayerWasSlimed();
            }
        }

        if (enemy != null) {
            enemy.Damage(halvedDamage);
        }
    }
}
