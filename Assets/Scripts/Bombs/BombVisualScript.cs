using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombVisualScript : MonoBehaviour
{

    private BombScript _myLogicScript;
    private Animator _myAnimator;

    private static readonly int ThrownAnim = Animator.StringToHash("Thrown");
    private static readonly int AlmostExplodingAnim = Animator.StringToHash("AlmostExploding");
    private static readonly int ExplodeAnim = Animator.StringToHash("Explode");

    private void Awake() {
        _myLogicScript = GetComponentInParent<BombScript>();
        _myAnimator = GetComponent<Animator>();
    }

    private void Start() {
        _myAnimator.CrossFade(ThrownAnim, 0f, 0);
    }

    public void AlmostExploding() {
        _myAnimator.CrossFade(AlmostExplodingAnim, 0f, 0);
    }

    public void Explode() {
        _myAnimator.CrossFade(ExplodeAnim, 0f, 0);
    }

    public void DestroyBomb() {
        Destroy(_myLogicScript.gameObject);
    }
}
