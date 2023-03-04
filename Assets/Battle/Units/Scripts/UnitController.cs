using System.Collections;
using System.Collections.Generic;
using TGamePlay.TAction;
using UnityEngine;

/// <summary>
/// <para>单位控制器</para>
/// </summary>
public class UnitController : MonoBehaviour
{
    public string idleAnimation = "Idle";
    public string moveAnimation = "Move";
    public string dieAnimation = "Die";
    public string[] attackAnimations = { "Act1" };
    public string spellAnimation;
    public string powerUpAnimation;

    protected Animator anim;
    protected SpriteRenderer sr;

    void Awake()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// <para>等待单位动画进入空闲状态</para>
    /// </summary>
    /// <returns></returns>
    public IEnumerator WaitForIdle()
    {
        for (var state = anim.GetCurrentAnimatorStateInfo(0);
            !state.IsName(idleAnimation);
            state = anim.GetCurrentAnimatorStateInfo(0))
        {
            yield return null;
        }
    }

    public bool flipX
    {
        get => sr.flipX;
        set => sr.flipX = value;
    }

    /// <summary>
    /// <para>单位从当前点移动到目标点，并播放移动动画</para>
    /// </summary>
    /// <param name="target">目标点</param>
    /// <param name="duration">耗用时间</param>
    /// <returns></returns>
    public IEnumerator Move(Vector2 target, float duration, bool backwards = false)
    {
        bool flipX = sr.flipX;
        if (target.x > transform.position.x)
        {
            flipX = false;
        }
        else if (target.x < transform.position.x)
        {
            flipX = true;
        }
        if (backwards)
        {
            sr.flipX = !flipX;
            anim.SetFloat("Speed", -1.0f);
        }
        else
        {
            sr.flipX = flipX;
            anim.SetFloat("Speed", 1.0f);
        }
        anim.Play(moveAnimation);
        yield return MoveTo(target, duration);
    }

    /// <summary>
    /// <para>播放死亡动画</para>
    /// </summary>
    /// <returns></returns>
    public IEnumerator Die()
    {
        anim.Play(dieAnimation);
        yield return null;  // 当前帧动画还未播放需要等待一帧

        for (var state = anim.GetCurrentAnimatorStateInfo(0);
            state.normalizedTime < 1.0f;
            state = anim.GetCurrentAnimatorStateInfo(0))
        {
            yield return null;
        }
    }

    /// <summary>
    /// <para>播放随机攻击动画</para>
    /// </summary>
    /// <returns></returns>
    public IEnumerator Attack()
    {
        int index = Random.Range(0, attackAnimations.Length);
        yield return PlayAnimation(attackAnimations[index]);
    }

    /// <summary>
    /// <para>播放施法动画</para>
    /// </summary>
    /// <returns></returns>
    public IEnumerator Spell()
    {
        yield return PlayAnimation(spellAnimation);
    }

    /// <summary>
    /// <para>播放能力提升动画</para>
    /// </summary>
    /// <returns></returns>
    public IEnumerator PowerUp()
    {
        yield return PlayAnimation(powerUpAnimation);
    }

    /// <summary>
    /// <para>播放指定动画</para>
    /// </summary>
    /// <param name="name">动画名称</param>
    /// <returns></returns>
    public IEnumerator PlayAnimation(string name, bool waitIdle = true)
    {
        anim.Play(name);
        yield return null;  // 当前帧动画还未播放需要等待一帧
        if (waitIdle)
        {
            yield return WaitForIdle();
        }
    }

    private IEnumerator MoveTo(Vector2 target, float duration)
    {
        Vector3 start = transform.position;
        Vector3 end = target;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            anim.SetBool("Moving", true);
            transform.position = Vector3.Lerp(start, end, t / duration);
            yield return null;
        }
        transform.position = end;
        anim.SetBool("Moving", false);
    }

    public IEnumerator Shake()
    {
        Vector2 orig = transform.position;
        int sign = flipX ? 1 : -1;

        var act = new Ease(new MoveBy(0.3f, transform, new Vector3(sign * 0.5f, 0)), Ease.FuncExpoOut);
        yield return act.Play();
        yield return Move(orig, 0.2f);
    }
}
