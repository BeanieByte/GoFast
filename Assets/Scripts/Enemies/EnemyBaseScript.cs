using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseScript : MonoBehaviour, IDamageable
{
    [SerializeField] private EnemySO _mySO;
    [SerializeField] private EnemyVisualBaseScript _myVisual;

    private Rigidbody2D _myRigidBody;
    [SerializeField] private CapsuleCollider2D _attackCollider;

    private int _currentHealth;

    private void Awake() {
        _myRigidBody = GetComponent<Rigidbody2D>();

        _currentHealth = _mySO.health;
    }

    private void Update() {
        Debug.Log("Mushroom's health is " + _currentHealth);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        PlayerScript player = collision.GetComponent<PlayerScript>();
        IDamageable enemy = collision.GetComponent<IDamageable>();
        if (player != null) {
            player.Damage(_mySO.attackPower);
        }
        if (enemy != null) {
            enemy.Damage(_mySO.attackPower);
        }
    }


    public void Damage(int attackPower) {
        _currentHealth -= attackPower;
        DeadCheck();
    }

    public bool DeadCheck() {
        if (_currentHealth > 0) {
            return false;
        } else return true;
    }
}
