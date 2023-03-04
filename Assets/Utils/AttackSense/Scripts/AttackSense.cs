using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class AttackSense : Singleton<AttackSense>
{
    private void Start()
    {
        if (Camera.main.GetComponent<CinemachineBrain>() != null)
        {
            useCinemachine = true;
            impulseSrc = GetComponent<CinemachineImpulseSource>();
        }
        else
        {
            useCinemachine = false;
        }
    }

    public static void HitPause(float duration, float scale)
    {
        if (instance.pauseCo != null)
        {
            instance.StopCoroutine(instance.pauseCo);
        }
        else
        {
            instance.origTimeScale = Time.timeScale;
        }

        instance.pauseCo = instance.StartCoroutine(instance.Pause(duration, scale));
    }

    public static void HitShake(float duration, float strength)
    {
        if (instance.useCinemachine)
        {
            instance.impulseSrc.GenerateImpulse(strength);
            return;
        }

        if (instance.shakeCo != null)
        {
            instance.StopCoroutine(instance.shakeCo);
        }
        else
        {
            instance.origCameraPos = Camera.main.transform.position;
        }

        instance.shakeCo = instance.StartCoroutine(instance.Shake(duration, strength));
    }

    private IEnumerator Pause(float duration, float scale)
    {
        Time.timeScale = scale;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = origTimeScale;
        pauseCo = null;
    }

    private IEnumerator Shake(float duration, float strength)
    {
        Transform camera = Camera.main.transform;
        while (duration > 0)
        {
            camera.position = Random.insideUnitSphere * strength + origCameraPos;
            duration -= Time.deltaTime;
            yield return null;
        }
        camera.position = origCameraPos;
        shakeCo = null;
    }

    private bool useCinemachine;
    private CinemachineImpulseSource impulseSrc;

    private Coroutine pauseCo;
    private float origTimeScale;

    private Coroutine shakeCo;
    private Vector3 origCameraPos;
}
