using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysteryBox : LootBase
{
    protected override void OnTouchPlayer(Collider2D _collision)
    {
        base.OnTouchPlayer(_collision);
        WeaponChoice.Instance.OpenWeaponChoice();
        Destroy(gameObject);

    }
}
