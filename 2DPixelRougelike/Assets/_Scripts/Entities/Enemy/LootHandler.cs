using System.Collections;
using System.Collections.Generic;
using System.Net.Configuration;
using UnityEngine;

public class LootHandler : MonoBehaviour
{
    [SerializeField] private PickUp[] loot;
    
    public void SpawnLoot()
    {
        if (loot == null) return;
        Instantiate(loot[0], transform.position + Random.insideUnitSphere, Quaternion.identity);
    }

}
