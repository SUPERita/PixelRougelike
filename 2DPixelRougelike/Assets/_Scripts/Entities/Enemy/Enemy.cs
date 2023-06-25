using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains;
using MoreMountains.Feedbacks;
using DG.Tweening;
using MoreMountains.Tools;
using Sirenix.OdinInspector;
using Lean.Pool;

public class Enemy : MonoBehaviour, IDamageable, IHurtPlayer, IPoolable
{
    [SerializeField] protected Health health;
    [SerializeField] protected Transform follow = null;
    [SerializeField] protected Rigidbody2D rb = null;
    [SerializeField] protected LootHandler lootHandler = null;

    [Header("FX")]
    [SerializeField] private MMF_Player hitFeedback;
    //[SerializeField] private GameObject dieParticle = null;
    private float hitFlashTime = 0.25f;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Material regularMat;
    [SerializeField] private Material hitMat;
    [SerializeField] private float dieKnockSpeed = 10f;
    [Header("vals")]
    [SerializeField] protected int damage = 5;

    private bool alive = true;

    private void FixedUpdate()
    {
        if (!alive) { return; }

        DoBehaviour();
    }

    protected virtual void DoBehaviour()
    {
        //debug.log(doing);
    }

    #region Lifecycle

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        follow = FindAnyObjectByType<PlayerMovement>().transform;
        health.OnDie += Health_OnDie;
    }
    private void OnDestory()
    {
        //also goes off if you just disable the game object
        health.OnDie -= Health_OnDie;
    }
    //private void OnApplicationQuit()
    //{
    //    //also goes off if you just disable the game object
    //    health.OnDie -= Health_OnDie;
    //}

    private void Health_OnDie()
    {
        //sr.enabled = false;
        alive = false;
        //Debug.Log("do something about it v instantiating");
        GameObject _t = LeanPoolManager.Instance.SpawnFromPool("dieParticle");
        _t.transform.position = transform.position;
        LeanPoolManager.Instance.DespawnFromPool(_t, 1f);

        //Instantiate(dieParticle, transform.position, Quaternion.identity);
        GetComponent<Collider2D>().enabled = false;
        rb.velocity = dieKnockSpeed * (transform.position - follow.position);
        //Destroy(gameObject);  
        //transform.DOScale(0f, 1f).SetEase(Ease.InOutCirc).OnComplete(() => Destroy(gameObject));
        transform.DOScale(0f, 1f).SetEase(Ease.InOutCirc).OnComplete(() => LeanPoolManager.Instance.DespawnFromPool(gameObject));//transform.DOScale(0f, 1f).SetEase(Ease.InOutCirc).OnComplete(() => GetComponent<PoolEnemy>().ReleaseToPool());
        lootHandler.SpawnLoot();
        //ResourceSystem.Instance.AddResourceAmount(ResourceType.Gold, 7);
        //ResourceSystem.Instance.AddResourceAmount(ResourceType.EnergyNugget, 2);

        //MidRunUpgradesManager.Instance.OpenStatChoice();
        //health.OnDie -= Health_OnDie;
    }

    #endregion

    #region Interfaces

    public void TakeDamage(int _val)
    {
        if(!alive) return;
        //Debug.Log(AudioSystem.Instance.name);
        AudioSystem.Instance.PlaySound("s3");
        if(hitFeedback != null) { hitFeedback?.PlayFeedbacks();}
        if(health != null) {health.TakeDamage(_val); }

        //particle pool?
        GameObject _p = UnityEngine.Random.Range(0, 2) == 1 ? LeanPoolManager.Instance.SpawnFromPool("particleBase") : LeanPoolManager.Instance.SpawnFromPool("pv1");
        _p.transform.position = transform.position;
        LeanPoolManager.Instance.DespawnFromPool(_p, .3f);//_p.CallReleaseToPool(.3f);
        
        //replaced coroutines because invoke doestn use memory?
        Invoke(nameof(SetHitMat), 0);
        Invoke(nameof(SetRegMat), hitFlashTime);
        //StartCoroutine(SetMat(hitMat));
        //StartCoroutine(SetMat(regularMat, true));
    }

    public int GetDamage()
    {
        return damage;
    }

    //public virtual void OnReset()
    //{
        
    //}

    public void OnSpawn()
    {
        alive = true;
        //Instantiate(dieParticle, transform.position, Quaternion.identity);
        //GetComponent<Collider2D>().enabled = false;
        GetComponent<Collider2D>().enabled = true;
        //rb.velocity = dieKnockSpeed * (transform.position - follow.position);
        //Destroy(gameObject);
        transform.localScale = Vector3.one;//transform.DOScale(0f, 1f).SetEase(Ease.InOutCirc).OnComplete(() => Destroy(gameObject)); 
        //ResourceSystem.Instance.AddResourceAmount(ResourceType.Gold, 7);
        //ResourceSystem.Instance.AddResourceAmount(ResourceType.EnergyNugget, 2);\
        health.ResetHealth();
        //health.OnDie += Health_OnDie;
    }

    public void OnDespawn()
    {
        //throw new System.NotImplementedException();
    }

    #endregion

    #region Utils

    private void SetHitMat()
    {
        sr.material = hitMat;
    }
    private void SetRegMat()
    {
        sr.material = regularMat;
    }



    #endregion
}
