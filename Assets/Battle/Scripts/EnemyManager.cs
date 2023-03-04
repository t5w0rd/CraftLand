using System.Collections;
using System.Collections.Generic;
using TGamePlay.TBattle;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private static EnemyManager instance;

    void Awake()
    {
        if (instance)
        {
            Destroy(instance);
        }
        instance = this;
    }

    public static List<EnemyObject> Enemies { get; private set; }

    public static List<EnemyObject> SpawnEnemies(UnitLibrary unitLib, int min, int max)
    {
        return Enemies = instance.SimpleSpawn(unitLib, min, max);
    }

    private List<EnemyObject> SimpleSpawn(UnitLibrary unitLib, int min, int max)
    {
        List<EnemyObject> enemies = new();
        BattleStatistics bs = new();
        
        int count = Random.Range(min, max+1);
        for (int i = 0; i < count; i++)
        {
            UnitMeta data = unitLib.RandomUnit();
            int sameEnemyCount = bs.GetInteger("enemy/names", data.title);
            bs.ChangeIntegerValue("enemy/names", data.title, 1);

            EnemyObject enemy = new();
            enemy.origData = data;

            string unitName = $"{data.title}{(sameEnemyCount > 0 ? new string(new char[]{' ', (char)('A' + sameEnemyCount) }) : "")}";

            Unit unit = new(TGamePlay.TBattle.Object.GenLocalID(), unitName);

            // 添加HP特性
            unit.AddComponent(new HPAttribute(data.maxHP));

            // 添加Fight特性
            var fight = unit.AddComponent(new FightAttribute());
            fight.ATKRaw = data.atk;
            fight.DEFRaw = data.def;

            // 添加Reward特性
            var reward = DefaultRewardLibary.GetReward(data.rewardName);
            unit.AddComponent(new RewardAttribute(reward));

            enemy.SetUnit(unit);

            enemies.Add(enemy);
        }
        return enemies;
    }
}
