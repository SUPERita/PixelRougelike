using System.Collections;
using System.Collections.Generic;
//using System.Net.Configuration;
using UnityEngine;

public class LootHandler : MonoBehaviour
{
    [SerializeField] private int amount = 2;
    [SerializeField] private GameObject[] objectsToSpawn = null;
    public void SpawnLoot()
    {
        //if (loot == null) return;
        //LeanPoolManager.Instance.SpawnFromPool("pickupNugget").transform.position = transform.position + Random.insideUnitSphere;

        for (int i = 0; i < amount; i++)
        {
            if(Helpers.RollChance(90f)/*Random.Range(0, 2) == 1*/) LeanPoolManager.Instance.SpawnFromPool("pickupNugget").transform.position = transform.position + Random.insideUnitSphere;
            else LeanPoolManager.Instance.SpawnFromPool("pickupGold").transform.position = transform.position + Random.insideUnitSphere ;
        }

        foreach (GameObject obj in objectsToSpawn)
        {
            Instantiate(obj, transform.position + Random.insideUnitSphere, transform.rotation);
        }


        
            
        //Instantiate(loot[0], , Quaternion.identity);
    }

}
