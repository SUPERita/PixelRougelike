using System.Collections;
using System.Collections.Generic;
using System.Net.Configuration;
using UnityEngine;

public class LootHandler : MonoBehaviour
{
    //[SerializeField] private PickUp[] loot;
    
    public void SpawnLoot()
    {
        //if (loot == null) return;
        LeanPoolManager.Instance.SpawnFromPool("pickupNugget").transform.position = transform.position + Random.insideUnitSphere;

        if(Random.Range(0, 2) == 1) LeanPoolManager.Instance.SpawnFromPool("pickupNugget").transform.position = transform.position + Random.insideUnitSphere;
        else LeanPoolManager.Instance.SpawnFromPool("pickupGold").transform.position = transform.position + Random.insideUnitSphere ;

        
            
        //Instantiate(loot[0], , Quaternion.identity);
    }

}
