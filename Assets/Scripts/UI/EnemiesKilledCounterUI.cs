using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemiesKilledCounterUI : MonoBehaviour
{
    private TextMeshProUGUI _myText;

    private void Awake() {
        _myText = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start() {
        EnemyManager.Instance.OnEnemyKilled += Instance_OnEnemyKilled;
    }

    private void Instance_OnEnemyKilled(object sender, EnemyManager.OnEnemyKilledEventArgs e) {
        _myText.text = "x" + e.totalKilledEnemies.ToString();
    }
}
