using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBase : MonoBehaviour, WaypointTarget
{
    [SerializeField] private string pickup_sound = string.Empty;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerMovement>())
        {
            OnTouchPlayer(collision);
        }
    }

    public Transform GetTargetTransform()
    {
        return transform;
    }
    protected virtual void Start()
    {
        WaypointIndicatorManager.Instance.SummonWaypointIndicator(this);
    }

    protected virtual void OnTouchPlayer(Collider2D _collison)
    {
        
    }
    protected void PlayPickupSound()
    {
        AudioSystem.Instance.PlaySound(pickup_sound);
    }
}
