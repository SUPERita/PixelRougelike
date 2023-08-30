using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using UnityEngine;

public class GiftDrop : LootBase
{
    [Header("GiftDrop")]
    [SerializeField] private int amount = 50;

    protected override void OnTouchPlayer(Collider2D _collision)
    {
        base.OnTouchPlayer(_collision);

        if (_collision.TryGetComponent(out PlayerMovement _out))
        {

            ResourceSystem.Instance.AddResourceAmount(ResourceType.Gold, amount);
            PlayPickupSound();
            Destroy(gameObject);
        }
    }
}
