using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using TGamePlay.TBattle;
using UnityEngine;

public struct UnitInfoTD
{
    public UnitMeta meta;
    public Unit unit;
    public UnitControllerTD ctrl;
    public StatusBar hpBar;

    public static UnitInfoTD invalid = new();

    public bool valid => unit != null;

    public void InitWithPlayerUnitData(PlayerUnitData data)
    {
        var hp = unit.Fight.HP;
        hp.MaxHPRaw = data.hp.maxHP;
        hp.HP = data.hp.hp;
        unit.Fight.ATKRaw = data.atk;
        unit.Fight.DEFRaw = data.def;
        var expLevel = unit.AddComponent(new ExpLevelAttribute(ArchiveManager.MaxExpTable));
        expLevel.AddExp(data.expLevel.exp);
        expLevel.MaxLevel = data.expLevel.maxLevel;
        expLevel.Level = data.expLevel.level;
    }
}

public class BattleManagerTD : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var u = CreateUnit("Unit", "不是玩家", playerCtrl);
        //u.ctrl.EnablePlayerInput(true);
        //cmCamera.Follow = u.ctrl.transform;
        //u.InitWithPlayerUnitData()
        playerCtrl.EquipRight(ironSword);
        playerCtrl.attackMethod = new SwordAttackMethod();


        u = CreateUnit("Unit", "我有防御技能", enemyCtrl);
        var skills = u.unit.AddComponent(new SkillAttribute());
        var skill = new TestOnDamageSkill(TGamePlay.TBattle.Object.GenLocalID(), "测试防御技能", new Coefficient(-90));
        skills.AddSkill(skill);

        u = CreateUnit("Unit", "控制测试");
        u.ctrl.transform.position = Random.insideUnitSphere * 5;

        StartBattle(ArchiveManager.player);
    }

    private void StartBattle(PlayerObject player)
    {
        this.player = player;

        MockCreateHero();
    }

    private void MockCreateHero()
    {
        // CreateHero
        var data = player.heroes[0];
        var u = CreateUnit(data.metaID, data.title);
        u.InitWithPlayerUnitData(data);

        u.ctrl.EquipRight(ironSword);
        u.ctrl.attackMethod = new SwordAttackMethod();
        u.ctrl.EnablePlayerInput(true);
        cmCamera.Follow = u.ctrl.transform;

        var skills = u.unit.AddComponent(new SkillAttribute());
        Skill skill = new TestOnAttackSkill(TGamePlay.TBattle.Object.GenLocalID(), "测试攻击技能", new Coefficient(100), 0.3f);
        skills.AddSkill(skill);
    }

    private UnitInfoTD CreateUnit(string metaID, string name, UnitControllerTD ctrl)
    {
        UnitMeta meta = unitLib[metaID];
        if (meta == null)
        {
            Debug.LogWarning($"UnitMeta({metaID}) not found");
            return UnitInfoTD.invalid;
        }

        Unit unit = new(TGamePlay.TBattle.Object.GenLocalID(), name);
        var hp = unit.AddComponent(new HPAttribute(meta.maxHP));
        var fight = unit.AddComponent(new FightAttribute());
        fight.ATKRaw = meta.atk;
        fight.DEFRaw = meta.def;

        ctrl.name = $"Unit_{unit.Name}_{unit.ID}";
        ctrl.unit = unit;
        ctrl.OnDamage += OnUnitDamage;

        StatusBar hpBar = statusBarMgr.CreateStatusBar(ctrl.hpBarRect);

        UnitInfoTD info = new()
        {
            meta = meta,
            unit = unit,
            ctrl = ctrl,
            hpBar = hpBar,
        };
        unitInfos.Add(unit.ID, info);
        return info;
    }

    public UnitInfoTD CreateUnit(string metaID, string name)
    {
        UnitMeta meta = unitLib[metaID];
        if (meta == null)
        {
            Debug.LogWarning($"UnitMeta({metaID}) not found");
            return UnitInfoTD.invalid;
        }

        UnitControllerTD ctrl = Instantiate(meta.prefab, transform).GetComponent<UnitControllerTD>();

        return CreateUnit(metaID, name, ctrl);
    }

    public UnitInfoTD GetUnit(int id)
    {
        return unitInfos.TryGetValue(id, out UnitInfoTD value) ? value : UnitInfoTD.invalid;
    }

    private void OnUnitDamage(UnitControllerTD ctrl, WeaponTD weapon)  // TODO: Weapon换成接口，用于支持间接伤害，如弓箭的箭矢、法师的法术弹
    {
        UnitInfoTD target = GetUnit(ctrl.unit.ID);
        UnitInfoTD source = GetUnit(weapon.unitID);
        if (!target.valid || !source.valid)
        {
            return;
        }

        int damge = target.unit.Fight.Damage(weapon.damageInfo);

        StatusBar bar = target.hpBar;

        bar.SetValue(target.unit.Fight.HP.HP, target.unit.Fight.HP.MaxHP);
        hud.PlayDamageText(-damge, ctrl.transform);

        AttackSense.HitPause(hitSenseDuration, hitSenseTimeScale);
        AttackSense.HitShake(hitSenseDuration, hitSenseShakeStrength);
    }

    [SerializeField] private BattleHUD hud;
    [SerializeField] private StatusBarManager statusBarMgr;
    [SerializeField] private UnitLibrary unitLib;
    [SerializeField] private ItemLibrary itemLib;
    [SerializeField] private CinemachineVirtualCamera cmCamera;

    [SerializeField] private UnitControllerTD playerCtrl;  // for test
    [SerializeField] private UnitControllerTD enemyCtrl;  // for test
    [SerializeField] private SwordTD ironSword;  // for test

    [SerializeField] private float hitSenseDuration = 0.1f;
    [SerializeField] private float hitSenseTimeScale = 0f;
    [SerializeField] private float hitSenseShakeStrength = 0.1f;

    private PlayerObject player;
    private readonly Dictionary<int, UnitInfoTD> unitInfos = new();
}
