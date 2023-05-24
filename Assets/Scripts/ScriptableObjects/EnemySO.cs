using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/EnemySO")]
public class EnemySO : ScriptableObject
{
    public Transform prefab;
    public Sprite sprite;
    public string enemyName;
    public int health;
    public int attackPower;
    public float speed;
}
