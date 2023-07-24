
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Tilemaps;
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
    [Button]
    private void Editor_ConnectWeaponToIcon()
    {
        for (int i = 0; i < weaponIconPairs.Length; i++)
        {
            if (weaponIconPairs[i]._weaponIcon == null)
            {
                weaponIconPairs[i] = new WeaponNamePair(
                    weaponIconPairs[i]._weapon,
                    weaponIconPairs[i]._weapon.gameObject.GetComponentInChildren<SpriteRenderer>().sprite);
            }
        }
    }

}

[Serializable]
public struct WeaponNamePair
{
    [field: SerializeField] public Weapon _weapon { get; private set; }
    [field: SerializeField] public Sprite _weaponIcon { get; private set; }

    public WeaponNamePair(Weapon _w,Sprite _s)
    {
        _weapon = _w;
        _weaponIcon = _s;
    }
}
