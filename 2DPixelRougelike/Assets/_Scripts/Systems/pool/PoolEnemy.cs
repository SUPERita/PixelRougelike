using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolEnemy : MonoBehaviour, IResetable
{
    [field: SerializeField] public Health _health { get; private set; }
    [field: SerializeField] public Transform _enemyTransform { get; private set; }
    private ObjectPool<PoolEnemy> pool;
    [SerializeField] private Enemy enm;
    //pool
    public void SetPool(ObjectPool<PoolEnemy> _pool) => pool = _pool;
    public void ReleaseToPool()
    {
        pool.Release(this);

    }

    public void OnReset()
    {
        //GetComponent<IResetable>().OnReset(); // never do this it may be recursive
        enm.OnReset();
        _health.ResetHealth();
    }

    private void Awake()
    {
        _enemyTransform = transform;
        if (!enm) Debug.LogError("you no enemy on: " +  _enemyTransform.name + "'s poolEnemy comp");
    }
}
