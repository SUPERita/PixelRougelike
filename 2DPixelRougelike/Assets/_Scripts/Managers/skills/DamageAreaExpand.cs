using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DamageAreaExpand : DamageAreaBehaviour
{
    [SerializeField] private float startSize = 1f;
    [SerializeField] private float finalSize = 3f;
    [SerializeField] private float scalingTime = .25f;

    protected override void DoBehaviour()
    {
        base.DoBehaviour();
        transform.localScale = Vector3.one * startSize;
        transform.DOScale(Vector3.one * finalSize, scalingTime);
    }

    private void Start()
    {
        DoBehaviour();
    }
}
