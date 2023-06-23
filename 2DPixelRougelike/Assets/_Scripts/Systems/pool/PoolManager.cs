using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;

public class PoolManager : StaticInstance<PoolManager>
{

    [InlineEditor]
    [SerializeField] private PoolCollection _poolCollection;

    //particle pools
    //private Dictionary<string, ObjectPool<PoolParticle>> particlePoolsDictionary = new Dictionary<string, ObjectPool<PoolParticle>>();
    //private Dictionary<string, PoolParticle> particleDictionary = new Dictionary<string, PoolParticle>();

    //enemies
    //private Dictionary<string, ObjectPool<PoolEnemy>> enemyPoolsDictionary = new Dictionary<string, ObjectPool<PoolEnemy>>();
    //private Dictionary<string, PoolEnemy> enemyDictionary = new Dictionary<string, PoolEnemy>();

    
    //dmg txt pool
    //public ObjectPool<DamageText> dmgTextPool { get; private set; }
    
    protected override void Awake() 
    {
        base.Awake();

        //dmgTextPool = new ObjectPool<DamageText>(CreateDamageTextObject, OnTakeDamageTextFromPool, OnReturnTextDamageToPool);

        //-particles
        //initialize particle dictionary
        /*
        particleDictionary.Clear();
        foreach (ParticlePoolInstance pool in _poolCollection.particlePoolsInstance)
        {
            particleDictionary.Add(pool.poolName, pool.particlePrefab);
        }
        particlePoolsDictionary.Clear();
        //initialize particle pools dictionary
        foreach (ParticlePoolInstance pool in _poolCollection.particlePoolsInstance) {

            particlePoolsDictionary.Add(pool.poolName, new ObjectPool<PoolParticle>(CreateParticleForPool, OnTakeParticleFromPool, OnReturnParticleToPool));
        }
        */
        //-enemies
        //initialize enemies dictionary
        /*
        enemyDictionary.Clear();
        foreach (EnemyPoolInstance pool in _poolCollection.enemyPoolsInstance)
        {
            enemyDictionary.Add(pool.poolName, pool.enemyPrefab);
        }
        enemyPoolsDictionary.Clear();
        //initialize enemies pools dictionary
        foreach (EnemyPoolInstance pool in _poolCollection.enemyPoolsInstance)
        {
            enemyPoolsDictionary.Add(pool.poolName, new ObjectPool<PoolEnemy>(CreateEnemyForPool, OnTakeEnemyFromPool, OnReturnEnemyToPool));
        }
        */

    }
    /*
    #region enemies

    //really sneaky treak, honestly.
    // need to set this before asking for a enemy
    string enemyPoolName_SneakRefrence = "";
    public PoolEnemy SpawnEnemy(string _enemyName)
    {
        enemyPoolName_SneakRefrence = _enemyName;
        //Debug.Log(enemyPoolName_SneakRefrence + "   " + _enemyName);
        if (enemyPoolsDictionary.TryGetValue(enemyPoolName_SneakRefrence, out ObjectPool<PoolEnemy> _pool))
        {
            //enemyPoolName_SneakRefrence = "";
            return _pool.Get();
        }
        else
        {
            Debug.LogError("called for an unset enemy");
            //enemyPoolName_SneakRefrence = "";
            return null;
        }
    }

    PoolEnemy CreateEnemyForPool()
    {
        //if has pool and enemy
        if (enemyPoolsDictionary.TryGetValue(enemyPoolName_SneakRefrence, out ObjectPool<PoolEnemy> _pool) &&
           enemyDictionary.TryGetValue(enemyPoolName_SneakRefrence, out PoolEnemy _enemy))
        {
            PoolEnemy poolEnemy = Instantiate(_enemy, this.transform);
            poolEnemy.SetPool(_pool);
            return poolEnemy;
        }
        Debug.LogError("called for an unset enemy");
        return null;
    }
    private void OnTakeEnemyFromPool(PoolEnemy _poolEnemy)
    {
        _poolEnemy.OnReset();
        _poolEnemy.gameObject.SetActive(true);
    }
    private void OnReturnEnemyToPool(PoolEnemy _poolEnemy)
    {
        _poolEnemy.gameObject.SetActive(false);
    }

    #endregion
    */

    /*
    #region particles

    //really sneaky treak, honestly.
    // need to set this before asking for a particle
    string particlePoolName_SneakRefrence = "";
    public PoolParticle SpawnParticle(string _particleName)
    {
        particlePoolName_SneakRefrence = _particleName;
        //Debug.Log(particlePoolName_SneakRefrence + "   " + _particleName);
        if(particlePoolsDictionary.TryGetValue(particlePoolName_SneakRefrence, out ObjectPool<PoolParticle> _pool)){
            //particlePoolName_SneakRefrence = "";
            _pool.Clear();
            return _pool.Get();
        } else
        {
            Debug.LogError("called for an unset particle");
            //particlePoolName_SneakRefrence = "";
            return null;
        }
    }

    PoolParticle CreateParticleForPool()
    {
        //if has pool and particle
        if(particlePoolsDictionary.TryGetValue(particlePoolName_SneakRefrence, out ObjectPool<PoolParticle> _pool) &&
           particleDictionary.TryGetValue(particlePoolName_SneakRefrence, out PoolParticle _particle))
        {
            PoolParticle poolParticle = Instantiate(_particle,this.transform);
            poolParticle.SetPool(_pool);
            return poolParticle;
        }
        Debug.LogError("called for an unset particle");
        return null;
    }
    private void OnTakeParticleFromPool(PoolParticle _poolParticle)
    {
        _poolParticle.gameObject.SetActive(true);
        //restarts
        _poolParticle._particleSystem.Stop();
        _poolParticle._particleSystem.Play();
    }
    private void OnReturnParticleToPool(PoolParticle _poolParticle)
    {
        _poolParticle.gameObject.SetActive(false);
    }

    #endregion
    */

    /*
    #region dmg text

    DamageText CreateDamageTextObject()
    {
        var damageText = Instantiate(_poolCollection.DmgTextPrefab);
        damageText.SetPool(dmgTextPool);
        return damageText;
    }
    private void OnTakeDamageTextFromPool(DamageText _damageTex)
    {
        _damageTex.gameObject.SetActive(true);
    }
    private void OnReturnTextDamageToPool(DamageText _damageTex)
    {
        _damageTex.gameObject.SetActive(false);
    }
    #endregion
    */



}

[Serializable]
public struct ParticlePoolInstance
{
    //public ObjectPool<ParticleSystem> _particlePool { get; private set; }
    [field:SerializeField] public PoolParticle particlePrefab { get; private set; }
    [field: SerializeField] public string poolName { get; private set; }
}

[Serializable]
public struct EnemyPoolInstance
{
    //public ObjectPool<ParticleSystem> _particlePool { get; private set; }
    [field: SerializeField] public PoolEnemy enemyPrefab { get; private set; }
    [field: SerializeField] public string poolName { get; private set; }
}
