using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TGamePlay.TBattle;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TestReward();
    }

    void TestReward()
    {
        RewardRandom clothTable = new();
        clothTable.AddItem("0", 1, 50);
        clothTable.AddItem("1", 1, 10);
        clothTable.AddItem("2", 1, 20);

        RewardRandom table = new();
        table.AddItem("3", 1, 20);
        table.AddReward(clothTable, 80);

        List<int> res = new(new int[]{ 0, 0, 0, 0 });
        Debug.Log(res.Count);
        for (int i = 0; i < 10000; i++)
        {
            var items = table.GetItems();
            for (int j = 0; j < items.Count; j++)
            {
                res[items[j].name[0] - '0']++;
            }
        }
        Debug.Log($"0:{res[0]}, 1:{res[1]}, 2:{res[2]}, 3:{res[3]}");
    }

    void TestExpLevel()
    {
        Unit u = new(0, "TestUnit");

        int[] maxExpTable = { 0, 10, 20 };
        var expLevel = u.AddComponent(new ExpLevelAttribute(maxExpTable));
        Debug.Log($"Level: {expLevel.Level}/{expLevel.MaxLevel}, EXP: {expLevel.BaseExp}/{expLevel.Exp}/{expLevel.MaxExp}");

        expLevel.AddExp(5);
        Debug.Log($"Level: {expLevel.Level}/{expLevel.MaxLevel}, EXP: {expLevel.BaseExp}/{expLevel.Exp}/{expLevel.MaxExp}");

        expLevel.AddExp(5);
        Debug.Log($"Level: {expLevel.Level}/{expLevel.MaxLevel}, EXP: {expLevel.BaseExp}/{expLevel.Exp}/{expLevel.MaxExp}");

        expLevel.AddExp(5);
        Debug.Log($"Level: {expLevel.Level}/{expLevel.MaxLevel}, EXP: {expLevel.BaseExp}/{expLevel.Exp}/{expLevel.MaxExp}");

        expLevel.AddExp(5);
        Debug.Log($"Level: {expLevel.Level}/{expLevel.MaxLevel}, EXP: {expLevel.BaseExp}/{expLevel.Exp}/{expLevel.MaxExp}");

        expLevel.AddExp(5);
        Debug.Log($"Level: {expLevel.Level}/{expLevel.MaxLevel}, EXP: {expLevel.BaseExp}/{expLevel.Exp}/{expLevel.MaxExp}");
    }

    void TestHP()
    {
        Unit u = new(0, "TestUnit");
        var hp = u.AddComponent(new HPAttribute(100));
        Debug.Log($"HP: {hp.HP}/{hp.MaxHP}");

        hp.HP = 200;
        Debug.Log($"HP: {hp.HP}/{hp.MaxHP}");

        hp.MaxHPRaw = 400;
        Debug.Log($"HP: {hp.HP}/{hp.MaxHP}");

        hp.HP = 200;
        hp.MaxHPRaw = 200;
        Debug.Log($"HP: {hp.HP}/{hp.MaxHP}");
        Debug.Assert(hp.HP == 100);
    }

    void TestAttackAndDamage()
    {
        Unit u = new(0, "TestUnit");
        var fight = u.AddComponent(new FightAttribute());
        fight.ATKRaw = 100;
        fight.DEFRaw = 500;
        var hp = u.GetComponent<HPAttribute>();

        Unit u2 = new(0, "TestUnit2");
        u2.AddComponent(new HPAttribute(1000));
        var rpg2 = u2.AddComponent(new FightAttribute());
        rpg2.ATKRaw = 100;
        rpg2.DEFRaw = 500;
        var hp2 = u2.GetComponent<HPAttribute>();

        Debug.Log($"{u.Name} HP: {hp.HP}/{hp.MaxHP} ATK: {fight.ATK}, DEF: {fight.DEF}");
        Debug.Log($"{u2.Name} HP: {hp2.HP}/{hp2.MaxHP} ATK: {rpg2.ATK}, DEF: {rpg2.DEF}");

        var dmgInfo = fight.Attack();
        int dmg = rpg2.Damage(dmgInfo);
        Debug.Log(dmg);

        Debug.Log($"{u2.Name} HP: {hp2.HP}/{hp2.MaxHP} ATK: {rpg2.ATK}, DEF: {rpg2.DEF}");
    }
}
