using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>单位元数据</para>
/// </summary>
[CreateAssetMenu(fileName = "New Unit", menuName = "Game Data/New Unit")]
public class UnitMeta : ScriptableObject
{
    public GameObject prefab;

    public string title;

    public int maxHP = 100;
    public int maxSP = 20;

    public int atk = 1;
    public int def = 0;

    public string rewardName;
    public int gold;
    public int exp;

    public string id => name;
}
