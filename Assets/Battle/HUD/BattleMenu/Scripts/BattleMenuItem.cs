using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void OnMenuItemClicked(string parentId, string id);

/// <summary>
/// <para>战斗菜单项</para>
/// </summary>
public class BattleMenuItem : MonoBehaviour
{
    public Text uiTitle;
    public Text uiValue;

    Button uiButton;

    // Start is called before the first frame update
    void Awake()
    {
        uiTitle.text = "";
        uiValue.text = "";
        uiButton = GetComponent<Button>();
    }

    public bool interactable
    {
        get => uiButton.interactable;
        set => uiButton.interactable = value;
    }

    public string id { get; set; }
    public string parentId { get; set; }

    public string title
    {
        get => uiTitle.text;
        set => uiTitle.text = value;
    }

    public string value
    {
        get => uiValue.text;
        set => uiValue.text = value;
    }

    public OnMenuItemClicked onMenuItemClicked { get; set; }

    public void OnClicked()
    {
        if (onMenuItemClicked != null)
        {
            onMenuItemClicked.Invoke(parentId, id);
        }
    }
}
