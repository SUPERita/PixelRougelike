using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Items/Item")]
public class Item : ScriptableObject
{
    [field: SerializeField] public string itemName { get; private set; }
    [field: SerializeField, PreviewField(50, ObjectFieldAlignment.Left)] public Sprite itemSprite { get; private set; }
    [field: SerializeField] public Vector2 itemPriceRange { get; private set; }
    [field: SerializeField] public List<PlayerStatInstance> statInstances { get; private set; }
    [field: SerializeField, TextArea(5,5)] public string itemDescription { get; private set; }

    public string GetItemStatsReadable()
    {
        string _out = "";
        PlayerStats _p = PlayerStatsHolder.Instance.GetPlayerStats();
        foreach (var _stat in statInstances)
        {
            string _statNumber = _stat.number.ToString();
            _statNumber = _stat.number > 0 ?
                "<color=green>" + _statNumber + "</color>" :
                "<color=red>" + _statNumber + "</color>";

            _out += _p.GetPlayerStatRaw(_stat.statName).statName +" "+ _statNumber +"(<sprite=0>) "+ "\n";
        }

        return _out;
    }
}

