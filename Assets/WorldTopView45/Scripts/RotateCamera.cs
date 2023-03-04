using System.Collections;
using System.Collections.Generic;
using TGamePlay.TAction;
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    public float rotateDuration = 0.1f;

    private Transform player;
    private bool rotating = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.rotation = player.transform.rotation;
        Rotate();
    }

    private void Rotate()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !rotating)
        {
            var act = new Ease(new RotateBy(rotateDuration, transform, -45), Ease.FuncExpoInOut);
            StartCoroutine(act.Play());
            //StartCoroutine(RotateArround(-45, rotateDuration));
            return;
        }

        if (Input.GetKeyDown(KeyCode.E) && !rotating)
        {
            var act = new Ease(new RotateBy(rotateDuration, transform, 45), Ease.FuncExpoInOut);
            StartCoroutine(act.Play());
            //StartCoroutine(RotateArround(45, rotateDuration));
            return;
        }
    }

    private IEnumerator RotateArround(float angle, float duration)
    {
        rotating = true;
        Vector3 end = new(0, 0, angle);
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            var r = Vector3.Lerp(Vector3.zero, end, t / duration);
            Debug.Log(r.z);
            transform.Rotate(r);
            yield return null;
        }
            
        rotating = false;
    }
}
