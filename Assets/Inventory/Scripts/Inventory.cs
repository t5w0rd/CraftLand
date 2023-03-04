using System;
using System.Collections;
using System.Collections.Generic;
using TGamePlay.TBattle;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// <para>运行时的物品对象</para>
/// </summary>
public class ItemObject
{
    public ItemMeta origData;

    public Item item { get; private set; }

    public InventorySlotObject slot { get; internal set; }

    public void SetItem(PlayerItemData data)
    {
        Item item = new(TGamePlay.TBattle.Object.GenLocalID(), data.title);
        this.item = item;
    }
}

public class InventorySlotObject
{
    public InventorySlot ctrl;
    public List<ItemObject> stack = new();

    public int index => ctrl.index;

    public Inventory inventory => ctrl.inventory;

    public ItemMeta origData => stack.Count == 0 ? null : stack[0].origData;

    public bool AddItem(ItemObject item, bool updateUI = true)
    {
        if (!CanAdd(item.origData.id))
        {
            return false;
        }

        item.slot = this;
        stack.Add(item);
        if (updateUI)
        {
            ctrl.SetItem(item.origData.sprite, stack.Count);
        }
        return true;
    }

    public bool CanAdd(string id)
    {
        return stack.Count == 0 || (origData.id == id && stack.Count < stack[0].origData.stackLimit);
    }

    public void Clear()
    {
        stack.Clear();
        ctrl.Clear();
    }

    public void UpdateUI()
    {
        if (stack.Count > 0)
        {
            ctrl.SetItem(stack[0].origData.sprite, stack.Count);
        }
        else
        {
            ctrl.Clear();
        }
    }
}

public class Inventory : MonoBehaviour
{
    public Text uiTitle;

    public Text uiItemName;
    public Text uiItemDesc;
    public ScrollRect uiSlotList;
    public InventorySlot slotPrefab;
    public ItemLibrary itemLib;

    private CanvasGroup cg;

    private InventoryObject inventory;
    private List<ItemObject> items;
    private List<InventorySlotObject> slots;

    private InventorySlot dragSlot;
    private int selectedIndex;

    private void Awake()
    {
        cg = GetComponent<CanvasGroup>();
        Show(false);
    }

    public bool inited => inventory != null;

    private int curSortMode = 0;
    private string[] sortModes = new string[] { "kind", "rare", "id" };

