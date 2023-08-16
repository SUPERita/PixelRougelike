using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootEnemy : Enemy
{
    private Vector2 targetPosition = Vector3.zero;
    private float timer = 0f;
    protected override void DoBehaviour()
    {
        base.DoBehaviour();
        //timer stuff
        timer -= Time.fixedDeltaTime;
        if(timer < 0f || ((Vector2)transform.position - targetPosition).sqrMagnitude < 2*2)
        {
            //pick new target spot
            targetPosition = Random.insideUnitCircle*32;
            timer = 5f;
        }

        rb.velocity = (targetPosition - (Vector2)transform.position).normalized * walkSpeed;
    }
}
