using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemRareLevel
{
    Poor = 1,
    Normal = 2,
    Rare = 3,
    Uncommon = 4,
    Epic = 5,
    Legendary = 6,
}

public enum ItemKind
{
    Material = 1,
    Equipment = 2,
    Edible = 3,
    SkillScroll = 4,
}

/// <summary>
/// <para>道具数据</para>
/// </summary>
[CreateAssetMenu(fileName = "New Item", menuName = "Game Data/New Item")]
public class ItemMeta : ScriptableObject
{
    public Sprite sprite;
    public string title;
    [TextArea] public string description;
    public ItemRareLevel rare;
    public ItemKind kind;
    public int stackLimit;

    public string id => name;
}
