using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image uiIcon;
    public GameObject uiStackRect;
    public Text uiStack;
    public GameObject uiSelected;

    public Inventory inventory { get; internal set; }
    public int index { get; internal set; }

    public void SetItem(Sprite icon, int count)
    {
        if (!uiIcon.gameObject.activeSelf)
        {
            uiIcon.gameObject.SetActive(true);
        }

        if (uiIcon.sprite != icon)
        {
            uiIcon.sprite = icon;
        }

        if (count > 1)
        {
            uiStackRect.SetActive(true);
            uiStack.text = $"{count}";
        }
        else
        {
            uiStackRect.SetActive(false);
        }
    }

    public void Clear()
    {
        uiIcon.gameObject.SetActive(false);
    }

    public bool selected
    {
        get => uiSelected.activeSelf;
        set => uiSelected.SetActive(value);
    }

    public void OnClicked()
    {
        inventory.OnSlotClicked(this);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        inventory.OnBeginDrag(this, eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        inventory.OnDrag(this, eventData);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        inventory.OnEndDrag(this, eventData);
    }

    public void SwapItem(InventorySlot slot)
    {
        var sprite = uiIcon.sprite;
        var stack = uiStack.text;

        uiIcon.sprite = slot.uiIcon.sprite;
        uiStack.text = slot.uiStack.text;

        slot.uiIcon.sprite = sprite;
        slot.uiStack.text = stack;
    }
}
