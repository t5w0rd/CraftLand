using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>单位数据库</para>
/// </summary>
[CreateAssetMenu(fileName = "New Unit Library", menuName = "Game Data/New Unit Library")]
public class UnitLibrary : ScriptableObject
{
    public List<UnitMeta> units;

    private Dictionary<string, UnitMeta> index = null;

    private void BuildIndex()
    {
        index = new();
        for (int i = 0; i < units.Count; i++)
        {
            index[units[i].id] = units[i];
        }
    }

    public UnitMeta this[string id]
    {
        get
        {
            if (index == null)
            {
                BuildIndex();
            }
            return index.TryGetValue(id, out UnitMeta value) ? value : null;
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

    public UnitMeta RandomUnit()
    {
        if (units.Count == 0)
        {
            return null;
        }
        int index = Random.Range(0, units.Count);
        return units[index];
    }
}
