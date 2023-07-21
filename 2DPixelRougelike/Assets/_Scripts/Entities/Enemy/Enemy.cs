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
    [SerializeField] private float walkAnimSpeed = 4f;
    [SerializeField] protected float walkSpeed = 4f;
    [Header("vals")]
    [SerializeField] protected int damage = 5;
    [field: SerializeField] public int enemyWeight { get; private set; } = 1;

    private bool alive = true;
    private Tween _walkTween = null;
    private Tween _hitTween = null;
    //[SerializeField] protected float startScale = 1f;

    private void FixedUpdate()
    {
        if (!alive) { return; }

        DoBehaviour();

        sr.flipX = rb.velocity.x < 0;
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
        rb.DOKill();
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

        //sfx
        AudioSystem.Instance.PlaySound("s3");

        //squach and stretch
        if(hitFeedback != null) { hitFeedback?.PlayFeedbacks();}
        //if(_hitTween == null) _hitTween = sr.transform.DOPunchScale(Vector3.one + Vector3.up*.75f, .2f, 5).SetAutoKill(false);
        //_hitTween?.Restart();

        //health
        if (health != null) {health.TakeDamage(_val); }

        //particle pool?
        GameObject _p = UnityEngine.Random.Range(0, 2) == 1 ? LeanPoolManager.Instance.SpawnFromPool("particleBase") : LeanPoolManager.Instance.SpawnFromPool("pv1");
        _p.transform.position = sr.transform.position;
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


    public virtual void OnSpawn()
    {
        alive = true;
        //Instantiate(dieParticle, transform.position, Quaternion.identity);
        //GetComponent<Collider2D>().enabled = false;
        GetComponent<Collider2D>().enabled = true;
        //rb.velocity = dieKnockSpeed * (transform.position - follow.position);
        //Destroy(gameObject);
        //if(startScale == null) { startScale = transform.localScale; };
        transform.localScale = Vector2.one; //transform.DOScale(0f, 1f).SetEase(Ease.InOutCirc).OnComplete(() => Destroy(gameObject)); 

        //ResourceSystem.Instance.AddResourceAmount(ResourceType.Gold, 7);
        //ResourceSystem.Instance.AddResourceAmount(ResourceType.EnergyNugget, 2);\
        health.ResetHealth();
        //health.OnDie += Health_OnDie;
        //walk tween managment
        //transform.DOKill();
        if (_walkTween == null) _walkTween = transform.DOScaleY(1.15f, 1f/walkAnimSpeed).SetLoops(-1, LoopType.Yoyo).SetAutoKill(false); 
        _walkTween?.Restart();
    }

    public virtual void OnDespawn()
    {
        if (_walkTween != null) _walkTween.Rewind();
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
