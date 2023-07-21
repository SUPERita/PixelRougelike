
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponCollection", menuName = "DataSet/WeaponCollection")]
public class WeaponCollection : ScriptableObject
{
    [field: SerializeField] public WeaponNamePair[] weaponIconPairs { get; private set; }

    public GameObject GetWeaponFromName(string _name)
    {
        foreach (var pair in weaponIconPairs)
        {
            if (pair._weapon.gameObject.name == _name)
            {
                return pair._weapon.gameObject;
            }
        }

        Debug.LogError("asked for nonexsistane skill");
        return null;
    }

    public WeaponNamePair GetRandomWeapon()
    {
        return weaponIconPairs[UnityEngine.Random.Range(0,weaponIconPairs.Length)];
    }

}

[Serializable]
public struct WeaponNamePair
{
    [field: SerializeField] public Weapon _weapon { get; private set; }
    [field: SerializeField] public Sprite _weaponIcon { get; private set; }
}
