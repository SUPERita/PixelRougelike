using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float reload = 1f;
    [SerializeField] private bool spawn = true;
    private void Start()
    {
        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        if (spawn)
        {
            Instantiate(enemyPrefab, transform.position, transform.rotation, transform);
        }
        Invoke(nameof(SpawnEnemy), reload);
    }
}
