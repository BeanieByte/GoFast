using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadlyWaterInstaKillScript : MonoBehaviour
{
    private int _instaKillDamage = 1000;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerScript player = collision.GetComponent<PlayerScript>();
        IDamageable damageable = collision.GetComponent<IDamageable>();

        player?.Damage(_instaKillDamage);

        damageable?.Damage(_instaKillDamage);
    }
}
