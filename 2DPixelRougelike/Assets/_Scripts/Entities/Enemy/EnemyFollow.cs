using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : Enemy
{
    protected override void DoBehaviour()
    {
        base.DoBehaviour();
        rb.velocity = -(transform.position - follow.position).normalized * 5f;
    }
}
