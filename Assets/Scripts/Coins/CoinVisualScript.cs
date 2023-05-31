using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinVisualScript : MonoBehaviour
{
    private CoinScript _myLogicScript;
    private Animator _myAnimator;

    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int PickUp = Animator.StringToHash("PickUp");

    private void Awake() {
        _myLogicScript = GetComponentInParent<CoinScript>();
        _myAnimator = GetComponent<Animator>();
    }

    private void Start() {
        _myAnimator.CrossFade(Idle, 0f, 0);
    }

    public void PlayPickUpAnim() {
        _myAnimator.CrossFade(PickUp, 0f, 0);
    }

    public void DestroyCoin() {
        Destroy(_myLogicScript.gameObject);
    }
}
