using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using UnityEngine.UIElements;
using DG.Tweening;

public class EnemySpawner : MonoBehaviour, ISpawner
{
    //[SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float reload = 1f;
    [SerializeField] private float spawnRange = 64f;
    [SerializeField] private GameObject spawnIndicator;

    private Transform _transform;
    private bool working = false;
    private void Start()
    {
        _transform = transform;
    }

    private void SpawnEnemy()
    {
        if (!working) { return; }
        Vector2 spawnPosition = (Vector2)UnityEngine.Random.insideUnitSphere * spawnRange;
        Transform _indc = Instantiate(spawnIndicator, spawnPosition, Quaternion.identity).transform;

        _indc.localScale = Vector3.zero;
        _indc.DOScale(1, 1f)
        .OnComplete(() =>
        {
            //spawn enemy
            PoolEnemy _t = UnityEngine.Random.Range(0, 2) == 1 ? PoolManager.Instance.SpawnEnemy("box2") : PoolManager.Instance.SpawnEnemy("box");
            _t._enemyTransform.SetParent(_transform);
            _t._enemyTransform.localPosition = spawnPosition;

            //destory indicator
            Destroy(_indc.gameObject);
        });
        //UnityEngine.Random.insideUnitSphere//can give a vector3 if not careful

        //Instantiate(enemyPrefab).transform.SetParent(_transform);
        //Instantiate(enemyPrefab, _transform.position, _transform.rotation, _transform);
        Invoke(nameof(SpawnEnemy), reload);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnRange);
    }

    public void StopSpawning()
    {
        working = false;
    }
    public void StartSpawning()
    {
        working = true;
        SpawnEnemy();
    }
}

public interface ISpawner
{
    void StopSpawning();
    void StartSpawning();
}
