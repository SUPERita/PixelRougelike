using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : StaticInstance<WaveManager>
{
    [SerializeField] private GameObject[] spawnersGameObject;
    private List<ISpawner> spawners;

    private void Start()
    {
        CacheSpawners();
        SetSpawnersState(true);
    }


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
}
