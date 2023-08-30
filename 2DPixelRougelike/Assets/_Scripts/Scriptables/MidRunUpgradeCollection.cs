
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MidRunUpgradeCollection", menuName = "DataSet/MidRunUpgradeCollection")]
public class MidRunUpgradeCollection : ScriptableObject
{
    [System.Serializable]
    public struct StatTypeValRarityPair
{
        [field: SerializeField] public StatType statType { get; private set; }
        [field: SerializeField] public int baseStatValue { get; private set; }
        public MidRunUpgradeRarity rarity { get; private set; }

        public StatTypeValRarityPair(int _StatValue, StatType _statType, MidRunUpgradeRarity _midRunUpgradeRarity)
        {
            baseStatValue = _StatValue;
            rarity = _midRunUpgradeRarity;
            statType = _statType;
        }
    }

    [field: SerializeField] public StatTypeValRarityPair[] commonStats { get; private set; }

    private StatTypeValRarityPair GetRandomUpgrade()
    {
        return commonStats[Random.Range(0, commonStats.Length)];
    }
    public StatTypeValRarityPair GetRandomStatOfRandomRarity(float _luck=0)
    {
        StatTypeValRarityPair _base = GetRandomUpgrade();

        MidRunUpgradeRarity _choosenRarity = GetRandomRarity(_luck);
        StatTypeValRarityPair _out = new StatTypeValRarityPair(_base.baseStatValue*(int)_choosenRarity, _base.statType, _choosenRarity);

        return _out;
    }

    
    private MidRunUpgradeRarity GetRandomRarity(float _luckAddition=0)
    { 
        if (_luckAddition < -60) _luckAddition = -60;
        float UncommonChance = 25f * (1 + _luckAddition /70f);
        float RareChance = 10f * (1 + _luckAddition / 70f);
        float EpicChance = 4f * (1 + _luckAddition / 70f);
        float LegendaryChance = 1f * (1 + _luckAddition / 70f);

        float _f = Random.Range(0f, 100f);
        //need to look at that
        //_f *= 1+(_luckAddition/200f);
        MidRunUpgradeRarity _u = MidRunUpgradeRarity.Common;

        if(_f> 100 - (UncommonChance + RareChance + EpicChance + LegendaryChance))
        {
            _u = MidRunUpgradeRarity.Uncommon;
        }
        if (_f > 100 - (RareChance + EpicChance + LegendaryChance))
        {
            _u = MidRunUpgradeRarity.Rare;
        }
        if (_f > 100 - (EpicChance + LegendaryChance))
        {
            _u = MidRunUpgradeRarity.Epic;
        }
        if (_f > 100 - (LegendaryChance))
        {
            _u = MidRunUpgradeRarity.Legendary;
        }

        //MessageBoard.Instance.SpawnMessage(_f + "%/100");
        return _u;
    }

    public Color UpgradeRarityToColor(MidRunUpgradeRarity _rar)
    {
        switch(_rar) {
            case MidRunUpgradeRarity.Common:
                return Color.gray;
            case MidRunUpgradeRarity.Uncommon: 
                return Color.green;
            case MidRunUpgradeRarity.Rare:
                return Color.blue;
            case MidRunUpgradeRarity.Epic:
                return Color.magenta;
            case MidRunUpgradeRarity.Legendary:
                return Color.yellow;

        }
        Debug.LogError("i dont even know how");
        return Color.black;

    }

}


public enum MidRunUpgradeRarity
{
    Common =1,
    Uncommon=2,
    Rare=3,
    Epic=4,
    Legendary=5
}
