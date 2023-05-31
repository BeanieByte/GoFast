using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }

    private int _enemiesKilled = 0;

    public event EventHandler<OnEnemyKilledEventArgs> OnEnemyKilled;

    public class OnEnemyKilledEventArgs : EventArgs {
        public int totalKilledEnemies;
    }

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this);
        }
        Instance = this;

        _enemiesKilled = 0;
    }

    public void IncreaseKilledEnemiesCounter() {
        _enemiesKilled++;
        OnEnemyKilled?.Invoke(this, new OnEnemyKilledEventArgs {
            totalKilledEnemies = _enemiesKilled
        }) ;
    }
}
