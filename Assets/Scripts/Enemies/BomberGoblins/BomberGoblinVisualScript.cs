using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberGoblinVisualScript : EnemyVisualBaseScript
{
    [SerializeField] private Transform _myBomb;
    [SerializeField] private Transform _bombThrowStartPoint;
    [SerializeField] private Transform _bombHolder;

    [SerializeField] private float _minThrowVectorX;
    [SerializeField] private float _maxThrowVectorX;
    private float _throwVectorX;
    private float _vectorXDirection;
    private Quaternion _bombRotation;

    [SerializeField] private float _minThrowVectorY;
    [SerializeField] private float _maxThrowVectorY;
    private float _throwVectorY;

    [SerializeField] private float _minThrowForce;
    [SerializeField] private float _maxThrowForce;
    private float _throwForce;

    public void ThrowBomb()
    {
        if (!_myLogicScript.IsFacingRight()) {
            _vectorXDirection = -1f;
        } else _vectorXDirection = 1f;

        _throwVectorX = Random.Range(_minThrowVectorX, _maxThrowVectorX) * _vectorXDirection;
        _throwVectorY = Random.Range(_minThrowVectorY, _maxThrowVectorY);
        _throwForce = Random.Range(_minThrowForce, _maxThrowForce);

        Transform myBomb = Instantiate(_myBomb, _bombThrowStartPoint.position, _bombRotation, _bombHolder);

        myBomb.GetComponent<Rigidbody2D>().AddForce(new Vector2(_throwVectorX, _throwVectorY) * _throwForce, ForceMode2D.Impulse);
    }

    public override void Die()
    {
        if(_bombHolder.childCount > 0)
        {
            _bombHolder.DetachChildren();
        }
        base.Die();
    }
}
