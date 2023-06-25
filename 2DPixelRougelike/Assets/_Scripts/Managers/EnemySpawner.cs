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
    //[SerializeField] private float reload = 1f;
    [SerializeField] private float spawnRange = 64f;
    [SerializeField] private float spawnRangeFromPlayer = 8f;
    [SerializeField] private GameObject spawnIndicator;
    [SerializeField] private Transform player;

    private Transform _transform;
    //private bool working = false;

    //lifecycle
    private void Start()
    {
        _transform = transform;
    }
    private void OnDisable()
    {
        StopCoroutine(nameof(SpawnEnemiesWithDelay));
    }

    //spawning
    private void SpawnEnemy(string _enemyName)
    {
        //if (!working) { return; }

        //choose spawn point
        Vector2 spawnPosition =
            (Vector2)player.position +
            (Vector2)UnityEngine.Random.insideUnitSphere * spawnRangeFromPlayer;
        //while off the arena, rechoose
        while((spawnPosition- (Vector2)transform.position).sqrMagnitude > spawnRange* spawnRange)
        {
            spawnPosition =
            (Vector2)player.position +
            (Vector2)UnityEngine.Random.insideUnitSphere * spawnRangeFromPlayer;
        }

        //spawn indc
        Transform _indc = Instantiate(spawnIndicator, spawnPosition, Quaternion.identity).transform;

        _indc.localScale = Vector3.zero;
        _indc.DOScale(1, 1f)
        .OnComplete(() =>
        {
            //spawn _enemy
            Transform _t = LeanPoolManager.Instance.SpawnFromPool(_enemyName).transform;//UnityEngine.Random.Range(0, 2) == 1 ? "dasher1":"follow1").transform;//PoolEnemy _t = UnityEngine.Random.Range(0, 2) == 1 ? PoolManager.Instance.SpawnEnemy("box2") : PoolManager.Instance.SpawnEnemy("box");
            _t.SetParent(_transform);
            _t.localPosition = spawnPosition;

            //destory indicator
            Destroy(_indc.gameObject);
        });
        //UnityEngine.Random.insideUnitSphere//can give a vector3 if not careful

        //Instantiate(enemyPrefab).transform.SetParent(_transform);
        //Instantiate(enemyPrefab, _transform.position, _transform.rotation, _transform);
        //Invoke(nameof(SpawnEnemy), reload);
    }
    public void SpawnEnemeis(string[] _enemies, float _spawnOverTimeSeconds)
    {
        StartCoroutine(SpawnEnemiesWithDelay(_enemies, _spawnOverTimeSeconds));
    }
    private IEnumerator SpawnEnemiesWithDelay(string[] _enemies, float _spawnOverTimeSeconds)
    {
        
        foreach(string _enemy in _enemies)
        {
            SpawnEnemy(_enemy);
            yield return new WaitForSeconds(_spawnOverTimeSeconds/_enemies.Length);
        }
    }


    //utils
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, spawnRange);
        Gizmos.DrawWireSphere(player.position, spawnRangeFromPlayer);
        Gizmos.DrawLine(player.position, transform.position);
    }
    public void StopSpawning()
    {
        //working = false;
    }
    public void StartSpawning()
    {
        //working = true;
        //SpawnEnemy();
    }
}

public interface ISpawner
{
    void StopSpawning();
    void StartSpawning();
}
