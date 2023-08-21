using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemCollection", menuName = "DataSet/ItemCollection")]
public class ItemCollection : ScriptableObject
{
    [SerializeField] private List<Item> items;

    public Item GetRandomItem()
    {
        return items[Random.Range(0, items.Count)];
    }
    //public Item GetRandomItemInWave(int _wave)
    //{
    //    //Item _out = GetRandomItem();
    //    //int counter = 0;
    //    //while(_out.itemMinWave > _wave && counter<2000)
    //    //{
    //    //    _out = GetRandomItem();
    //    //    counter++;
    //    //}

    //    //return _out;

    //    List <Item> _listGetRandomFrom = new List<Item>();
    //    /*commons always get added*/ AddItemsOfRarityToList(out _listGetRandomFrom, _listGetRandomFrom, ItemRarity.Common);
    //    if (_wave >= 3) AddItemsOfRarityToList(out _listGetRandomFrom, _listGetRandomFrom, ItemRarity.Uncommon);
    //    if (_wave >= 6) AddItemsOfRarityToList(out _listGetRandomFrom, _listGetRandomFrom, ItemRarity.Rare);
    //    if (_wave >= 9) AddItemsOfRarityToList(out _listGetRandomFrom, _listGetRandomFrom, ItemRarity.Epic);
    //    if (_wave >= 12) AddItemsOfRarityToList(out _listGetRandomFrom, _listGetRandomFrom, ItemRarity.Legendary);

    //    return _listGetRandomFrom[Random.Range(0, _listGetRandomFrom.Count)];
    //}


    public Item GetRandomItemInWave(int _wave)
    {
        //List<Item> _commonItems = new List<Item>();
        //List<Item> _uncommonItems = new List<Item>();
        //List<Item> _rareItems = new List<Item>();
        //List<Item> _epicItems = new List<Item>();
        //List<Item> _legendaryItems = new List<Item>();

        //AddItemsOfRarityToList(out _commonItems, _commonItems, ItemRarity.Common);
        //AddItemsOfRarityToList(out _uncommonItems, _uncommonItems, ItemRarity.Uncommon);
        //AddItemsOfRarityToList(out _rareItems, _rareItems, ItemRarity.Rare);
        //AddItemsOfRarityToList(out _epicItems, _epicItems, ItemRarity.Epic);
        //AddItemsOfRarityToList(out _legendaryItems, _legendaryItems, ItemRarity.Legendary);
        //float luckMultiplyAmount = (1 + (PlayerStatsHolder.Instance.TryGetStat(StatType.Luck) / 200f));
        float UncommonChance = 25f * (1 + (PlayerStatsHolder.Instance.TryGetStat(StatType.Luck) / 200f));
        float RareChance = 10f * (1 + (PlayerStatsHolder.Instance.TryGetStat(StatType.Luck) / 100f));
        float EpicChance = 4f * (1 + (PlayerStatsHolder.Instance.TryGetStat(StatType.Luck) / 50f));
        float LegendaryChance = 1f * (1 + (PlayerStatsHolder.Instance.TryGetStat(StatType.Luck) / 50f));

        //choose rarity, with random while regarding current wave
        float _f = Random.Range(0f, 100f);
        //_f *= ;
        ItemRarity _choosenRarity = ItemRarity.Common;
        if (_wave >= 3 && _f > 100 - (UncommonChance + RareChance + EpicChance + LegendaryChance)) _choosenRarity = ItemRarity.Uncommon;
        if (_wave >= 6 && _f > 100 - (RareChance + EpicChance + LegendaryChance)) _choosenRarity = ItemRarity.Rare;
        if (_wave >= 9 && _f > 100 - (EpicChance + LegendaryChance)) _choosenRarity = ItemRarity.Epic;
        if (_wave >= 12 && _f > 100 - (LegendaryChance)) _choosenRarity = ItemRarity.Legendary;

        //MessageBoard.Instance.SpawnMessage(_f + "%/100");
        //create a list of all items in rarity
        List<Item> _listGetRandomFrom = new List<Item>();
        AddItemsOfRarityToList(out _listGetRandomFrom, _listGetRandomFrom, _choosenRarity);
        //get random from choosen rarity
        return _listGetRandomFrom[Random.Range(0, _listGetRandomFrom.Count)];
    }

    private void AddItemsOfRarityToList(out List<Item> _out, List<Item> _startingItems, ItemRarity _rarity)
    {
        _out = _startingItems;
        foreach (Item _item in items)
        {
            if (_item.itemRarity == _rarity) _out.Add(_item);

        }
        //return _out;
    }
}