using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Cursor : MonoBehaviour
{
    public float moveDuration = 0.3f;

    private Coroutine curAct = null;
    private List<Transform> positions = new();
    private int curIndex = -1;

    public void MoveTo(Vector2 target, float duration)
    {
        if (curAct != null)
        {
            StopCoroutine(curAct);
        }
        curAct = StartCoroutine(MoveToAction(target, duration));
    }

    public void SetActive(bool value)
    {
        if (!value && curAct != null)
        {
            StopCoroutine(curAct);
            curAct = null;
        }
        gameObject.SetActive(value);
    }

    public void AddPosition(Transform position)
    {
        positions.Add(position);
        if (curIndex < 0)
        {
            curIndex = 0;
            transform.position = position.position;
        }
    }

    public void RemovePosition(Transform position)
    {
        int index = positions.IndexOf(position);
        if (index < 0)
        {
            return;
        }

        positions.RemoveAt(index);
        if (positions.Count == 0)
        {
            SetActive(false);
            curIndex = -1;
        }
    }

    public void ClearPositions()
    {
        positions.Clear();
        SetActive(false);
        curIndex = -1;
    }

    public Transform current
    {
        get
        {
            if (curIndex < 0)
            {
                return null;
            }
            return positions[curIndex];
        }
        set
        {
            int index = positions.IndexOf(value);
            if (index < 0)
            {
                return;
            }
            if (!gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }
            MoveTo(value.position, moveDuration);
        }
    }

    private IEnumerator MoveToAction(Vector2 target, float duration)
    {
        Vector3 start = transform.position;
        Vector3 end = new(target.x, target.y, start.z);
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            transform.position = Vector3.Lerp(start, end, t / duration);
            yield return null;
        }
        transform.position = end;
        curAct = null;
    }

    public void Nevigate(Vector2 value)
    {
        if (positions.Count == 0)
        {
            return;
        }

        if (value == Vector2.left || value == Vector2.up)
        {
            curIndex = (curIndex + positions.Count - 1) % positions.Count;
        }
        else
        {
            curIndex = (curIndex + 1) % positions.Count;
        }
        
        MoveTo(positions[curIndex].position, moveDuration);
    }
}
