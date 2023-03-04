using System.Collections;
using System.Collections.Generic;
using TGamePlay.TAction;
using TGamePlay.TBattle;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// <para>运行时的单位对象</para>
/// </summary>
public class UnitObject
{
    public UnitMeta origData;

    public Unit unit { get; protected set; }

    public UnitController ctrl;

    public FightAttribute fight { get; protected set; }

    public void SetUnit(Unit unit)
    {
        this.unit = unit;
        fight = unit.GetComponent<FightAttribute>();
    }
}

/// <summary>
/// <para>运行时的英雄单位对象</para>
/// </summary>
public class HeroObject : UnitObject
{
    public HeroRectController rectCtrl;
    public StatusItem statusItem;

    public void UpdateStatus()
    {
        statusItem.SetHP(fight.HP.HP, fight.HP.MaxHP);
    }

    public void SetPlayerUnit(PlayerUnitData data)
    {
        Unit unit = new(TGamePlay.TBattle.Object.GenLocalID(), data.title);
        var hp = unit.AddComponent(new HPAttribute(data.hp.maxHP));
        hp.HP = data.hp.hp;
        fight = unit.AddComponent(new FightAttribute());
        fight.ATKRaw = data.atk;
        fight.DEFRaw = data.def;
        var expLevel = unit.AddComponent(new ExpLevelAttribute(ArchiveManager.MaxExpTable));
        expLevel.AddExp(data.expLevel.exp);

        this.unit = unit;
    }
}

/// <summary>
/// <para>运行时的敌人单位对象</para>
/// </summary>
public class EnemyObject : UnitObject
{
}

enum BattleState
{
    Running,
    Waiting,
    Quit
}

/// <summary>
/// <para>战斗管理器</para>
/// </summary>
public class BattleManager : MonoBehaviour
{
    public List<HeroRectController> heroRectCtrls;
    public EnemiesRectController enemiesRectCtrl;
    public Transform orderPoint;
    public Cursor cursor;
    public BattleHUD hud;
    public BattleMenuManager battleMenuManager;
    public StatusManager statusManager;
    public BattleResult battleResult;
    public Inventory inventory;
    public Inventory storehouse;

    public UnitLibrary unitLib;
    public UnitLibrary enemyLib;
    public ItemLibrary itemLib;

    private static BattleManager instance;

    private PlayerObject player;
    private List<HeroObject> heroes = new();
    private List<EnemyObject> enemies;

    private HeroObject curHero;
    private EnemyObject curEnemy;
    private BattleState state = BattleState.Running;

    private UnitObject selectedTarget = null;

    private BattleStatistics statistics = new();

    void Awake()
    {
        if (instance)
        {
            Destroy(instance);
        }
        instance = this;
    }

    void Start()
    {
        MockSpawnEnemies();

        cursor.SetActive(false);

        StartCoroutine(StartBattle());
    }

    private void MockSpawnEnemies()
    {
        EnemyManager.SpawnEnemies(enemyLib, 1, 2);
    }

    /// <summary>
    /// <para>开始战斗</para>
    /// </summary>
    /// <returns></returns>
    public static IEnumerator StartBattle()
    {
        yield return instance.InitBattle(ArchiveManager.player, EnemyManager.Enemies);

        int curHeroIndex = 0;
        int curEnemyIndex = -1;
        while (instance.state != BattleState.Quit)
        {
            switch (instance.state)
            {
                case BattleState.Running:
                    if (curEnemyIndex < 0)
                    {
                        yield return instance.OrderHero(instance.heroes[curHeroIndex]);
                        WaitPlayerOperation();
                        curHeroIndex++;
                        if (curHeroIndex >= instance.heroes.Count)
                        {
                            curHeroIndex = -1;
                            curEnemyIndex = 0;
                        }
                    }
                    else
                    {
                        yield return instance.OrderEnemy(instance.enemies[curEnemyIndex]);
                        curEnemyIndex++;
                        if (curEnemyIndex >= instance.enemies.Count)
                        {
                            curEnemyIndex = -1;
                            curHeroIndex = 0;
                            //yield return new WaitForSeconds(0.5f);
                        }
                    }
                    break;

                case BattleState.Waiting:
                    yield return new WaitForFixedUpdate();
                    break;

                case BattleState.Quit:
                    yield break;
            }
        }
    }

