using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinCounterUI : MonoBehaviour
{
    private TextMeshProUGUI _myText;

    private void Awake() {
        _myText = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start() {
        CoinManager.Instance.OnCoinCollected += Instance_OnCoinCollected;
    }

    private void Instance_OnCoinCollected(object sender, CoinManager.OnCoinCollectedEventArgs e) {
        _myText.text = "x" + e.collectedCoins.ToString();
    }

    private void OnDestroy() {
        CoinManager.Instance.OnCoinCollected -= Instance_OnCoinCollected;
    }
}
