using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/CoinSO")]
public class CoinSO : ScriptableObject
{
    [TitleGroup("Coin")]
    [HorizontalGroup("Coin/Split"), LabelWidth(150)]
    [VerticalGroup("Coin/Split/Left")]
    [BoxGroup("Coin/Split/Left/Type")]
    [PreviewField(60, ObjectFieldAlignment.Left), HideLabel]
    public Sprite sprite;

    [BoxGroup("Coin/Split/Left/Drag and Drop")]
    public Transform prefab;

    [BoxGroup("Coin/Split/Left/Drag and Drop")]
    public Material material;


    [VerticalGroup("Coin/Split/Right")]
    [BoxGroup("Coin/Split/Right/Stats and Values")]
    public string coinName;

    [BoxGroup("Coin/Split/Right/Stats and Values")]
    public int value;
}
