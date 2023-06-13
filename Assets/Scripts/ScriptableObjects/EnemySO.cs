using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "ScriptableObjects/EnemySO")]
public class EnemySO : ScriptableObject
{

    [TitleGroup("Enemy")]
    [HorizontalGroup("Enemy/Split"), LabelWidth(150)]
    [VerticalGroup("Enemy/Split/Left")]
    [BoxGroup("Enemy/Split/Left/Type")]
    [PreviewField(60, ObjectFieldAlignment.Left), HideLabel]
    public Sprite sprite;

    [BoxGroup("Enemy/Split/Left/Drag and Drop")]
    public Transform prefab;

    [BoxGroup("Enemy/Split/Left/Drag and Drop")]
    public Material material;


    [VerticalGroup("Enemy/Split/Right")]
    [BoxGroup("Enemy/Split/Right/Stats and Values")]
    [Required]
    public string enemyName;

    [BoxGroup("Enemy/Split/Right/Stats and Values")]
    [Required]
    public int health;

    [BoxGroup("Enemy/Split/Right/Stats and Values")]
    [Required]
    public int touchPower;

    [BoxGroup("Enemy/Split/Right/Stats and Values")]
    [Required]
    public float speed;

    [BoxGroup("Enemy/Split/Right/Stats and Values")]
    public int attackPower;

    [BoxGroup("Enemy/Split/Right/Stats and Values")]
    public float attackCooldownTime;

    [BoxGroup("Enemy/Split/Right/Stats and Values")]
    public bool canBurn;

    [BoxGroup("Enemy/Split/Right/Stats and Values")]
    public bool canParalyze;

    [BoxGroup("Enemy/Split/Right/Stats and Values")]
    public bool canFreeze;

    [BoxGroup("Enemy/Split/Right/Stats and Values")]
    public bool canPoison;

    [BoxGroup("Enemy/Split/Right/Stats and Values")]
    public bool canSlime;

    [BoxGroup("Enemy/Split/Right/Stats and Values")]
    public float bounceOffMultiplier;
}
