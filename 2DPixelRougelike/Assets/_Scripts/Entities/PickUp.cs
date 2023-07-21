using DG.Tweening;
using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PickUp : MonoBehaviour, IPoolable
{
    private Transform target = null;
    [SerializeField] private ResourceType resourceType = ResourceType.EnergyNugget;
    [SerializeField] private int amount = 3;
    [SerializeField] private CircleCollider2D collider = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartFollowing(collision.transform);
        }
    }

    public void StartFollowing(Transform _t)
    {
        target = _t;
        FollowTarget();
    }

    private void FollowTarget()
    {
        if (target == null) return;
        if((target.position - transform.position).sqrMagnitude < 1f)
        {
            OnCollect();
            LeanPoolManager.Instance.DespawnFromPool(gameObject); //Destroy(gameObject);
            target = null;
            return;
        } 

        transform.position = Vector3.Slerp(transform.position, target.position, .2f);

        
        Invoke(nameof(FollowTarget), .02f);
    }

    protected virtual void OnCollect()
    {
        //Debug.Log("1");
        AudioSystem.Instance.PlaySound("Beep3", .2f);
        if(resourceType == ResourceType.EnergyNugget) XPManager.Instance.AddXP(5000);

        ResourceSystem.Instance.AddResourceAmount(resourceType, amount);
    }

    private void Start()
    {
        t = transform.DOScale(1f, .25f).SetAutoKill(false);
        
        //PopOutTween();
    }

    Tween t = null;
    private void PopOutTween()
    {
        transform.localScale = Vector3.one*.5f;
        //transform.DOKill();
        t.Rewind();
        t.Play();   
        //t = transform.DOScale(1f, .25f);
        //MessageBoard.Instance.SpawnMessage("restarted");
    }

    public void OnSpawn()
    {
        
        PopOutTween();
        transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(-90, 90));
        collider.radius = 2.5f * (1+PlayerStatsHolder.Instance.TryGetStat(StatType.PickUpRange)/100f);
    }

    public void OnDespawn()
    {
        
    }
}
