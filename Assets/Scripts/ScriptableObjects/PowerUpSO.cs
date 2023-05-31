using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/PowerUpSO")]
public class PowerUpSO : ScriptableObject
{
    [TitleGroup("PowerUp")]
    [HorizontalGroup("PowerUp/Split"), LabelWidth(150)]
    [VerticalGroup("PowerUp/Split/Left")]
    [BoxGroup("PowerUp/Split/Left/Type")]
    [PreviewField(60, ObjectFieldAlignment.Left), HideLabel]
    public Sprite sprite;

    [BoxGroup("PowerUp/Split/Left/Drag and Drop")]
    public Transform prefab;

    [BoxGroup("PowerUp/Split/Left/Drag and Drop")]
    public Material material;

    [VerticalGroup("PowerUp/Split/Right")]
    [BoxGroup("PowerUp/Split/Right/Stats and Values")]
    public string powerUpName;

    [BoxGroup("PowerUp/Split/Right/Stats and Values")]
    public bool healthRegen;

    [BoxGroup("PowerUp/Split/Right/Stats and Values")]
    public bool turboTimeDuplication;

    [BoxGroup("PowerUp/Split/Right/Stats and Values")]
    public bool extraAirJump;

    [BoxGroup("PowerUp/Split/Right/Stats and Values")]
    public bool invincibility;
}
