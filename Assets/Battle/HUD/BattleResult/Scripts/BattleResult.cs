using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleResult : MonoBehaviour
{
    public Text uiGold;
    public Text uiExp;
    public Text uiExtraGold;
    public Text uiExtraExp;

    public Transform uiExtraReward;
    public ResultExtraRewardItem extraRewardItemPrefab;

    public Transform uiStatus;
    public ResultStatusItem statusItemPrefab;

    public GameObject uiRewardLabel;
    public ScrollRect uiRewardList;
    public ResultRewardItem rewardItemPrefab;

    private CanvasGroup cg;

    void Awake()
    {
        cg = GetComponent<CanvasGroup>();
    }

    void Start()
    {
        Show(false);
        uiExtraGold.gameObject.SetActive(false);
        uiExtraExp.gameObject.SetActive(false);
        uiRewardLabel.SetActive(false);
    }

    public void Show(bool value)
    {
        if (value)
        {
            cg.alpha = 1;
            cg.interactable = true;
            cg.blocksRaycasts = true;
        }
        else
        {
            cg.alpha = 0;
            cg.interactable = false;
            cg.blocksRaycasts = false;
        }
    }

    public void SetBaseReward(int gold, int exp)
    {
        uiGold.text = $"{gold}";
        uiExp.text = $"{exp}";
    }

    public void SetExtraReward(int gold, int exp)
    {
        if (gold > 0)
        {
            uiExtraGold.text = $"+{gold}";
            uiExtraGold.gameObject.SetActive(true);
        }

        if (exp > 0)
        {
            uiExtraExp.text = $"+{exp}";
            uiExtraExp.gameObject.SetActive(true);
        }
    }

    public ResultExtraRewardItem AddExtraRewardType(string name)
    {
        ResultExtraRewardItem item = Instantiate(extraRewardItemPrefab, uiExtraReward);
        item.SetRewardType(name);
        return item;
    }

    public ResultStatusItem AddStatus(string name)
    {
        ResultStatusItem item = Instantiate(statusItemPrefab, uiStatus);
        item.SetUnitName(name);
        return item;
    }

    public ResultRewardItem AddRewardItem(Sprite icon, string name, int count)
    {
        if (!uiRewardLabel.activeSelf)
        {
            uiRewardLabel.SetActive(true);
        }
        ResultRewardItem item = Instantiate(rewardItemPrefab, uiRewardList.content);
        item.SetItem(icon, name, count);
        return item;
    }
}
