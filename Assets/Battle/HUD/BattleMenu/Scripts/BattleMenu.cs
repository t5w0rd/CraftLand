using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>战斗菜单</para>
/// </summary>
public class BattleMenu : MonoBehaviour
{
    public BattleMenuItem itemPrefab;
    public Transform container;

    public void AddItem(string id, string title, string value, bool enabled)
    {
        var item = Instantiate(itemPrefab, container);
        item.id = id;
        item.parentId = this.id;
        item.title = title;
        item.value = value;
        item.interactable = enabled;
        item.onMenuItemClicked = onMenuItemClicked;
    }

    public string id { get; set; }

    public OnMenuItemClicked onMenuItemClicked { get; set; }
}
