using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombTouchTriggerScript : MonoBehaviour
{
    private BombScript _myLogicScript;

    private EnemyBaseScript _myEnemyParent;

    private void Awake() {
        _myLogicScript = GetComponentInParent<BombScript>();
        _myEnemyParent = GetComponentInParent<EnemyBaseScript>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        PlayerScript player = collision.GetComponent<PlayerScript>();
        EnemyBaseScript enemy = collision.GetComponent<EnemyBaseScript>();



        if (player != null || (enemy != null && enemy != _myEnemyParent)) {

            _myLogicScript.Explode();
        }
    }
}
