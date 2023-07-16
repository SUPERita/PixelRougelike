using Lean.Pool;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class LeanPoolManager : StaticInstance<LeanPoolManager>
{
    //[SerializeField] private LeanGameObjectPool p1 = null;
    //[SerializeField] private GameObject prefab1 = null;
    [SerializeField] private List<PoolNamePair> poolNamePairs = null;
    private Dictionary<string, LeanGameObjectPool> poolDictionary;
    //private Dictionary<GameObject, LeanGameObjectPool> poolGameObjectDictionary;
    protected override void Awake()
    {
        base.Awake();

        //pool dictionary
        poolDictionary = new Dictionary<string, LeanGameObjectPool>();
        foreach (var _pair in poolNamePairs)
        {
            poolDictionary.Add(_pair.poolName, _pair.pool);
        }
        /*
        poolGameObjectDictionary = new Dictionary<GameObject, LeanGameObjectPool>();
        foreach (var _pair in poolNamePairs)
        {
            poolGameObjectDictionary.Add(_pair.poolGameObject, _pair.pool);
        }
        */
    }

    LeanGameObjectPool _tmp = null;
    public GameObject SpawnFromPool(string _poolName/*, Transform _parent/*?, to make it so i dont need to constantly set parent again*/)
    {
         _tmp = poolDictionary[_poolName];
        //var e1 = Lean.Pool.LeanPool.Spawn(prefab1);
        var e = _tmp.Spawn(transform);
        return e;


    }

    public void DespawnFromPool(GameObject _g,  float _inSeconds = 0)
    {
        //Debug.Log(_g.name);
        poolDictionary[_g.name].Despawn(_g, _inSeconds);
    }

    [Button]
    private void CacheChildPools()
    {
        poolNamePairs = new List<PoolNamePair>();
        foreach (LeanGameObjectPool _p in GetComponentsInChildren<LeanGameObjectPool>())
        {
            poolNamePairs.Add(new PoolNamePair(_p, _p.Prefab.name, _p.Prefab));
            _p.gameObject.name = _p.Prefab.name;
        }
    }

}

[Serializable]
public struct PoolNamePair
{
    [field: SerializeField] public LeanGameObjectPool pool { get; private set; }
    [field: SerializeField] public string poolName { get; private set; }
    [field: SerializeField, Required, InlineEditor(InlineEditorModes.GUIAndHeader)] public GameObject poolGameObject { get; private set; }

    public PoolNamePair( LeanGameObjectPool _pool, string _poolName, GameObject _poolGameObject)
    {
        this.pool = _pool;
        this.poolName = _poolName;
        this.poolGameObject = _poolGameObject;
    }

}
