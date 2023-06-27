using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : StaticInstance<WaveManager>
{
    [Header("Waves")]
    [SerializeField] private float waveDuration = 5f;
    [SerializeField] private float delayBetweenWaves = 5f;
    //[SerializeField] private float waveDuration = 5f;
    [SerializeField] private WaveWeightPair[] waves = null;
    [SerializeField] private GameObject[] spawnersGameObject;
    private List<ISpawner> spawners;
    [SerializeField] private EnemySpawner enemySpawner = null;
    public event Action<int> OnWaveStart;

    [Header("Bosses")]
    [SerializeField] private BossWavePair[] bossWavePairs = null;
    private bool GetBossAtWave(int _waveIndex, out GameObject _prefab)
    {
        foreach (var pair in bossWavePairs) 
        {
            if (pair.spawnAtWave == _waveIndex) { 
                _prefab = pair.bossPrefab; 
                return true;
            }
        }
        _prefab = null;
        return false;
    }

    private void Start()
    {
        CacheSpawners();
        //SetSpawnersState(true);
        StartCoroutine(SpawnWaves());
    }
    private void OnDisable()
    {
        StopCoroutine(SpawnWaves());
    }
    private IEnumerator SpawnWaves()
    {
        int waveCounter = 1;
        foreach (var wave in waves) {
            //wave notifcation
            MessageBoard.Instance.SpawnHeader($"wave - {waveCounter}");
            OnWaveStart?.Invoke(waveCounter);//notify on wave start

            //try spawn boss
            if(GetBossAtWave(waveCounter, out GameObject _prefab))
            {
                enemySpawner.SpawnBoss(_prefab);
            }

            //spawn enemies
            enemySpawner.SpawnEnemeis(
                wave.enemyCollection.GetWaveOfWeight(wave.waveWeight),
                waveDuration);
            yield return new WaitForSeconds(waveDuration+delayBetweenWaves);

            //increment
            waveCounter++;
        }
    }

    /*
    [Button]
    private void SetSpawnersState(bool _state)
    {
        if(_state)
        {
            foreach(var spawner in spawners)
            {
                spawner.StartSpawning();
            }
        } 
        else
        {
            foreach (var spawner in spawners)
            {
                spawner.StopSpawning();
            }
        }
    }
    */

    private void CacheSpawners()
    {
        spawners = new List<ISpawner>();
        foreach (var _spawner in spawnersGameObject)
        {
            spawners.Add(_spawner.GetComponent<ISpawner>());
        }
    }
}

[System.Serializable]
public struct WaveWeightPair
{
    [field: SerializeField, InlineEditor] public EnemyCollection enemyCollection { get; private set; }
    [field: SerializeField] public int waveWeight { get; private set; }
}

[System.Serializable]
public struct BossWavePair
{
    [field: SerializeField] public GameObject bossPrefab { get; private set; }
    [field: SerializeField] public int spawnAtWave { get; private set; }
}