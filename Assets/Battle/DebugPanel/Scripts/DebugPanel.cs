using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugPanel : MonoBehaviour
{
    public UnitController unit;
    public UnitMeta unitData;
    public EnemiesRectController enemiesRectCtrl;
    public BattleMenuManager battleMenuManager;

    public UnitLibrary unitLib;
    public ItemLibrary itemLib;

    public BattleResult battleResult;
    public List<Sprite> icons;

    public Inventory inventory;
    public Inventory inventory2;

    public void OnMoveClicked()
    {
        StartCoroutine(unit.Move(unit.transform.position - new Vector3(-1, 0, 0), 2.0f));
    }

    public void OnDieClicked()
    {
        StartCoroutine(unit.Die());
    }

    public void OnActClicked(string name)
    {
        StartCoroutine(unit.PlayAnimation(name));
    }

    public void OnBattleMenuClicked()
    {
        CreateL1Menu();
    }

    private void CreateL1Menu()
    {
        var menu = battleMenuManager.CreateMenu("l1", OnL1MenuItemClicked);
        menu.AddItem("attack", "攻击", "10", true);
        menu.AddItem("skill", "技能", "", true);
        menu.AddItem("defence", "防御", "", true);
        menu.AddItem("item", "道具", "", true);
        menu.AddItem("run", "逃跑", "", false);
    }

    private void CreateSkillMenu()
    {
        var menu = battleMenuManager.CreateMenu("l1", OnSkillMenuItemClicked);
        menu.AddItem("skill1", "圣光", "10", true);
        menu.AddItem("skill2", "顺劈斩", "20", true);
        menu.AddItem("skill3", "旋风斩", "60", false);
        menu.AddItem("skill4", "雷霆之锤", "30", true);
    }

    private void CreateItemsMenu()
    {
        var menu = battleMenuManager.CreateMenu("l1", OnItemMenuItemClicked);
        menu.AddItem("item1", "恢复药水", "10", true);
        menu.AddItem("item2", "解毒药水", "5", true);
        menu.AddItem("item3", "飞刀", "6", true);
        menu.AddItem("item4", "烟雾弹", "2", true);
        menu.AddItem("item5", "魔力药水", "12", true);
    }

    private void OnL1MenuItemClicked(string parentId, string id)
    {
        Debug.Log($"{id} clicked");
        switch (id)
        {
            case "attack":
                break;
            case "skill":
                CreateSkillMenu();
                break;
            case "defence":
                break;
            case "item":
                CreateItemsMenu();
                break;
            case "run":
                break;
        }
    }

    private void OnSkillMenuItemClicked(string parentId, string id)
    {
        Debug.Log($"{id} clicked");
    }

    private void OnItemMenuItemClicked(string parentId, string id)
    {
        Debug.Log($"{id} clicked");
    }

    private void BattleResult()
    {
        //battleResult.gameObject.SetActive(true);
        battleResult.Show(true);

        battleResult.SetBaseReward(10000, 5000);
        battleResult.SetExtraReward(123, Random.Range(0, 2));

        battleResult.AddExtraRewardType("无伤");
        battleResult.AddExtraRewardType("击中要害");

        int n = Random.Range(0, 10);
        for (int i = 0; i < n; i++)
        {
            int index = Random.Range(0, icons.Count);
            int count = Random.Range(1, 20);
            battleResult.AddRewardItem(icons[index], $"测试奖励{(char)('A' + index)}", count);
        }

        var item = battleResult.AddStatus("艾丽西娅");
        item.SetExp(50, 100);
        item.SetLevel(1);
        item.SetLevelUp(false);

        item = battleResult.AddStatus("欧菲莉亚");
        item.SetExp(5, 1000);
        item.SetLevel(30);
        item.SetLevelUp(true);
    }

    public void OnTestButtonClicked(string type)
    {
        switch (type)
        {
            case "CreateEnemy":
                enemiesRectCtrl.CreateEnemy(unitData.prefab);
                break;

            default:
                Invoke(type, 0.0f);
                break;
        }
    }

    private void Inventory()
    {
        inventory.InitInventory(ArchiveManager.player.inventories[0]);
        inventory.Show(true);

        inventory2.InitInventory(ArchiveManager.player.inventories[1]);
        inventory2.Show(true);
    }

    private void AddItem()
    {
        ItemMeta meta = itemLib.RandomItem();
        PlayerItemData data = new()
        {
            metaID = meta.id,
            title = meta.title
        };
        inventory.AddItem(data);
    }

    private void CreateArchive()
    {
        ArchiveManager.mock?.MockArchive("test");
    }

    private void LoadArchive()
    {
        ArchiveManager.player = ArchiveManager.LoadArchive(ArchiveManager.player.name);
    }

    private void SaveArchive()
    {
        ArchiveManager.SaveArchive(ArchiveManager.player);
    }
}
