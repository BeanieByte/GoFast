using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombTouchTriggerScript : MonoBehaviour
{
    [SerializeField] private BombSO _mySO;

    private BombScript _myLogicScript;

    private void Awake() {
        _myLogicScript = GetComponentInParent<BombScript>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        PlayerScript player = collision.GetComponent<PlayerScript>();
        if (player != null) {

            player.Damage(_mySO.attackPower);
            _myLogicScript.Explode();

            if (_mySO.canBurn) {
                player.PlayerWasBurned();
            }
            if (_mySO.canParalyze) {
                player.PlayerWasParalyzed();
            }
            if (_mySO.canFreeze) {
                player.PlayerWasFrozen();
            }
            if (_mySO.canPoison) {
                player.PlayerWasPoisoned();
            }
            if (_mySO.canSlime) {
                player.PlayerWasSlimed();
            }
        }
    }
}
