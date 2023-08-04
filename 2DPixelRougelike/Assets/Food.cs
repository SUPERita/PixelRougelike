using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour, WaypointTarget
{
    [SerializeField] private int healingAmount = 10;

    public Transform GetTargetTransform()
    {
        return transform;
    }
    private void Start()
    {
        WaypointIndicatorManager.Instance.SummonWaypointIndicator(this);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerMovement _out))
        {
            if (!_out.gameObject.GetComponent<Health>().IsFullHealth())
            {
                AudioSystem.Instance.PlaySound("pickup_heal");
                _out.gameObject.GetComponent<Health>().HealHealth(healingAmount);
                Destroy(gameObject);
            }
        }
    }

}
