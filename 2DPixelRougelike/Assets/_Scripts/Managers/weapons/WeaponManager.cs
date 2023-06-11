using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private Weapon[] weapons;

    [Button]
    private void CacheWeaponSockets()
    {
        weaponSockets.Clear();
        foreach (Transform t in transform.GetComponentsInChildren<Transform>())
        {
            if(t == transform) { continue; }
            weaponSockets.Add(t);
        }
    }

    [SerializeField] private List<Transform> weaponSockets;
    
    void Start()
    {
        SpawnWeapons();
    }

    private void SpawnWeapons()
    {
        if(weaponSockets.Count < weapons.Length) { Debug.LogError("too many weapons not enougth sockets"); }
        ClearWeaponSockets();
        for (int i = 0; i < weapons.Length; i++)
        {
            
            Instantiate(weapons[i].gameObject, weaponSockets[i]);
        }
    }

    private void ClearWeaponSockets()
    {
        for (int i = 0; i < weaponSockets.Count; i++)
        {
            Helpers.DestroyChildren(weaponSockets[i]);  
        }
        
    }
}
