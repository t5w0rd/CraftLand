using System.Collections;
using System.Collections.Generic;
using TGamePlay.TAction;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    public Text uiValue;
    public Animator anim;

    private IEnumerator PlayFloatUp(int value, float deltaY) {
        uiValue.text = $"{value}";

        Action act = new Ease(new MoveBy(0.5f, transform, new Vector3(0, deltaY, 0)), Ease.FuncExpoInOut);
        yield return act.Play();

        Destroy(gameObject);
    }

    public void FloatUp(int value, float deltaY)
    {
        StartCoroutine(PlayFloatUp(value, deltaY));
    }
}