    /// <summary>
    /// <para>退出战斗循环</para>
    /// </summary>
    public static void QuitBattle()
    {
        instance.state = BattleState.Quit;
    }

    /// <summary>
    /// <para>等待玩家操作，战斗循环进入等待状态</para>
    /// </summary>
    public static void WaitPlayerOperation()
    {
        if (instance.state == BattleState.Quit)
        {
            return;
        }
        instance.state = BattleState.Waiting;
    }

    /// <summary>
    /// <para>战斗循环恢复运行，玩家完成操作后可通过该操作恢复战斗循环</para>
    /// </summary>
    public static void ContinueBattle()
    {
        if (instance.state == BattleState.Quit)
        {
            return;
        }
        instance.state = BattleState.Running;
    }

    /// <summary>
    /// <para>使用玩家数据初始化战场，创建战场单位</para>
    /// </summary>
    /// <param name="player">游戏运行时的玩家数据</param>
    /// <returns></returns>
    private IEnumerator InitBattle(PlayerObject player, List<EnemyObject> enemies)
    {
        instance.player = player;
        instance.enemies = enemies;
        instance.state = BattleState.Running;

        instance.CreateEnemies();
        yield return new WaitForSeconds(1);
        yield return instance.CreateHeroes();
    }

    /// <summary>
    /// <para>根据玩家数据在战场创建英雄</para>
    /// </summary>
    /// <returns></returns>
    private IEnumerator CreateHeroes()
    {
        List<HeroEntering> enterings = new();
        for (int i = 0; i < player.heroes.Count && i < heroRectCtrls.Count; i++)
        {
            PlayerUnitData heroData = player.heroes[i];

            HeroObject hero = new();
            hero.SetPlayerUnit(heroData);

            hero.statusItem = statusManager.CreateStatusItem(heroData.title, heroData.hp.hp, heroData.hp.maxHP, heroData.sp, heroData.maxSP);
            hero.rectCtrl = heroRectCtrls[i];
            hero.origData = unitLib[heroData.metaID];
            var entering = hero.rectCtrl.CreateHeroAndEnter(hero.origData.prefab, 1.0f);
            hero.ctrl = entering.hero;
            enterings.Add(entering);

            heroes.Add(hero);
        }

        for (int i = 0; i < enterings.Count; i++)
        {
            yield return enterings[i].moving;
        }
    }

