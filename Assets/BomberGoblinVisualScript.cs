using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberGoblinVisualScript : EnemyVisualBaseScript
{
    [SerializeField] private Transform _myBomb;

    public void ThrowBomb()
    {
        Transform myBomb = Instantiate(_myBomb);
        Rigidbody2D bombRigidBody = myBomb.GetComponent<Rigidbody2D>();

        bombRigidBody.AddForce(new Vector2(5f, 1f));
    }
}
