using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    [SerializeField] private BombSO _mySO;
    [SerializeField] private BombVisualScript _myVisual;

    private bool _hasExploded = false;

    private void Start()
    {
        _hasExploded = false;
    }

    private void OnColliderEnter2D(Collider2D collision)
    {
        PlayerScript player = collision.GetComponent<PlayerScript>();
        if (player != null)
        {

            player.Damage(_mySO.attackPower);
            _hasExploded = true;

            if (_mySO.canBurn)
            {
                player.PlayerWasBurned();
            }
            if (_mySO.canParalyze)
            {
                player.PlayerWasParalyzed();
            }
            if (_mySO.canFreeze)
            {
                player.PlayerWasFrozen();
            }
            if (_mySO.canPoison)
            {
                player.PlayerWasPoisoned();
            }
            if (_mySO.canSlime)
            {
                player.PlayerWasSlimed();
            }
        }
    }

    private void Explode()
    {
        if (_hasExploded)
        {
            return;
        }
        _myVisual.Explode();
        _hasExploded = true;
    }
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_hasExploded) {
            return;
        }

        PlayerScript player = collision.GetComponent<PlayerScript>();
        EnemyBaseScript enemy = collision.GetComponent<EnemyBaseScript>();

        int halvedDamage = _mySO.attackPower / 2;

        if (player != null)
        {
            player.Damage(halvedDamage);

            if (_mySO.canBurn)
            {
                player.PlayerWasBurned();
            }
            if (_mySO.canParalyze)
            {
                player.PlayerWasParalyzed();
            }
            if (_mySO.canFreeze)
            {
                player.PlayerWasFrozen();
            }
            if (_mySO.canPoison)
            {
                player.PlayerWasPoisoned();
            }
            if (_mySO.canSlime)
            {
                player.PlayerWasSlimed();
            }
        }

        if(enemy != null)
        {
            enemy.Damage(halvedDamage);
        }
    }
}
