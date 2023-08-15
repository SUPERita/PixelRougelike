using MoreMountains.Feel;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Items/Item")]
public class Item : ScriptableObject
{
    [field: SerializeField] public string itemName { get; private set; }
    [field: SerializeField, PreviewField(50, ObjectFieldAlignment.Left)] public Sprite itemSprite { get; private set; }
    [field: SerializeField] public ItemRarity itemRarity { get; private set; }
    [field: SerializeField] public int itemMinWave { get; private set; }
    [field: SerializeField] public List<PlayerStatInstance> statInstances { get; private set; }
    [field: SerializeField, TextArea(5,5)] public string itemDescription { get; private set; }

    public string GetItemStatsReadable()
    {
        string _out = itemDescription + "\n";
        PlayerStats _p = PlayerStatsHolder.Instance.GetPlayerStats();
        foreach (var _stat in statInstances)
        {
            string _statNumber = _stat.number.ToString();
            _statNumber = _stat.number > 0 ?
                "<color=green>" +"+"+ _statNumber + "</color>" :
                "<color=red>" + _statNumber + "</color>";
            string _statImage = $"<sprite name={_p.GetPlayerStatRaw(_stat.statName).statName}>";

            _out += _p.GetPlayerStatRaw(_stat.statName).statName +" "+ _statNumber + $"({_statImage})" + "\n";
        }

        return _out;
    }

    public static int RarityToBasePrice(ItemRarity _itemRarity)
    {
        switch (_itemRarity)
        {
            case ItemRarity.Common:
                return 10;
            case ItemRarity.Uncommon:
                return 15;
            case ItemRarity.Rare:
                return 20;
            case ItemRarity.Epic:
                return 25;
            case ItemRarity.Legendary:
                return 35;

        }
        Debug.LogError("wth nigo");
        return -1;
    }
}

public enum ItemRarity
{
    Common,
    Uncommon,
    Rare, 
    Epic,
    Legendary
}