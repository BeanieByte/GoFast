using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance { get; private set; }

    private int _collectedCoins = 0;

    public event EventHandler<OnCoinCollectedEventArgs> OnCoinCollected;

    public class OnCoinCollectedEventArgs : EventArgs {
        public int collectedCoins;
    }

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this);
        }
        Instance = this;

        _collectedCoins = 0;
    }

    public void IncreaseCoinCounter(int increaseBy) {

        _collectedCoins += increaseBy;
        OnCoinCollected?.Invoke(this, new OnCoinCollectedEventArgs {
            collectedCoins = _collectedCoins
        });
    }
}
