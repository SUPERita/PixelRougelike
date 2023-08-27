using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveManager : StaticInstance<WaveManager>
{
    
    [Header("Waves")]
    [SerializeField] private float waveDuration = 5f;
    [SerializeField] private float delayBetweenWaves = 5f;
    //[SerializeField] private float waveDuration = 5f;
    [SerializeField] private WaveWeightPair[] waves = null;
    [SerializeField] private EnemySpawner enemySpawner = null;
    public event Action<int> OnWaveStart;
    
    //[SerializeField] private GameObject[] spawnersGameObject;
    //private List<ISpawner> spawners;
    [SerializeField] private ObjectSpawner objectSpawner = null;

    [Header("Extra Waves")]
    [SerializeField] private WaveWeightPair extraWave;
    [SerializeField] private int extraWaveStartWeight = 400;
    [SerializeField, Tooltip("extraWaveStartWeight + extraWaveWeightPerLevel * level = wave weight ")] private int extraWaveWeightPerLevel = 30;

    [Header("Bosses")]
    [SerializeField] private BossWavePair[] bossWavePairs = null;

    [Header("Object")]
    [SerializeField] private ObjectWavePair[] objectWavePairs = null;

    [Header("testing (only works in editor)")]
    [SerializeField] private bool useStartWaveIndex = false;
    [SerializeField, ShowIf("@useStartWaveIndex"), Tooltip("starts from one"), Range(1,30)] private int startWave = 1;

    [SerializeField] private bool showColleclectionLoader = false;
    [SerializeField, ShowIf("@showColleclectionLoader")] private EnemyCollection[] collections = null;
    [Button, ShowIf("@showColleclectionLoader")]
    private void LoadCollection()
    {
        waves = new WaveWeightPair[collections.Length*3];
        for (int i = 0; i < collections.Length * 3; i++)
        {
            waves[i] = new WaveWeightPair(collections[i/3], 40+10*i, i+1);
        }
    }

   

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
    private bool GetObjectAtWave(int _waveIndex, out GameObject[] _prefabs)
    {
        foreach (var pair in objectWavePairs)
        {
            if (pair.spawnAtWave == _waveIndex)
            {
                _prefabs = pair.objectPrefabs;
                return true;
            }
        }
        _prefabs = null;
        return false;
    }

    private void Start()
    {
        //CacheSpawners();
        //SetSpawnersState(true);
        StartCoroutine(SpawnWaves());
        OnWaveStart += _notifySteam;
    }


    private void OnDisable()
    {
        StopCoroutine(SpawnWaves());
        OnWaveStart -= _notifySteam;
    }
    private IEnumerator SpawnWaves()
    {
        //regular waves
        int waveCounter = 0;
#if UNITY_EDITOR
        if (useStartWaveIndex) { waveCounter = startWave-1; }
#endif
        for (int _index = waveCounter; _index < waves.Length; _index++)
        {
            //increment
            waveCounter++;

            //wave notifcation
            MessageBoard.Instance.SpawnWaveHeader($"wave - {waveCounter}");
            OnWaveStart?.Invoke(waveCounter);//notify on wave start

            //spawn stuff
            SpawnEntitiesOfWave(waveCounter, waves[_index].enemyCollection, waves[_index].waveWeight);

            //wait for next wave
            yield return new WaitForSeconds(waveDuration + delayBetweenWaves);
        }

        //foreach (var wave in waves)
        //{
        //    //increment
        //    waveCounter++;

        //    //wave notifcation
        //    MessageBoard.Instance.SpawnWaveHeader($"wave - {waveCounter}");
        //    OnWaveStart?.Invoke(waveCounter);//notify on wave start

        //    //spawn stuff
        //    SpawnEntitiesOfWave(waveCounter, wave.enemyCollection, wave.waveWeight);

        //    //wait for next wave
        //    yield return new WaitForSeconds(waveDuration + delayBetweenWaves);

        //}

        //finish level
        MessageBoard.Instance.SpawnHeader("Room ANNIHILATED!!!");

        //if level isnt already unlocked, unlock new level
        if (!DoorManager.GetIsLevelUnlocked(int.Parse(SceneManager.GetActiveScene().name)+1))
        {
            MessageBoard.Instance.SpawnMessage("NEW ROOM UNLOCKED",2);
            DoorManager.CompleatedLevel(int.Parse(SceneManager.GetActiveScene().name));
        }

        SaveSystem.SaveBoolAtLocation(true, StartDoor.roomFinishedSavedLoc + SceneManager.GetActiveScene().name);

        //tell steamworks // +1 cause for the user levels start from 1
        SteamIntegration.UnlockAchievment($"ACH_ROOM{int.Parse(SceneManager.GetActiveScene().name) + 1}");/*should be a number*/

        yield return new WaitForSeconds(8);

        //extra waves
        int _extraWaveCounter = 1;
        for (int i = 0; i < 100; i++)
        {
            //wave notifcation
            MessageBoard.Instance.SpawnWaveHeader($"wave - {waveCounter}+{_extraWaveCounter}");
            OnWaveStart?.Invoke(waveCounter+ _extraWaveCounter);//notify on wave start

            //spawn enemies
            SpawnEntitiesOfWave(
                waveCounter + _extraWaveCounter,
                extraWave.enemyCollection,
                _extraWaveCounter * extraWaveWeightPerLevel + extraWaveStartWeight);

            //wait
            yield return new WaitForSeconds(waveDuration + delayBetweenWaves);

            //increment
            _extraWaveCounter++;
        }

        MessageBoard.Instance.SpawnHeader("Fuck It");
        MessageBoard.Instance.SpawnHeader("Not Coding Anymore",1.5f);
        MessageBoard.Instance.SpawnHeader("You Won!",3f);
    }

    public int currentWaveRefrence { get; private set; } = 0;
    private void SpawnEntitiesOfWave(int waveCounter, EnemyCollection enemyCollection, int waveWeight)
    {
        //try spawn boss
        if (GetBossAtWave(waveCounter, out GameObject _bossPrefab))
        {
            enemySpawner.SpawnBoss(_bossPrefab);
        }

        //try spawn object
        if (GetObjectAtWave(waveCounter, out GameObject[] _objectPrefab))
        {
            foreach (GameObject _g in _objectPrefab)
            {
                if(_g == null) continue;
                objectSpawner.SpawnObject(_g);
                //Debug.Log("broken, not spawning");
            }
        }


        //number of enemis to spawn
        float EnemyExtraPrecentage = 1 + PlayerStatsHolder.Instance.TryGetStat(StatType.EnemyAmount) / 100f;// 1.1, 1.05, 1.2, 1.5, 0.95! etc...
        if (EnemyExtraPrecentage < .1f) EnemyExtraPrecentage = .1f;// just a check
        int _num =(int) (waveWeight * EnemyExtraPrecentage);
        //Debug.Log(_num + " enemis spawned");

        //spawn enemies
        enemySpawner.SpawnEnemeis(
            enemyCollection.GetWaveOfWeight(_num),
            waveDuration);

        currentWaveRefrence = waveCounter;

        string saveloc = StartDoor.roomProgressSaveLocBase + SceneManager.GetActiveScene().name;
        if (waveCounter > SaveSystem.LoadIntFromLocation(saveloc))
        {
            SaveSystem.SaveIntAtLocation(waveCounter, saveloc);
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

    private void CacheSpawners()
    {
        spawners = new List<ISpawner>();
        foreach (var _spawner in spawnersGameObject)
        {
            spawners.Add(_spawner.GetComponent<ISpawner>());
        }
    }
    */

    private void _notifySteam(int _lvl)
    {
        if (_lvl == 10) { SteamIntegration.UnlockAchievment("ACH_WAVE10"); }
        if (_lvl == 20) { SteamIntegration.UnlockAchievment("ACH_WAVE20"); }
        if (_lvl == 30) { SteamIntegration.UnlockAchievment("ACH_WAVE30"); }
        if (_lvl == 40) { SteamIntegration.UnlockAchievment("ACH_WAVE40"); }
    }
}

[System.Serializable]
public struct WaveWeightPair
{
    [field: SerializeField, InlineEditor, HorizontalGroup("a"),LabelWidth(130)] public EnemyCollection enemyCollection { get; private set; }
    [field: SerializeField, HorizontalGroup("a"), LabelWidth(50)] public int waveWeight { get; private set; }
    //just cosmetic for me
    [field: SerializeField, HorizontalGroup("a"), LabelWidth(25)] public int waveIndex { get; private set; }

    public WaveWeightPair(EnemyCollection _enemyCollection, int _waveWeight, int _waveIndex)
    {
        enemyCollection = _enemyCollection;
        waveWeight = _waveWeight;
        waveIndex = _waveIndex;
    }
}

[System.Serializable]
public struct BossWavePair
{
    [field: SerializeField, HorizontalGroup("a"), LabelWidth(75)] public GameObject bossPrefab { get; private set; }
    [field: SerializeField, HorizontalGroup("a"), LabelWidth(75)] public int spawnAtWave { get; private set; }
}

[System.Serializable]
public struct ObjectWavePair
{
    [field: SerializeField,HorizontalGroup("a"), LabelWidth(250)] public GameObject[] objectPrefabs { get; private set; }
    [field: SerializeField, HorizontalGroup("a"), LabelWidth(75)] public int spawnAtWave { get; private set; }
}