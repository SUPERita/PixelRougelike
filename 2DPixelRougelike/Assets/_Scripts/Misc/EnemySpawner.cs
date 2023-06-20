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
       // Invoke(nameof(SpawnEnemy), 1); // dont spawn stuff on start 
    }
     
    private void SpawnEnemy()
    {
        if (spawn)
        {

            PoolEnemy _t = UnityEngine.Random.Range(0, 2) == 1 ? PoolManager.Instance.SpawnEnemy("box2") : PoolManager.Instance.SpawnEnemy("box");
            _t._enemyTransform.SetParent(_transform);
            _t._enemyTransform.localPosition = Vector3.zero;

            //Instantiate(enemyPrefab).transform.SetParent(_transform);
            //Instantiate(enemyPrefab, _transform.position, _transform.rotation, _transform);
        }
        Invoke(nameof(SpawnEnemy), reload);
    }
}
