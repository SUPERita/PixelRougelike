using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponCollection", menuName = "DataSet/WeaponCollection")]
public class WeaponCollection : ScriptableObject
{
    [field: SerializeField] public WeaponNamePair[] weaponNamePairs { get; private set; }

    public GameObject GetWeaponFromName(string _name)
    {
        foreach (var pair in weaponNamePairs)
        {
            if (pair._weaponName == _name)
            {
                return pair._weapon.gameObject;
            }
        }

        Debug.LogError("asked for nonexsistane skill");
        return null;
    }

    public WeaponNamePair GetRandomWeapon()
    {
        return weaponNamePairs[UnityEngine.Random.Range(0,weaponNamePairs.Length)];
    }

}

[Serializable]
public struct WeaponNamePair
{
    [field: SerializeField] public string _weaponName { get; private set; }
    [field: SerializeField] public Weapon _weapon { get; private set; }
    [field: SerializeField] public Sprite _weaponIcon { get; private set; }
}
