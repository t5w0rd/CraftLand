using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// <para>状态项，显示单位的名字、血量、能量等信息</para>
/// </summary>
public class StatusItem : MonoBehaviour
{
    public Text uiUnitName;
    public Text uiHPValue;
    public Slider uiHPBar;
    public Text uiSPValue;
    public Slider uiSPBar;

    public Animator anim;

    public void SetUnitName(string name)
    {
        uiUnitName.text = name;
    }

    public void SetHP(int hp, int maxHP)
    {
        uiHPValue.text = $"{hp} / {maxHP}";
        uiHPBar.value = hp * 1.0f / Mathf.Max(maxHP, 0.1f);
    }

    public void SetSP(int sp, int maxSP)
    {
        uiSPValue.text = $"{sp} / {maxSP}";
        uiSPBar.value = sp * 1.0f / Mathf.Max(maxSP, 0.1f);
    }

    public bool focus
    {
        get => anim.GetBool("Focus");
        set => anim.SetBool("Focus", value);
    }
}
