using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour {

    [SerializeField] private BombSO _mySO;
    [SerializeField] private BombVisualScript _myVisual;

    private Rigidbody2D _myRigidBody;

    private bool _hasExploded = false;

    private bool _setAlmostExploding = false;

    private float _myExplosionTimer;
    private float _closeToExplodingTime;
    private float _explodingTimeOffset = 3f;

    private void Awake() {
        _myRigidBody = GetComponent<Rigidbody2D>();
        _myExplosionTimer = _mySO.explosionTimer;
    }

    private void Start()
    {
        _hasExploded = false;

        _setAlmostExploding = false;
        _closeToExplodingTime = _mySO.explosionTimer / _explodingTimeOffset;

    }

    private void FixedUpdate() {
        if (_hasExploded) { return; }

        _myExplosionTimer -= Time.deltaTime;

        if (_myExplosionTimer <= _closeToExplodingTime) 
        {
            AlmostExploding();
            if (_myExplosionTimer > 0) {
                return;
            }
            Explode();
        }
    }

    private void AlmostExploding() {
        if (_setAlmostExploding) { return; }

        _myVisual.AlmostExploding();
        _setAlmostExploding = true;
    }

    public void Explode()
    {
        if (_hasExploded)
        {
            return;
        }
        _myVisual.Explode();
        _myRigidBody.constraints = RigidbodyConstraints2D.FreezePosition;
        _hasExploded = true;
    }
}
