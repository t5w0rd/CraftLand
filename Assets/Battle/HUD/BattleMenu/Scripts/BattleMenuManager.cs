using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// <para>战斗菜单管理器</para>
/// </summary>
public class BattleMenuManager : MonoBehaviour
{
    public BattleMenu menuPrefab;
    public Transform container;
    public GameObject btnCloseOne;
    public GameObject btnCloseAll;

    public Vector2 padding = new(10, -10);

    private readonly List<BattleMenu> menus = new();
    private int focus = -1;
    private int closeUntil = -1;

    void Start()
    {
        btnCloseOne.SetActive(false);
        btnCloseAll.SetActive(false);
    }
    
    /// <summary>
    /// 使用菜单ID创建菜单
    /// </summary>
    /// <param name="id">菜单ID</param>
    /// <param name="onMenuItemClicked">当菜单项被点击时触发</param>
    /// <param name="closeProtect">设置当前菜单层为保护层</param>
    /// <returns>菜单对象</returns>
    public BattleMenu CreateMenu(string id, OnMenuItemClicked onMenuItemClicked, bool closeProtect = false)
    {
        BattleMenu menu = Instantiate(menuPrefab, container);
        Vector3 pos;
        if (menus.Count == 0)
        {
            pos = Vector3.zero;
        }
        else
        {
            pos = menus[menus.Count - 1].transform.localPosition + new Vector3(padding.x, padding.y, 0);
        }
        menu.transform.localPosition = pos;
        menu.id = id;
        menu.onMenuItemClicked = onMenuItemClicked;

        menus.Add(menu);
        focus = menus.Count - 1;
        if (closeProtect)
        {
            closeUntil = menus.Count - 1;
        }

        int canClose = menus.Count - closeUntil - 1;
        if (canClose > 0)
        {
            btnCloseOne.SetActive(true);
            if (canClose > 1)
            {
                btnCloseAll.SetActive(true);
            }
        }

        return menu;
    }

    private int canCloseCount { get => menus.Count - closeUntil - 1; }

    /// <summary>
    /// <para>关闭最上层的非保护菜单</para>
    /// </summary>
    public void CloseOneMenu()
    {
        if (canCloseCount <= 0)
        {
            return;
        }

        int last = menus.Count - 1;
        Destroy(menus[last].gameObject);
        menus.RemoveAt(last);
        focus = menus.Count - 1;

        if (canCloseCount < 2)
        {
            btnCloseAll.SetActive(false);
            if (canCloseCount < 1)
            {
                btnCloseOne.SetActive(false);
            }
        }
    }

    /// <summary>
    /// <para>关闭将保护菜单之上的所有菜单</para>
    /// </summary>
    public void CloseAllMenus()
    {
        if (canCloseCount <= 0)
        {
            return;
        }

        for (int i = menus.Count - 1; i > closeUntil; i--)
        {
            Destroy(menus[i].gameObject);
            menus.RemoveAt(i);
        }
        focus = menus.Count - 1;
        btnCloseOne.SetActive(false);
        btnCloseAll.SetActive(false);
    }

    /// <summary>
    /// <para>销毁全部菜单，保护设置对该操作无效</para>
    /// </summary>
    public void DestroyAllMenus()
    {
        if (menus.Count == 0)
        {
            return;
        }

        for (int i = menus.Count - 1; i >= 0; i--)
        {
            Destroy(menus[i].gameObject);
        }
        menus.Clear();
        focus = -1;
        btnCloseOne.SetActive(false);
        btnCloseAll.SetActive(false);
    }

    /// <summary>
    /// <para>最上层菜单</para>
    /// </summary>
    public string focusMenu { get => focus < 0 ? null : menus[focus].id; }

    public void OnCloseOneMenuClicked()
    {
        CloseOneMenu();
    }

    public void OnCloseAllMenuClicked()
    {
        CloseAllMenus();
    }
}
