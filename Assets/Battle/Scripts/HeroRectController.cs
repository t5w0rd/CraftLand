using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>英雄入场数据，包含英雄对象和入场动画协程</para>
/// </summary>
public struct HeroEntering
{
    public HeroEntering(UnitController hero, Coroutine moving)
    {
        this.hero = hero;
        this.moving = moving;
    }

    public readonly UnitController hero;
    public readonly Coroutine moving;

    public bool isEmpty => hero == null || moving == null;
    public static HeroEntering empty => emtpyHeroEntering;

    private static HeroEntering emtpyHeroEntering = new HeroEntering(null, null);
}

/// <summary>
/// <para>控制英雄在战场中的活动区域移动</para>
/// </summary>
public class HeroRectController : MonoBehaviour
{
    public Transform enterPoint;
    public Transform idlePoint;

    private UnitController hero;

    /// <summary>
    /// <para>在战场上创建英雄，并使其入场</para>
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="duration"></param>
    /// <returns></returns>
    public HeroEntering CreateHeroAndEnter(GameObject prefab, float duration)
    {
        hero = Instantiate(prefab, transform).GetComponent<UnitController>();
        hero.transform.position = enterPoint.position;

        return new HeroEntering(hero, StartCoroutine(hero.Move(idlePoint.position, duration)));
    }

    public IEnumerator MoveBackwardsToIdle(float duration)
    {
        yield return hero.Move(idlePoint.position, duration, true);
    }
}
