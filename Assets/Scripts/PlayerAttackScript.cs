using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerAttackScript : MonoBehaviour
{
    private int _attackPower = 0;

    public event EventHandler OnKillingEnemy;

    private void OnTriggerEnter2D(Collider2D collision) {
        IDamageable damageable = collision.GetComponent<IDamageable>();
        if (damageable == null) {
            return;
        }

        damageable.Damage(_attackPower);
        if (damageable.DeadCheck()){
            OnKillingEnemy.Invoke(this, EventArgs.Empty);
        }

    }

    public void SetAttackPower(int attackPower) {
        _attackPower = attackPower;
    }
}
