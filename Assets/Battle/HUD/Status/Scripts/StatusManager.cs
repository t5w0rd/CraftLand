using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>状态栏管理器</para>
/// </summary>
public class StatusManager : MonoBehaviour
{
    public StatusItem itemPrefab;

    /// <summary>
    /// <para>创建状态栏项</para>
    /// </summary>
    /// <param name="unitName">单位名称</param>
    /// <param name="hp">当前HP</param>
    /// <param name="maxHP">最大HP</param>
    /// <param name="sp">当前SP</param>
    /// <param name="maxSP">最大SP</param>
    /// <returns>被创建的状态栏项</returns>
    public StatusItem CreateStatusItem(string unitName, int hp, int maxHP, int sp, int maxSP)
    {
        var item = Instantiate(itemPrefab, transform);
        item.SetUnitName(unitName);
        item.SetHP(hp, maxHP);
        item.SetSP(sp, maxSP);
        return item;
    }
}
