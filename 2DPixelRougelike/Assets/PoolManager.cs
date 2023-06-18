using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : Singleton<PoolManager>
{

    [SerializeField] private DamageText DmgTextPrefab = null;

    //particle pools
    [SerializeField] private ParticlePoolInstance[] particlePoolsInstance;
    private Dictionary<string, ObjectPool<PoolParticle>> particlePoolsDictionary = new Dictionary<string, ObjectPool<PoolParticle>>();
    private Dictionary<string, PoolParticle> particleDictionary = new Dictionary<string, PoolParticle>();

    //dmg txt pool
    public ObjectPool<DamageText> dmgTextPool { get; private set; }
    
    void Start() 
    {
        dmgTextPool = new ObjectPool<DamageText>(CreateDamageTextObject, OnTakeDamageTextFromPool, OnReturnTextDamageToPool);

        //initialize particle dictionary
        particleDictionary.Clear();
        foreach (ParticlePoolInstance pool in particlePoolsInstance)
        {
            particleDictionary.Add(pool.poolName, pool.particlePrefab);
        }
        particlePoolsDictionary.Clear();
        //initialize particle pools dictionary
        foreach (ParticlePoolInstance pool in particlePoolsInstance) {

            particlePoolsDictionary.Add(pool.poolName, new ObjectPool<PoolParticle>(CreateParticleForPool, OnTakeParticleFromPool, OnReturnParticleToPool));
        }

    }

    //really sneaky treak, honestly.
    // need to set this before asking for a particle
    string particlePoolName_SneakRefrence = "";
    public PoolParticle SpawnParticle(string _particleName)
    {
        particlePoolName_SneakRefrence = _particleName;
        //Debug.Log(particlePoolName_SneakRefrence + "   " + _particleName);
        if(particlePoolsDictionary.TryGetValue(particlePoolName_SneakRefrence, out ObjectPool<PoolParticle> _pool)){
            //particlePoolName_SneakRefrence = "";
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
        _poolParticle._particleSystem.Stop();
        _poolParticle._particleSystem.Play();
    }
    private void OnReturnParticleToPool(PoolParticle _poolParticle)
    {
        _poolParticle.gameObject.SetActive(false);
    }


    #region dmg text
    DamageText CreateDamageTextObject()
    {
        var damageText = Instantiate(DmgTextPrefab);
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
}

[Serializable]
public struct ParticlePoolInstance
{
    //public ObjectPool<ParticleSystem> _particlePool { get; private set; }
    [field:SerializeField] public PoolParticle particlePrefab { get; private set; }
    [field: SerializeField] public string poolName { get; private set; }
}
