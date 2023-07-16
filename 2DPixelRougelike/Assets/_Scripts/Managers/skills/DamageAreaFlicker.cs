using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAreaFlicker : DamageAreaBehaviour
{
    [SerializeField] private Collider2D c = null;
    [SerializeField] private float flickerSpeed = .5f;
    protected override void DoBehaviour()
    {
        base.DoBehaviour();

        c.enabled = !c.enabled;

    }

    private void Awake()
    {
        InvokeRepeating(nameof(DoBehaviour), 0, flickerSpeed);
    }
}
