using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingHelper : MonoBehaviour
{
    public GameObject prefab;
    public int numberOfPoints = 10;
    public float radius = 5f;
    public float extraNum = 8f;


    [Button]
    private void SpawnPoints()
    {
        DestroyChildren(transform);
        float angleIncrement = 360f / numberOfPoints;

        for (int i = 0; i < numberOfPoints; i++)
        {
            float angle = i * angleIncrement;
            Vector3 spawnPosition = CalculatePositionOnCircle(angle);
            GameObject _g = Instantiate(prefab, spawnPosition, Quaternion.identity, transform);
            _g.transform.eulerAngles = angle * Vector3.forward;
            _g.transform.localScale = new Vector3(extraNum, 8*radius/numberOfPoints, 1f);
        }
    }
    private Vector3 CalculatePositionOnCircle(float angle)
    {
        float radianAngle = angle * Mathf.Deg2Rad;
        float x = radius * Mathf.Cos(radianAngle);
        float y = radius * Mathf.Sin(radianAngle);
        return transform.position + new Vector3(x, y, 0f);
    }

    [Button]
    private void DestroyChildren(Transform _t = null)
    {
        if (!_t) _t = transform;
        foreach (Transform item in _t.GetComponentsInChildren<Transform>())
        {
            if(transform == item) { continue; }
            DestroyImmediate(item.gameObject);
        }
    }
}
