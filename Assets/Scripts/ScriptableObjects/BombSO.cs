using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/BombSO")]
public class BombSO : ScriptableObject
{
    [TitleGroup("Bomb")]
    [HorizontalGroup("Bomb/Split"), LabelWidth(150)]
    [VerticalGroup("Bomb/Split/Left")]
    [BoxGroup("Bomb/Split/Left/Type")]
    [PreviewField(60, ObjectFieldAlignment.Left), HideLabel]
    public Sprite sprite;

    [BoxGroup("Bomb/Split/Left/Drag and Drop")]
    public Transform prefab;

    [BoxGroup("Bomb/Split/Left/Drag and Drop")]
    public Material material;


    [VerticalGroup("Bomb/Split/Right")]
    [BoxGroup("Bomb/Split/Right/Stats and Values")]
    public string bombName;

    [BoxGroup("Bomb/Split/Right/Stats and Values")]
    public int attackPower;

    [BoxGroup("Bomb/Split/Right/Stats and Values")]
    public bool canBurn;

    [BoxGroup("Bomb/Split/Right/Stats and Values")]
    public bool canParalyze;

    [BoxGroup("Bomb/Split/Right/Stats and Values")]
    public bool canFreeze;

    [BoxGroup("Bomb/Split/Right/Stats and Values")]
    public bool canPoison;

    [BoxGroup("Bomb/Split/Right/Stats and Values")]
    public bool canSlime;
}