    private void CreateEnemies()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            EnemyObject enemy = enemies[i];
            UnitController ctrl = enemiesRectCtrl.CreateEnemy(enemy.origData.prefab);
            if (ctrl == null)
            {
                break;
            }
            enemy.ctrl = ctrl;
        }
    }

    /// <summary>
    /// <para>对给定英雄发号命令</para>
    /// </summary>
    /// <param name="hero">等待下令的英雄</param>
    /// <returns></returns>
    private IEnumerator OrderHero(HeroObject hero)
    {
        instance.curHero = hero;
        hero.statusItem.focus = true;
        yield return hero.ctrl.Move(instance.orderPoint.position, 0.5f);
        instance.CreateOrderMenu();
    }

    private IEnumerator DoOrderAttack()
    {
        yield return new WaitForSeconds(0.2f);
        yield return curHero.ctrl.Attack();

        DamageInfo dmg = curHero.fight.Attack();
        int v = selectedTarget.fight.Damage(dmg);
        hud.PlayDamageText(-v, selectedTarget.ctrl.transform);

        Debug.Log($"{selectedTarget.unit.Name}: HP: {selectedTarget.fight.HP.HP}/{selectedTarget.fight.HP.MaxHP}");
        if (selectedTarget.fight.HP.HP == 0)
        {
            StartCoroutine(EnemyDie(selectedTarget as EnemyObject));
            if (enemies.Count == 0)
            {
                QuitBattle();
                BattleResult();
                yield break;
            }
        }
        else
        {
            StartCoroutine(selectedTarget.ctrl.Shake());
        }

        yield return OrderDone();
        ContinueBattle();
    }

    private IEnumerator DoOrderSpell()
    {
        battleMenuManager.DestroyAllMenus();
        yield return new WaitForSeconds(0.2f);
        yield return curHero.ctrl.Spell();
        yield return OrderDone();
        ContinueBattle();
    }

    private IEnumerator DoOrderUseItem()
    {
        battleMenuManager.DestroyAllMenus();
        yield return new WaitForSeconds(0.2f);
        yield return curHero.ctrl.PowerUp();
        yield return OrderDone();
        ContinueBattle();
    }

    private IEnumerator DoOrderDefence()
    {
        battleMenuManager.DestroyAllMenus();
        yield return new WaitForSeconds(0.2f);
        yield return OrderDone();
        ContinueBattle();
    }

    private IEnumerator DoOrderEscape()
    {
        battleMenuManager.DestroyAllMenus();
        yield return new WaitForSeconds(0.2f);
        yield return OrderDone();
        QuitBattle();
    }

    private IEnumerator OrderDone()
    {
        curHero.statusItem.focus = false;
        yield return curHero.rectCtrl.MoveBackwardsToIdle(0.5f);
    }

    private void CreateOrderMenu()
    {
        var menu = battleMenuManager.CreateMenu("order", OnOrderMenuItemClicked, true);
        menu.AddItem("attack", "攻击", $"{curHero.fight.ATK}", true);
        menu.AddItem("skill", "技能", "", true);
        menu.AddItem("defence", "防御", $"{curHero.fight.DEF}", true);
        menu.AddItem("item", "道具", "", true);
        menu.AddItem("escape", "逃跑", "", true);
    }

    private void CreateSkillMenu()
    {
        var menu = battleMenuManager.CreateMenu("skill", OnSkillMenuItemClicked);
        menu.AddItem("skill1", "圣光", "10", true);
        menu.AddItem("skill2", "顺劈斩", "20", true);
        menu.AddItem("skill3", "旋风斩", "60", false);
        menu.AddItem("skill4", "雷霆之锤", "30", true);
    }

    private void CreateItemsMenu()
    {
        var menu = battleMenuManager.CreateMenu("items", OnItemMenuItemClicked);
        menu.AddItem("item1", "恢复药水", "10", true);
        menu.AddItem("item2", "解毒药水", "5", true);
        menu.AddItem("item3", "飞刀", "6", true);
        menu.AddItem("item4", "烟雾弹", "2", true);
        menu.AddItem("item5", "魔力药水", "12", true);
    }

    private void OnOrderMenuItemClicked(string parentId, string id)
    {
        switch (id)
        {
            case "attack":
                battleMenuManager.DestroyAllMenus();
                SelectEnemies();
                break;
            case "skill":
                CreateSkillMenu();
                break;
            case "defence":
                StartCoroutine(DoOrderDefence());
                break;
            case "item":
                CreateItemsMenu();
                break;
            case "escape":
                StartCoroutine(DoOrderEscape());
                break;
        }
    }

    private void OnSkillMenuItemClicked(string parentId, string id)
    {
        StartCoroutine(DoOrderSpell());
    }

    private void OnItemMenuItemClicked(string parentId, string id)
    {
        StartCoroutine(DoOrderUseItem());
    }

    private void SelectEnemies()
    {
        cursor.ClearPositions();
        for (int i = 0; i < enemies.Count; i++)
        {
            cursor.AddPosition(enemies[i].ctrl.transform);
        }

        cursor.SetActive(true);
    }

    public void OnCursorSubmit()
    {
        Transform cur = cursor.current;
        if (cur == null)
        {
            Debug.LogWarning("cursor point to null");
            return;
        }

        cursor.ClearPositions();

        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i].ctrl.transform == cur)
            {
                selectedTarget = enemies[i];
                break;
            }
        }
        StartCoroutine(DoOrderAttack());
    }

    private IEnumerator EnemyDie(EnemyObject enemy)
    {
        enemies.Remove(enemy);

        // 统计奖励掉落
        var reward = enemy.unit.GetComponent<RewardAttribute>();
        var items = reward.GetItems();
        for (int i = 0; i < items.Count; i++)
        {
            statistics.AddStrIntPair("battle/result", items[i].name, items[i].count);
        }

        statistics.ChangeIntegerValue("battle/result", "gold", enemy.origData.gold);
        statistics.ChangeIntegerValue("battle/result", "exp", enemy.origData.exp);

        yield return enemy.ctrl.Die();
        yield return new WaitForSeconds(1);
        Destroy(enemy.ctrl.gameObject);
    }

    private IEnumerator HeroDie(HeroObject hero)
    {
        heroes.Remove(hero);
        yield return hero.ctrl.Die();
    }

    private IEnumerator OrderEnemy(EnemyObject enemy)
    {
        Vector2 idlePos = enemy.ctrl.transform.position;
        instance.curEnemy = enemy;
        yield return enemy.ctrl.Move(instance.orderPoint.position, 0.5f);
        yield return enemy.ctrl.Attack();

        var selectedTarget = heroes[Random.Range(0, heroes.Count)];

        DamageInfo dmg = enemy.fight.Attack();
        int v = selectedTarget.fight.Damage(dmg);
        hud.PlayDamageText(-v, selectedTarget.ctrl.transform);
        selectedTarget.UpdateStatus();
        //Debug.Log($"{selectedTarget.unit.Name}: HP: {selectedTarget.fight.HP.HP}/{selectedTarget.fight.HP.MaxHP}");
        if (selectedTarget.fight.HP.HP == 0)
        {
            StartCoroutine(HeroDie(selectedTarget));
            if (heroes.Count == 0)
            {
                QuitBattle();
            }
        }
        else
        {
            StartCoroutine(selectedTarget.ctrl.Shake());
        }

        yield return enemy.ctrl.Move(idlePos, 0.5f, true);
    }

    private void BattleResult()
    {
        int gold = statistics.GetInteger("battle/result", "gold");
        int exp = statistics.GetInteger("battle/result", "exp");

        battleResult.SetBaseReward(gold, exp);

        for (int i = 0; i < heroes.Count; i++)
        {
            var hero = heroes[i];
            Destroy(hero.statusItem.gameObject);
            var status = battleResult.AddStatus(heroes[i].unit.Name);

            status.SetUnitName(hero.unit.Name);

            var expLevel = hero.unit.GetComponent<ExpLevelAttribute>();
            int leftExp = exp;
            int lastExp = expLevel.Exp - expLevel.BaseExp;
            int lastMaxExp = expLevel.MaxExp - expLevel.BaseExp;
            List<FiniteTimeAction> acts = new();
            for (; ; )
            {
                bool lu = expLevel.AddExpUntilLevelUp(leftExp, out leftExp);
                if (!lu)
                {
                    acts.Add(new ResultStatusItemAddExpAction(status, lastExp, lastMaxExp, expLevel.Exp - expLevel.BaseExp));
                    break;
                }
                status.SetLevelUp(true);

                acts.Add(new ResultStatusItemAddExpAction(status, lastExp, lastMaxExp, lastMaxExp));
                acts.Add(new ResultStatusItemLevelUp(status, expLevel.Level));

                lastExp = expLevel.Exp - expLevel.BaseExp;
                lastMaxExp = expLevel.MaxExp - expLevel.BaseExp;
            }

            var act = new Sequence(acts.ToArray());
            StartCoroutine(act.Play());
        }

        var rewards = statistics.GetPairsMerged("battle/result");
        for (int i = 0; i < rewards.Count; i++)
        {
            var item = itemLib[rewards[i].s];
            battleResult.AddRewardItem(item.sprite, item.title, rewards[i].i);
            PlayerItemData itemData = new()
            {
                metaID = item.id,
                title = item.title
            };
            for (int j = 0; j < rewards[i].i; j++)
            {
                inventory.AddItem(itemData);
            }
        }
        inventory.SyncInventoryObject();
        battleResult.Show(true);
    }

    public void OnNevigate(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed)
        {
            return;
        }

        Vector2 input = context.ReadValue<Vector2>();
        if (input == Vector2.zero)
        {
            return;
        }

        if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
        {
            cursor.Nevigate(input.x > 0f ? Vector2.right : Vector2.left);
        }
        else
        {
            cursor.Nevigate(input.y > 0f ? Vector2.up : Vector2.down);
        }
    }

    public void OnSubmit(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Performed)
        {
            return;
        }

        OnCursorSubmit();
    }
}
