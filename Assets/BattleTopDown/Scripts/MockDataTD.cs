using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MockDataTD : MockData
{
    void Awake()
    {
        //GetComponent<ArchiveManager>();
        Debug.Log("child init");
    }

    public override void MockArchive(string name)
    {
        PlayerObject player = ArchiveManager.CreateArchive(name);

        // 创建英雄数据
        for (int i = 0; i < 4; i++)
        {
            // 创建测试角色
            var unitMeta = unitLib.RandomUnit();
            PlayerUnitData hero = new(unitMeta);
            hero.title = $"测试玩家{(char)('A' + i)}";
            hero.expLevel.exp = Random.Range(0, 50);
            hero.expLevel.level = 1;
            hero.expLevel.maxLevel = 99;
            hero.hp.hp = hero.hp.maxHP = Random.Range(100, 200);
            hero.sp = hero.maxSP = 25;
            hero.atk = Random.Range(10, 50);
            hero.def = Random.Range(20, 50);
            player.heroes.Add(hero);

            Debug.Log($"MockHero({hero.title}), atk({hero.atk})");
        }

        // 创建库存数据
        InventoryObject inventory = new();
        inventory.name = "背包";
        inventory.capacity = 50;

        int itemCount = Random.Range(20, 50);
        inventory.items = new();

        for (int i = 0; i < itemCount; i++)
        {
            var itemMeta = itemLib.RandomItem();
            PlayerItemData item = new();
            item.metaID = itemMeta.id;
            item.title = itemMeta.title;
            item.slot = Random.Range(0, inventory.capacity * 2 / 3);
            inventory.items.Add(item);
            //Debug.Log($"MockPlayerInfo.Inventory[0]: title({item.title}), slot({item.slot}))");
        }
        player.inventories.Add(inventory);

        inventory = new();
        inventory.name = "仓库";
        inventory.capacity = 50;

        itemCount = Random.Range(20, 50);
        inventory.items = new();

        for (int i = 0; i < itemCount; i++)
        {
            var itemMeta = itemLib.RandomItem();
            PlayerItemData item = new();
            item.metaID = itemMeta.id;
            item.title = itemMeta.title;
            item.slot = Random.Range(0, inventory.capacity * 2 / 3);
            inventory.items.Add(item);
            //Debug.Log($"MockPlayerInfo.Inventory[0]: title({item.title}), slot({item.slot}))");
        }
        player.inventories.Add(inventory);

        ArchiveManager.player = player;
    }

    [SerializeField] private UnitLibrary unitLib;
    [SerializeField] private ItemLibrary itemLib;
}
