using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAreaRotate : DamageAreaBehaviour
{
    [SerializeField] private Vector3 finalRotationEuler = new Vector3(0,0,360f);
    [SerializeField] private float rotateTime = .25f;

    protected override void DoBehaviour()
    {
        base.DoBehaviour();
        finalRotationEuler.z += transform.eulerAngles.z;
        Debug.Log(finalRotationEuler.z);
        transform.DORotate(finalRotationEuler, rotateTime, RotateMode.FastBeyond360);
    }

    private void Start()
    {
        DoBehaviour();
    }
}
