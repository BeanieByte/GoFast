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


    private float _bombTickingDownCurrentTime;
    private float _bombTickingDownMaxTime;
    private float _bombTickingDownDefaultMaxTime = 0.2f;
    private float _bombTickingDownFasterMaxTime = 0.1f;

    private void Awake() {
        _myRigidBody = GetComponent<Rigidbody2D>();
        _myExplosionTimer = _mySO.explosionTimer;
    }

    private void Start()
    {
        _hasExploded = false;

        _setAlmostExploding = false;
        _closeToExplodingTime = _mySO.explosionTimer / _explodingTimeOffset;

        //sound related variables
        _bombTickingDownMaxTime = _bombTickingDownDefaultMaxTime;
        _bombTickingDownCurrentTime = _bombTickingDownMaxTime;

    }

    private void FixedUpdate() {
        if (_hasExploded) { return; }

        BombTickingDownSound();

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

        _bombTickingDownMaxTime = _bombTickingDownFasterMaxTime;
    }

    public void Explode()
    {
        if (_hasExploded)
        {
            return;
        }
        SoundManager.Instance.PlayBombExplosionSound(transform.position);
        _myVisual.Explode();
        _myRigidBody.constraints = RigidbodyConstraints2D.FreezePosition;
        _hasExploded = true;
    }

    private void BombTickingDownSound() {

        if (_bombTickingDownCurrentTime == _bombTickingDownMaxTime) {
            SoundManager.Instance.PlayBombTickingDownSound(transform.position);
        }

        _bombTickingDownCurrentTime -= Time.deltaTime;

        if (_bombTickingDownCurrentTime <= 0) {
            _bombTickingDownCurrentTime = _bombTickingDownMaxTime;
        }
    }
}
