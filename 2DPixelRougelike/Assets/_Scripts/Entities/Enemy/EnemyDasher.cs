using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDasher : Enemy
{
    [Header("Dasher")]
    [SerializeField] private float dashDuration = .5f;
    [SerializeField] private float dashDelay = .5f;
    [SerializeField] private float dashCooldown = 2f;
    [SerializeField] private float dashRange = 7f;
    [SerializeField] private float overShot = 1f;
    private float nextDashAvailable = 0f;
    private bool doingSomething = false;

    protected override void DoBehaviour()
    {
        base.DoBehaviour();

        if(doingSomething) { return; }

        if (nextDashAvailable < Time.time && InRange())
        {
            Dash();
            nextDashAvailable = dashCooldown +Time.time;
        } 
        else
        {
            FollowPlayer();
        }

    }

    private void FollowPlayer()
    {
        rb.velocity = -(transform.position - follow.position).normalized * 5f;
    }

    protected virtual bool InRange()
    {
        return (follow.position - transform.position).sqrMagnitude < dashRange * dashRange;
       
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, dashRange);
    }

    protected virtual void Dash()
    {
        doingSomething = true;

        Vector3 playerPosPlus = follow.position + (follow.position - transform.position).normalized * overShot;
        transform.DOScaleY(0.5f, dashDelay).OnComplete(()=> transform.DOScaleY(1f, dashDelay/10f));
        rb.DOMove(playerPosPlus, dashDuration)
            .SetDelay(dashDelay)
            .OnComplete(() =>doingSomething = false);
            
    }

    public override void OnDespawn()
    {
        doingSomething = false;
        base.OnDespawn();
    }

}
