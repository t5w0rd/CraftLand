using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultRewardItem : MonoBehaviour
{
    public Image uiIcon;
    public Text uiName;
    public Text uiCount;

    public void SetItem(Sprite icon, string name, int count)
    {
        uiIcon.sprite = icon;
        uiName.text = name;
        uiCount.text = $"{count}";
    }
}
