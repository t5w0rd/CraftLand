using System.Collections;
using System.Collections.Generic;
using TGamePlay.TAction;
using UnityEngine;
using UnityEngine.UI;


public class ResultStatusItemAddExpAction : ActionInterval
{
    private readonly ResultStatusItem status;
    private readonly int startExp;
    private readonly int endExp;
    private readonly int maxExp;

    public ResultStatusItemAddExpAction(ResultStatusItem status, int exp, int maxExp, int toExp) :
        base((toExp - exp) * 1.0f / maxExp * 0.5f)
    {
        this.status = status;
        startExp = exp;
        endExp = toExp;
        this.maxExp = maxExp;
    }

    public override void Start()
    {
        base.Start();
        status.SetExp(startExp, maxExp);
        //Debug.Log($"SetExp({startExp}, {maxExp}){Duration}");
    }

    public override void Update(float time)
    {
        int exp = (int)(startExp + (endExp - startExp) * time);
        status.SetExp(exp, maxExp);
        //Debug.Log($"SetExp({exp}, {maxExp}){Duration}");
    }
}

public class ResultStatusItemLevelUp : CallFunc
{
    private readonly ResultStatusItem status;
    private readonly int level;

    public ResultStatusItemLevelUp(ResultStatusItem status, int level) :
        base(null)
    {
        Function = SetLevel;
        this.status = status;
        this.level = level;
    }

    protected void SetLevel()
    {
        status.SetLevel(level);
    }
}

public class ResultStatusItem : MonoBehaviour
{
    public Text uiLevelUp;
    public Text uiName;
    public Text uiLevel;
    public Text uiExp;
    public Slider uiExpBar;

    void Start()
    {
        uiLevelUp.gameObject.SetActive(false);
    }

    public void SetUnitName(string value)
    {
        uiName.text = value;
    }

    public void SetLevel(int value)
    {
        uiLevel.text = $"LV.{value}";
    }

    public void SetExp(int exp, int maxExp)
    {
        uiExp.text = $"{exp}/{maxExp}";
        uiExpBar.value = exp * 1.0f / maxExp;
    }

    public void SetLevelUp(bool show)
    {
        uiLevelUp.gameObject.SetActive(show);
    }
}
