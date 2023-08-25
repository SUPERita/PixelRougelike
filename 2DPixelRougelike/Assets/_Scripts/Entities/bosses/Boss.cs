using Cinemachine;
using DG.Tweening;
using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Xsl;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class Boss : MonoBehaviour, IDamageable, IHurtPlayer
{
    [Header("Components")]
    [SerializeField] protected Health health;
    protected Transform follow = null;
    [SerializeField] protected Rigidbody2D rb = null;
    [SerializeField] protected LootHandler lootHandler = null;
    [SerializeField] protected CinemachineVirtualCamera bossCam = null;

    //[SerializeField] private MMF_Player hitFeedback;
    //[SerializeField] private GameObject dieParticle = null;
    [Header("FX")]
    [SerializeField] private SpriteRenderer sr;
    private float hitFlashTime = 0.25f;
    [SerializeField] private Material regularMat;
    [SerializeField] private Material hitMat;
    [SerializeField] private float dieKnockSpeed = 10f;
    [SerializeField] private MMF_Player spawnFX;
    [SerializeField] private float walkAnimSpeed = 1f;
    [Header("Vals")]
    [SerializeField] protected int damage = 5;

    protected Vector3 startSize = Vector3.one;
    private bool alive = true;

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

    #region Behaviours

    protected virtual void FollowPlayer()
    {
        rb.velocity = -(transform.position - follow.position).normalized * 5f;
    }

    #endregion

    #region Lifecycle

    private void Start()
    {
        startSize = transform.localScale;
        rb = GetComponent<Rigidbody2D>();
        follow = FindAnyObjectByType<PlayerMovement>().transform;
        health.OnDie += Health_OnDie;

        if (SettingsCanvas.Instance.showCutscenes) StartCoroutine(ShowBossCamFor(2f));

        sr.transform.DOScaleY(sr.transform.localScale.y*1.10f, 1f / walkAnimSpeed).SetLoops(-1, LoopType.Yoyo).SetAutoKill(false);


    }
    
    private void OnDestory()
    {
        health.OnDie -= Health_OnDie;
    }
    private void Health_OnDie()
    {
        alive = false;

        //death particle fx
        GameObject _t = LeanPoolManager.Instance.SpawnFromPool("dieParticle");
        _t.transform.position = transform.position;
        LeanPoolManager.Instance.DespawnFromPool(_t, 1f);
        rb.velocity = dieKnockSpeed * (transform.position - follow.position);

        //disable collision
        GetComponent<Collider2D>().enabled = false;

        //stop any tweens and shit
        transform.DOKill();

        //tween and destroy
        transform.DOScale(0f, 1f).SetEase(Ease.InBounce)
            .OnComplete(() => Destroy(gameObject));
        lootHandler.SpawnLoot();

        SteamIntegration.UnlockAchievment("ACH_KILL1BOSS");
    }

    #endregion

    #region Interfaces
    public void TakeDamage(int _val)
    {
        if (!alive) return;
        //Debug.Log(AudioSystem.Instance.name);
        //sfx
        if (Random.value > .5) AudioSystem.Instance.PlaySound("hit_thud1", .2f);
        else AudioSystem.Instance.PlaySound("hit_thud2", .2f);

        HitTween();//if (hitFeedback != null) { hitFeedback?.PlayFeedbacks(); }
        if (health != null) { health.TakeDamage(_val); }

        //particle pool?
        GameObject _p = LeanPoolManager.Instance.SpawnFromPool("enemyHit1");//UnityEngine.Random.Range(0, 2) == 1 ? LeanPoolManager.Instance.SpawnFromPool("particleBase") : LeanPoolManager.Instance.SpawnFromPool("pv1");
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

    Tween _t = null;
    private void HitTween()
    {
        if (_t == null)
        {
            //DOPunchScale is additive to the current scale
            //collider with the dash scale
            //_t = transform.DOPunchScale(new Vector3(.5f, -.5f, transform.localScale.z), .15f, 0, 0f)
            //    .SetAutoKill(false);
            _t = transform.DOPunchRotation(new Vector3(0, 0, 10f), .1f, 5, .2f)
                .SetAutoKill(false);
        }
        else
        {
            _t.Restart();
        }
    }

    protected virtual bool DistanceFromPlayerLessThan(float _d)
    {
        return (follow.position - transform.position).sqrMagnitude < _d * _d;

    }

    private IEnumerator ShowBossCamFor(float _timeInSeconds)
    {
        bossCam.enabled = true;
        Time.timeScale = 0;
        GameStateManager.Instance.SetState(GameState.Cutscene);

        spawnFX?.PlayFeedbacks();
        StartCoroutine(DoInTime(()=>AudioSystem.Instance.PlaySound("levelup_vibrato"), .4f));
        yield return new WaitForSecondsRealtime(_timeInSeconds);

        bossCam.enabled = false;
        Time.timeScale = 1;
        GameStateManager.Instance.ReturnToBaseState();
    }
    private IEnumerator DoInTime(UnityAction _A, float time = 0f)
    {
        yield return new WaitForSecondsRealtime(time);
        _A.Invoke();
    }

    #endregion
}
