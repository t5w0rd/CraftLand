using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// <para>玩家存档数据中的经验等级数据</para>
/// </summary>
[System.Serializable]
public class PlayerExpLevelData
{
    public int level;
    public int maxLevel;
    public int exp;
}

/// <summary>
/// <para>玩家存档数据中的HP数据</para>
/// </summary>
[System.Serializable]
public class PlayerHPData
{
    public int hp;
    public int maxHP;
}

/// <summary>
/// <para>玩家存档数据中的单位数据</para>
/// </summary>
[System.Serializable]
public class PlayerUnitData
{
    public PlayerUnitData()
    {
    }

    public PlayerUnitData(UnitMeta data)
    {
        title = data.title;
        metaID = data.id;
        hp = new() {
            hp = data.maxHP,
            maxHP = data.maxHP
        };
        sp = maxSP = data.maxSP;

        atk = data.atk;
        def = data.def;

        expLevel = new();
    }

    public string metaID;  // 元数据ID
    public string title;

    public PlayerExpLevelData expLevel = new();

    public PlayerHPData hp = new();

    public int sp;
    public int maxSP;

    public int atk;
    public int def;
}

/// <summary>
/// <para>玩家存档数据中的道具数据</para>
/// </summary>
[System.Serializable]
public class PlayerItemData
{
    [System.Serializable]
    public class ItemHPData
    {
        public int hp;
        public int maxHP;
    }

    [System.Serializable]
    public class ItemExpLevelData
    {
        public int level;
        public int maxLevel;
        public int exp;
    }

    public string metaID;
    public string title;
    public int slot;
}

/// <summary>
/// <para>玩家存档数据中的库存数据</para>
/// </summary>
[System.Serializable]
public class PlayerInventoryData
{
    public string name;
    public int capacity;
    public PlayerItemData[] items;
}

/// <summary>
/// <para>运行时中的库存数据</para>
/// </summary>
public class InventoryObject
{
    public string name;
    public int capacity;
    public List<PlayerItemData> items = new();
}

/// <summary>
/// <para>运行时中的玩家对象数据</para>
/// </summary>
public class PlayerObject
{
    public string name;

    public List<PlayerUnitData> heroes = new();
    public List<InventoryObject> inventories = new();
}

/// <summary>
/// <para>存档管理器</para>
/// </summary>
public class ArchiveManager : Singleton<ArchiveManager>
{
    public UnitLibrary heroLib;
    public ItemLibrary itemLib;

    private MockData _mock;

    [System.Serializable]
    private class Archive
    {
        public string name;

        public PlayerUnitData[] heroes;

        public PlayerInventoryData[] inventories;
    }

    public static PlayerObject player { get; set; }

    public static int[] MaxExpTable = { 0, 10, 25, 50, 80, 120, 170, 230, 300, 400}; // TODO: 移走

    private void Awake()
    {
        _mock = GetComponent<MockData>();
        _mock?.MockArchive("test");
    }

    public static MockData mock => instance._mock;

    private static string NameToPath(string name)
    {
        string path = $"{Application.persistentDataPath}/playerdata_{name}.sav";
        return path;
    }

    /// <summary>
    /// <para>创建存档</para>
    /// </summary>
    /// <param name="name">存档名称</param>
    /// <returns>新创建的玩家数据</returns>
    public static PlayerObject CreateArchive(string name)
    {
        Archive archive = new();
        archive.name = name;
        return ArchiveToPlayerObject(archive);
    }

    /// <summary>
    /// <para>从存档中加载玩家</para>
    /// </summary>
    /// <param name="name">存档名称</param>
    /// <returns>运行时中的玩家数据对象</returns>
    public static PlayerObject LoadArchive(string name)
    {
        string path = NameToPath(name);
        StreamReader sw = new(path);
        string json = sw.ReadToEnd();
        sw.Close();
        Archive archive = JsonUtility.FromJson<Archive>(json);
        return ArchiveToPlayerObject(archive);
    }

    /// <summary>
    /// <para>保存玩家数据到存档</para>
    /// </summary>
    /// <param name="player">运行时中的玩家数据对象</param>
    public static void SaveArchive(PlayerObject player)
    {
        Archive archive = PlayerObjectToArchive(player);
        SaveArchive(archive);
    }

    private static void SaveArchive(Archive archive)
    {
        string path = NameToPath(archive.name);
        StreamWriter sw = new(path);
        string json = JsonUtility.ToJson(archive);
        sw.Write(json);
        sw.Close();
    }

    private static PlayerObject ArchiveToPlayerObject(Archive archive)
    {
        PlayerObject player = new(){
            name = archive.name
        };

        if (archive.heroes != null)
        {
            player.heroes.AddRange(archive.heroes);
        }

        if (archive.inventories != null)
        {
            for (int i = 0; i < archive.inventories.Length; i++)
            {
                PlayerInventoryData data = archive.inventories[i];
                InventoryObject inventoryObject = new()
                {
                    name = data.name,
                    capacity = data.capacity,
                    items = new(data.items)
                };
                player.inventories.Add(inventoryObject);
            }
        }

        return player;
    }

    private static Archive PlayerObjectToArchive(PlayerObject player)
    {
        Archive archive = new()
        {
            name = player.name
        };

        archive.heroes = player.heroes.ToArray();

        archive.inventories = new PlayerInventoryData[player.inventories.Count];
        for (int i = 0; i < player.inventories.Count; i++)
        {
            InventoryObject inventoryObject = player.inventories[i];
            archive.inventories[i] = new()
            {
                name = inventoryObject.name,
                capacity = inventoryObject.capacity,
                items = inventoryObject.items.ToArray()
            };
        }
        
        return archive;
    }
}
