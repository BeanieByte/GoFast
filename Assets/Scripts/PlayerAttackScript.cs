using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerAttackScript : MonoBehaviour
{
    private int _attackPower = 0;

    public event EventHandler OnKillingEnemy;

    private void Awake() {
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        IDamageable enemy = collision.GetComponent<IDamageable>();
        if (enemy == null) {
            return;
        }
        enemy.Damage(_attackPower);
        if (enemy.DeadCheck()){
            OnKillingEnemy.Invoke(this, EventArgs.Empty);
            Destroy(collision.gameObject);
        }

    }

    public void SetAttackPower(int attackPower) {
        _attackPower = attackPower;
    }
}
