using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysteryBox : MonoBehaviour, WaypointTarget
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerMovement>())
        {
            WeaponChoice.Instance.OpenWeaponChoice();
            Destroy(gameObject);
        }
    }

    public Transform GetTargetTransform()
    {
        return transform;
    }
    private void Start()
    {
        WaypointIndicatorManager.Instance.SummonWaypointIndicator(this);
    }
}
