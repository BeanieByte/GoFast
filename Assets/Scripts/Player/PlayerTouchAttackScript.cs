using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTouchAttackScript : MonoBehaviour
{
    public event EventHandler OnInvincibleTouchKillingEnemy;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyBaseScript enemy = collision.GetComponent<EnemyBaseScript>();
        if (enemy != null)
        {
            enemy.Damage(enemy.MyMaxHealth());
            OnInvincibleTouchKillingEnemy?.Invoke(this, EventArgs.Empty);
        }
    }
}
