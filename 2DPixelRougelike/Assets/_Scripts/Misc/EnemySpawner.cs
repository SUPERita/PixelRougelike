using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using UnityEngine.UIElements;

public class EnemySpawner : MonoBehaviour
{
    //[SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float reload = 1f;
    [SerializeField] private bool spawn = true;

    private Transform _transform;
    private void Start()
    {
        _transform = transform;
        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        if (spawn)
        {
            PoolEnemy _t = PoolManager.Instance.SpawnEnemy("box");
            _t._enemyTransform.SetParent(_transform);
            _t._enemyTransform.SetLocalPositionAndRotation(_transform.position, _transform.rotation);
            //Instantiate(enemyPrefab, _transform.position, _transform.rotation, _transform);
        }
        Invoke(nameof(SpawnEnemy), reload);
    }
}
