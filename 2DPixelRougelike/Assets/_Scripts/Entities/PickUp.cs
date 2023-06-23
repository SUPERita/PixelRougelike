using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
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
        if((target.position - transform.position).sqrMagnitude < 1f)
        {
            OnCollect();
            Destroy(gameObject);
            return;
        }

        transform.position = Vector3.Lerp(transform.position, target.position, .1f);
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
}
