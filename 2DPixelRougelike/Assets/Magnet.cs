using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : LootBase
{
    protected override void OnTouchPlayer(Collider2D _collision)
    {
        base.OnTouchPlayer(_collision);

        //works
        foreach (PickUp _p in FindObjectsOfType<PickUp>()) _p.StartFollowing(FindObjectOfType<PlayerMovement>().transform);

        PlayPickupSound();
        Destroy(gameObject);

    }
}
