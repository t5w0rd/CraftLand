using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesRectController : MonoBehaviour
{
    public Transform rectPrefab;

    //private List<Transform> rects = new();

    private Transform curRect = null;
    private int curRectPlaceLeft = 0;
    private int rectLeft = 3;
    private Transform curLayout = null;

    public UnitController CreateEnemy(GameObject prefab)
    {
        if (curRectPlaceLeft <= 0)
        {
            if (rectLeft == 0)
            {
                return null;
            }
            rectLeft--;
            curRectPlaceLeft = Random.Range(2, 4);
            curRect = Instantiate(rectPrefab, transform);
            string layoutName = $"Layout{Random.Range(0, 3)}";
            curLayout = curRect.Find(layoutName);
        }

        curRectPlaceLeft--;

        var enemy = Instantiate(prefab, curRect);
        string pointName = $"Point{curRectPlaceLeft}";
        Transform point = curLayout.Find(pointName);
        enemy.transform.position = point.position;

        UnitController ctrl = enemy.GetComponent<UnitController>();
        return ctrl;
    }
}
