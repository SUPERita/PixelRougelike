using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : Singleton<PoolManager>
{

    [SerializeField] private DamageText DmgTextPrefab = null;

    public ObjectPool<DamageText> dmgTextPool { get; private set; }
    // Start is called before the first frame update
    void Start() => dmgTextPool = new ObjectPool<DamageText>(CreateDamageTextObject, OnTakeDamageTextFromPool, OnReturnTextDamageToPool);


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

}
