using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains;
using MoreMountains.Feedbacks;
using DG.Tweening;
using MoreMountains.Tools;

public class BoxBase : MonoBehaviour, IDamageable
{
    [SerializeField] private Health health;
    [SerializeField] private Transform follow = null;
    [SerializeField] private Rigidbody2D rb = null;

    [Header("FX")]
    [SerializeField] private MMF_Player hitFeedback;
    [SerializeField] private GameObject particleSystem = null;
    private float hitFlashTime = 0.25f;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Material regularMat;
    [SerializeField] private Material hitMat;
    [SerializeField] private float dieKnockSpeed = 10f;


    private bool alive = true;
    public void TakeDamage(int _val)
    {
        if(!alive) return;
        //Debug.Log(AudioSystem.Instance.name);
        AudioSystem.Instance.PlaySound("s3");
        if(hitFeedback != null) { hitFeedback?.PlayFeedbacks();}
        if(health != null) {health.TakeDamage(_val); }
        
        Invoke(nameof(SetHitMat), 0);
        Invoke(nameof(SetRegMat), hitFlashTime);
        //StartCoroutine(SetMat(hitMat));
        //StartCoroutine(SetMat(regularMat, true));
    }

    //WaitForSeconds _waitCache = new WaitForSeconds(0.25f);
    //WaitForSeconds _noWaitCache = new WaitForSeconds(0f);
    //private IEnumerator SetMat(Material _m, bool _waitFlashTime = false)
    //{
    //    //yield return new WaitForSeconds(_t);
    //    if(_waitFlashTime)
    //    {
    //        yield return _waitCache;
    //    } else
    //    {
    //        yield return _noWaitCache;
    //    }
    //    sr.material = _m;
    //}

    private void SetHitMat()
    {
        sr.material = hitMat;
    }
    private void SetRegMat()
    {
        sr.material = regularMat;
    }

    private MaterialPropertyBlock materialPropertyBlock;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        follow = FindAnyObjectByType<PlayerMovement>().transform;
        health.OnDie += Health_OnDie;
    }
    private void OnDisable()
    {
        //also goes off if you just disable the game object
        health.OnDie -= Health_OnDie;
    }

    private void Health_OnDie()
    {
        //sr.enabled = false;
        alive = false;
        Instantiate(particleSystem, transform.position, Quaternion.identity);
        GetComponent<Collider2D>().enabled = false;
        rb.velocity = dieKnockSpeed * (transform.position - follow.position);
        //Destroy(gameObject);
        transform.DOScale(0f, 1f).SetEase(Ease.InOutCirc).OnComplete(() => Destroy(gameObject));
        ResourceSystem.Instance.AddResourceAmount(ResourceType.Gold, 7);
        ResourceSystem.Instance.AddResourceAmount(ResourceType.EnergyNugget, 2);
        //MidRunUpgradesManager.Instance.OpenStatChoice();
    }

    private void FixedUpdate()
    {
        if (!alive) { return; }

        rb.velocity = -(transform.position - follow.position).normalized * 5f;
    }
}
