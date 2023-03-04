using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>道具数据库</para>
/// </summary>
[CreateAssetMenu(fileName = "New Item Library", menuName = "Game Data/New Item Library")]
public class ItemLibrary : ScriptableObject
{
    public List<ItemMeta> items;

    private Dictionary<string, ItemMeta> index = null;

    private void BuildIndex()
    {
        index = new();
        for (int i = 0; i < items.Count; i++)
        {
            index[items[i].id] = items[i];
        }
    }

    public ItemMeta this[string id]
    {
        get
        {
            if (index == null)
            {
                BuildIndex();
            }
            return index.TryGetValue(id, out ItemMeta value) ? value : null;
        }
        set
        {
            if (index == null)
            {
                BuildIndex();
            }
            index[id] = value;
        }
    }

    public ItemMeta RandomItem()
    {
        if (items.Count == 0)
        {
            return null;
        }
        int index = Random.Range(0, items.Count);
        return items[index];
    }
}