    public void InitInventory(InventoryObject inventory)
    {
        if (inited)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                Destroy(slots[i].ctrl.gameObject);
            }
        }

        this.inventory = inventory;
        items = new(inventory.items.Count);

        uiTitle.text = inventory.name;

        // 创建槽位
        slots = new(inventory.capacity);
        for (int i = 0; i < inventory.capacity; i++)
        {
            InventorySlot ctrl = Instantiate(slotPrefab, uiSlotList.content.transform);
            ctrl.inventory = this;
            ctrl.index = i;
            ctrl.selected = false;

            InventorySlotObject slot = new()
            {
                ctrl = ctrl
            };
            slots.Add(slot);
        }

        // 将物品放入槽位
        for (int i = 0; i < inventory.items.Count; i++)
        {
            PlayerItemData itemData = inventory.items[i];
            if (itemData.slot >= slots.Count)
            {
                //Debug.LogWarning($"item slot({itemData.slot}) >= slots.Count({slots.Count})");
                continue;
            }

            ItemObject itemObject = new()
            {
                origData = itemLib[itemData.metaID],
            };
            itemObject.SetItem(itemData);

            //Debug.Log($"InitInventory.AddItemToSlot, title({itemData.title}), slot({itemData.slot})");

            InventorySlotObject slotObject = slots[itemData.slot];
            if (slotObject.AddItem(itemObject, false))
            {
                items.Add(itemObject);
            }
        }

        // 更新槽位显示
        for (int i = 0; i < inventory.capacity; i++)
        {
            slots[i].UpdateUI();
        }

        selected = -1;
    }

    internal void OnSlotClicked(InventorySlot slot)
    {
        if (selected == slot.index)
        {
            selected = -1;
        }
        else
        {
            selected = slot.index;
        }

        Debug.Log($"selected({selected})");
    }

    internal void OnBeginDrag(InventorySlot slot, PointerEventData eventData)
    {
        Debug.Log("begin drag");
        if (slot.inventory.slots[slot.index].stack.Count > 0)
        {
            slot.uiIcon.transform.SetParent(transform.parent);
            slot.uiIcon.transform.position = eventData.position;
            dragSlot = slot;
            return;
        }

        dragSlot = null;
    }

    internal void OnDrag(InventorySlot slot, PointerEventData eventData)
    {
        if (dragSlot == null)
        {
            return;
        }

        //Debug.Log("drag, " + eventData.pointerCurrentRaycast.gameObject);
        slot.uiIcon.transform.position = eventData.position;
    }

    internal void OnEndDrag(InventorySlot slot, PointerEventData eventData)
    {
        if (dragSlot == null)
        {
            return;
        }

        // 放回原处，后面只交换内容
        dragSlot.uiIcon.transform.SetParent(dragSlot.transform);
        dragSlot.uiIcon.transform.localPosition = Vector3.zero;

        //Debug.Log($"end drag, name({eventData.pointerCurrentRaycast.gameObject.name}), tag({eventData.pointerCurrentRaycast.gameObject.tag})");
        if (eventData.pointerCurrentRaycast.gameObject != null && eventData.pointerCurrentRaycast.gameObject.CompareTag("InventorySlot"))
        {
            InventorySlot endSlot = eventData.pointerCurrentRaycast.gameObject.GetComponent<InventorySlot>();
            Debug.Log("end, " + endSlot.index);
            if (endSlot != dragSlot)
            {
                SwapSlot(dragSlot.inventory.slots[dragSlot.index], endSlot.inventory.slots[endSlot.index]);
                return;
            }
        }
    }

    /// <summary>
    /// <para>首先尝试向目标槽位进行合并，如果不能合并则进行交换</para>
    /// </summary>
    /// <param name="from">起始槽位</param>
    /// <param name="to">目标槽位</param>
    private void SwapSlot(InventorySlotObject from, InventorySlotObject to)
    {
        var metaID = from.origData.id;
        if (to.CanAdd(metaID))
        {
            // 将当前槽位中的物品尝试向目标槽位合并，直到不能合并为止
            int i;
            for (i = from.stack.Count - 1; i >= 0 && to.AddItem(from.stack[i], false); i--)
            {
                if (from.inventory != to.inventory)
                {
                    to.inventory.items.Add(from.stack[i]);
                    from.inventory.items.Remove(from.stack[i]);
                }
            }
            Debug.Log($"remove range: {i + 1}, {from.stack.Count - i - 1}");
            from.stack.RemoveRange(i + 1, from.stack.Count - i - 1);
        }
        else
        {
            // 交换剩余物品
            var tmpStack = from.stack;
            from.stack = to.stack;
            to.stack = tmpStack;
            for (int i = 0; i < from.stack.Count; i++)
            {
                from.stack[i].slot = from;
                if (from.inventory != to.inventory)
                {
                    from.inventory.items.Add(from.stack[i]);
                    to.inventory.items.Remove(from.stack[i]);
                }
            }
            for (int i = 0; i < to.stack.Count; i++)
            {
                to.stack[i].slot = to;
                if (from.inventory != to.inventory)
                {
                    to.inventory.items.Add(to.stack[i]);
                    from.inventory.items.Remove(to.stack[i]);
                }
            }
            from.ctrl.SwapItem(to.ctrl);
        }

        to.UpdateUI();
        from.UpdateUI();
        selected = selected;
        if (from.inventory != to.inventory)
        {
            to.inventory.selected = to.inventory.selected;
        }
    }

    public int selected
    {
        get => selectedIndex;
        set
        {
            if (selectedIndex >= 0)
            {
                slots[selectedIndex].ctrl.selected = false;
            }

            if (value < 0)
            {
                selectedIndex = -1;
                uiItemName.text = "";
                uiItemDesc.text = "";
                return;
            }

            selectedIndex = value >= slots.Count ? slots.Count - 1 : value;
            slots[selectedIndex].ctrl.selected = true;
            var data = slots[selectedIndex].origData;
            if (data == null)
            {
                uiItemName.text = "";
                uiItemDesc.text = "";
                return;
            }
            uiItemName.text = data.title;
            uiItemDesc.text = data.description;
        }
    }

    public void Show(bool value)
    {
        if (value)
        {
            cg.alpha = 1;
            cg.interactable = true;
            cg.blocksRaycasts = true;
        }
        else
        {
            SyncInventoryObject();
            cg.alpha = 0;
            cg.interactable = false;
            cg.blocksRaycasts = false;
        }
    }

    public bool visible => cg.alpha == 1.0f;

    private int SortComparison(ItemObject a, ItemObject b)
    {
        int compare = 0;
        int mode = curSortMode;
        for (; ; )
        {
            switch (sortModes[mode])
            {
                case "kind":
                    compare = b.origData.kind - a.origData.kind;
                    break;
                case "rare":
                    compare = b.origData.rare - a.origData.rare;
                    break;
                case "id":
                    compare = b.origData.id.CompareTo(a.origData.id);
                    break;
            }
            if (compare != 0)
            {
                return compare;
            }

            mode = (mode + 1) % sortModes.Length;
            if (mode == curSortMode)
            {
                return compare;
            }
        }
    }

    public void Sort()
    {
        items.Sort(SortComparison);

        int slotIndex = -1;
        //slots[0].Clear();
        // 将物品放入槽位
        for (int i = 0; i < items.Count; i++)
        {
            if (slotIndex >= 0 && slots[slotIndex].AddItem(items[i]))
            {
                continue;
            }

            slotIndex++;
            slots[slotIndex].Clear();
            slots[slotIndex].AddItem(items[i]);
        }

        // 清空之后的槽位
        for (int i = slotIndex + 1; i < inventory.capacity; i++)
        {
            slots[i].Clear();
        }

        selected = selected;
        curSortMode = (curSortMode + 1) % sortModes.Length;
    }

    public bool AddItem(PlayerItemData data)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            InventorySlotObject slot = slots[i];
            if (!slot.CanAdd(data.metaID))
            {
                continue;
            }

            ItemObject itemObject = new()
            {
                origData = itemLib[data.metaID],
            };
            itemObject.SetItem(data);
            slots[i].AddItem(itemObject);
            data.slot = i;
            items.Add(itemObject);
            selected = selected;
            Debug.Log($"Add item({itemObject.item.Name}) to slot({i})");
            return true;
        }

        return false;
    }

    public void SyncInventoryObject()
    {
        if (!inited)
        {
            return;
        }

        inventory.items.Clear();
        for (int i = 0; i < items.Count; i++)
        {
            ItemObject item = items[i];

            PlayerItemData data = new()
            {
                metaID = item.origData.id,
                title = item.item.Name,
                slot = item.slot.index
            };
            inventory.items.Add(data);
        }
    }
}
