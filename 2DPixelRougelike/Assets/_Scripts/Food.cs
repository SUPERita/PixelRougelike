using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : LootBase
{
    [Header("Food")]
    [SerializeField] private int healingAmount = 10;


    protected override void OnTouchPlayer(Collider2D _collision)
    {
        base.OnTouchPlayer(_collision);

        if (_collision.TryGetComponent(out PlayerMovement _out))
        {
            if (!_out.gameObject.GetComponent<Health>().IsFullHealth())
            {
                int _healAmount = Mathf.Max( healingAmount + PlayerStatsHolder.Instance.TryGetStat(StatType.PickUpHeal), 1);
                _out.gameObject.GetComponent<Health>().HealHealth(_healAmount); 
                PlayPickupSound();
                Destroy(gameObject);
            }
        }
    }


}
