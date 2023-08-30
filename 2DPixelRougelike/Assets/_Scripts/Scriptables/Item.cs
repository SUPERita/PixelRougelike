using MoreMountains.Feel;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Build;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Items/Item")]
public class Item : ScriptableObject
{
    [field: SerializeField] public string itemName { get; private set; }
    [field: SerializeField, PreviewField(50, ObjectFieldAlignment.Left)] public Sprite itemSprite { get; private set; }
    [field: SerializeField] public ItemRarity itemRarity { get; private set; }
    //[field: SerializeField] public int itemMinWave { get; private set; }
    [field: SerializeField] public List<PlayerStatInstance> statInstances { get; private set; }
    [field: SerializeField, TextArea(5,5)] public string itemDescription { get; private set; }

    public string GetItemStatsReadable(bool _withDescription = true)
    {
        string _out = "";
        if (_withDescription) { _out = itemDescription + "\n" + "\n"; }
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
                return 20;
            case ItemRarity.Uncommon:
                return 30;
            case ItemRarity.Rare:
                return 40;
            case ItemRarity.Epic:
                return 50;
            case ItemRarity.Legendary:
                return 70;

        }
        Debug.LogError("wth nigo");
        return -1;
    }


    public static Color ItemRarityToColor(ItemRarity _itemRarity)
    {
        switch(_itemRarity) { 
            case ItemRarity.Common:
                return Color.white; 
            case ItemRarity.Uncommon:
                return Color.green;
            case ItemRarity.Rare:
                return Color.blue;
            case ItemRarity.Epic:
                return Color.magenta;
            case ItemRarity.Legendary:
                return Color.yellow;
        }
        return Color.black;
    }


    public float relativeValue = 0;
    private void OnValidate()
    {
        relativeValue = 0;
        foreach (PlayerStatInstance _p in statInstances)
        {
            relativeValue += GetStatValue(_p.statName) * _p.number;
        }
    }
    private float GetStatValue(StatType _s)
    {
        switch (_s)
        {
            case StatType.MaxHealth:
                return 3f;
            case StatType.AttackSpeed:
                return 1f;
            case StatType.MoveSpeed:
                return .75f;
            case StatType.SkillCap:
                return 100;
            case StatType.SkillDamage:
                return 1f;
            case StatType.WeaponDamage:
                return 3;
            case StatType.Dodge:
                return 1;
            case StatType.Armor:
                return 1;
            case StatType.XPGain:
                return 1;
            case StatType.MoneyGain:
                return 2; 
            case StatType.WeaponAttackSpeed:
                return 1;
            case StatType.SkillCooldown:
                return 1.5f;
            case StatType.MeleeDamage:
                return 1;
            case StatType.PickUpRange:
                return .5f;
            case StatType.SkillProj:
                return 25;
            case StatType.HealthRegen:
                return 5;
            case StatType.EnemyAmount:
                return -1;
            case StatType.Invesment:
                return 1;
            case StatType.Luck:
                return 2;
            case StatType.PickUpHeal:
                return 1;
        }
        return 0;
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