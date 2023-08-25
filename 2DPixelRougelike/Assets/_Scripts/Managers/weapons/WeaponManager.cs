using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WeaponManager : StaticInstance<WeaponManager>
{
    [SerializeField] private List<Weapon> weapons;
    [SerializeField] private GameObject sampleWeaponSocket;

    //[Button]
    //private void CacheWeaponSockets()
    //{
    //    weaponSockets.Clear();
    //    foreach (Transform t in transform.GetComponentsInChildren<Transform>())
    //    {
    //        if(t == transform) { continue; }
    //        weaponSockets.Add(t);
    //    }
    //}

    //[SerializeField] private List<Transform> weaponSockets;
    
    void Start()
    {
        SpawnWeapons();
    }

    private void SpawnWeapons()
    {
        //if(weaponSockets.Count < weapons.Count) { Debug.LogError("too many weapons not enougth sockets"); }
        ClearWeaponSockets();
        for (int i = 0; i < weapons.Count; i++)
        {
            Transform _socket = Instantiate(sampleWeaponSocket, transform).transform;
            //move the socket a little
            _socket.transform.localPosition = AngleToUnitCircle(i*360f/weapons.Count)*2f;
            Instantiate(weapons[i].gameObject, _socket);
        }
    }

    public bool TryAddWeapon(Weapon weapon)
    {
        //if (HasMaxWeapons())
        //{
        //    return false;
        //}
        weapons.Add(weapon);
        SpawnWeapons();

        return true;

    }

    //private bool HasMaxWeapons()
    //{
    //    return weapons.Count >= weaponSockets.Count;
    //}

    private void ClearWeaponSockets()
    {
        Helpers.DestroyChildren(transform);
        //for (int i = 0; i < weaponSockets.Count; i++)
        //{
        //    Helpers.DestroyChildren(weaponSockets[i]);  
        //}
    }
    public void Test_ClearWeaponSockets()
    {
        weapons = new List<Weapon>();
        SpawnWeapons();
    }

    public Vector2 AngleToUnitCircle(float _angleDegrees)
    {
        float _angleRadians = _angleDegrees * Mathf.Deg2Rad;
        float x = Mathf.Cos(_angleRadians) +0f;
        float y = Mathf.Sin(_angleRadians);
        return new Vector2(x, y);
    }
}
