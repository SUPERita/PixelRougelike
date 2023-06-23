using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeanPoolManager : StaticInstance<LeanPoolManager>
{
    [SerializeField] private LeanGameObjectPool p1 = null;
    //[SerializeField] private GameObject prefab1 = null;
    public GameObject SpawnFromPool1()
    {
        //var e1 = Lean.Pool.LeanPool.Spawn(prefab1);
        var e = p1.Spawn(transform);
        return e.gameObject;
    }

}
