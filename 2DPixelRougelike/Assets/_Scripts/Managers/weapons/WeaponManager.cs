using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : StaticInstance<WeaponManager>
{
    [SerializeField] private List<Weapon> weapons;

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
        if(weaponSockets.Count < weapons.Count) { Debug.LogError("too many weapons not enougth sockets"); }
        ClearWeaponSockets();
        for (int i = 0; i < weapons.Count; i++)
        {
            
            Instantiate(weapons[i].gameObject, weaponSockets[i]);
        }
    }

    public void AddWeapon(Weapon weapon)
    {
        weapons.Add(weapon);
        SpawnWeapons();
    }

    private void ClearWeaponSockets()
    {
        for (int i = 0; i < weaponSockets.Count; i++)
        {
            Helpers.DestroyChildren(weaponSockets[i]);  
        }
        
    }
}
