using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDasher : Boss
{
    [Header("Dasher")]
    [SerializeField] private float dashDuration = .5f;
    [SerializeField] private float dashDelay = 1f;
    [SerializeField] private float dashCooldown = 5f;
    [SerializeField] private float dashRange = 7f;
    [SerializeField] private float overShot = 0f;
    private float nextDashAvailable = 0f;
    private bool doingSomething = false;

    protected override void DoBehaviour()
    {
        base.DoBehaviour();

        if (doingSomething) { return; }

        if (nextDashAvailable < Time.time && DistanceFromPlayerLessThan(dashRange))
        {
            Dash();
            nextDashAvailable = dashCooldown + Time.time;
        }
        else
        {
            FollowPlayer();
        }

    }

    protected virtual void Dash()
    {
        doingSomething = true;

        Vector3 playerPosPlus = follow.position + (follow.position - transform.position).normalized * overShot;
        transform.DOScaleY(0.5f, dashDelay).OnComplete(() => transform.DOScale(startSize, dashDelay / 10f));
        rb.DOMove(playerPosPlus, dashDuration)
            .SetDelay(dashDelay)
            .OnComplete(() => doingSomething = false);

    }
}
