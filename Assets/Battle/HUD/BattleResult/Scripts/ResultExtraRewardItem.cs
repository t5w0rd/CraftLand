using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultExtraRewardItem : MonoBehaviour
{
    public Text uiRewardType;

    public void SetRewardType(string text)
    {
        uiRewardType.text = text;
    }
}
