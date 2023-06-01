using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpVisualScript : MonoBehaviour
{
    private PowerUpScript _myLogicScript;
    private Animator _myAnimator;

    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int PickUp = Animator.StringToHash("PickUp");

    private void Awake() {
        _myLogicScript = GetComponentInParent<PowerUpScript>();
        _myAnimator = GetComponent<Animator>();
    }

    private void Start() {
        _myAnimator.CrossFade(Idle, 0f, 0);
    }

    public void PlayPickUpAnim() {
        _myAnimator.CrossFade(PickUp, 0f, 0);
    }

    public void DestroyPowerUp() {
        Destroy(_myLogicScript.gameObject);
    }
}
