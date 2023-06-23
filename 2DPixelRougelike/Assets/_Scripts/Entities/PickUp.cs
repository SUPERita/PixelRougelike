using DG.Tweening;
using Lean.Pool;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour, IPoolable
{
    private Transform target = null;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            target = collision.transform;
            FollowTarget();
        }
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
        XPManager.Instance.AddXP(5);
        ResourceSystem.Instance.AddResourceAmount(ResourceType.EnergyNugget, 3);
    }

    private void Start()
    {
        PopOutTween();
    }

    Tween t = null;
    private void PopOutTween()
    {
        transform.localScale = Vector3.zero;
        if (t == null)
        {
            t = transform.DOScale(1f, .25f);
        }
        else
        {
            t.Restart();
        }
    }

    public void OnSpawn()
    {
        PopOutTween();
    }

    public void OnDespawn()
    {
        
    }
}
