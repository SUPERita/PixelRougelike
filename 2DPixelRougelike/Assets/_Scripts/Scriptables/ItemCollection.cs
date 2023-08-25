using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.IO;
using Unity.Burst.Intrinsics;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemCollection", menuName = "DataSet/ItemCollection")]
public class ItemCollection : ScriptableObject
{
    [SerializeField] private List<Item> items;
    [SerializeField] private string ItemsPath = "";

    public Item GetRandomItem()
    {
        return items[UnityEngine.Random.Range(0, items.Count)];
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
        float _f = UnityEngine.Random.Range(0f, 100f);
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
        return _listGetRandomFrom[UnityEngine.Random.Range(0, _listGetRandomFrom.Count)];
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

#if UNITY_EDITOR // uses AssetDatabase
    [Button, HorizontalGroup("a")] 
    private void LoadItems()
    {
        items = new List<Item>();
        string[] filePaths = Directory.GetFiles(Application.dataPath, ItemsPath, SearchOption.AllDirectories);

        if (filePaths.Length > 0)
        {
            foreach (string _p in filePaths)
            {
                if (!IsFileMeta(_p)) items.Add(AssetDatabase.LoadAssetAtPath<Item>(PathGlobalToLocal(_p)));
                    
            }
        }

        EditorUtility.SetDirty(this);//???! // items, because the items arent the dirty object. this is , whatever it means

        Refresh();
    }
#endif
    [SerializeField, HorizontalGroup("a")] private bool ShowHiddenData = true;
    [Button, ShowIf("ShowHiddenData"), HorizontalGroup("a")]
    private void Refresh()
    {
        int[] _counts = new int[5];
        foreach (Item _item in items)
        {
            switch (_item.itemRarity)
            {
                case ItemRarity.Common:
                    _counts[0] = _counts[0] +1;
                    break;
                case ItemRarity.Uncommon:
                    _counts[1] = _counts[1] + 1;
                    break;
                case ItemRarity.Rare:
                    _counts[2] = _counts[2] + 1;
                    break;
                case ItemRarity.Epic:
                    _counts[3] = _counts[3] + 1;
                    break;
                case ItemRarity.Legendary:
                    _counts[4] = _counts[4] + 1;
                    break;
            }
        }

        ItemOfRarity = 
            $"commons: {_counts[0]}\n"+
            $"uncommons: {_counts[1]}\n" +
            $"rares: {_counts[2]}\n" +
            $"epics: {_counts[3]}\n" +
            $"legenderies: {_counts[4]}";

        StatsPlayedWith = "";
        Array enumValues = Enum.GetValues(typeof(StatType));
        foreach(StatType enumVal in enumValues)
        {
            int positivesForStat = 0;
            int negativesForStat = 0;
            foreach (Item _item in items)
            {
                if (FindStatInItem(_item, enumVal) > 0) positivesForStat++;
                else if (FindStatInItem(_item, enumVal) < 0) negativesForStat++;

            }
            StatsPlayedWith += $"{enumVal}: +{positivesForStat}, -{negativesForStat} \n";
        }
    }

    private int FindStatInItem(Item _item, StatType _type)
    {
        foreach (PlayerStatInstance _s in _item.statInstances)
        {
            if(_s.statName == _type)
            {
                return _s.number;
            }
        }
        return 0;
    }


    [SerializeField, TextArea(6,6), ShowIf("ShowHiddenData")] private string ItemOfRarity = "";
    [SerializeField, TextArea(25, 25), ShowIf("ShowHiddenData")] private string StatsPlayedWith = "";


    private bool IsFileMeta(string filePath)
    {
        string extension = ".meta";
        bool hasMetaExtension = filePath.EndsWith(extension, StringComparison.OrdinalIgnoreCase);
        return hasMetaExtension;
    }
    private string FileName(string filePath)
    {
        string[] pathComponents = filePath.Split('/', '\\');
        string lastPart = pathComponents[pathComponents.Length - 1];
        return lastPart;
    }
    private string PathGlobalToLocal(string filePath)
    {
        string keyword = "Assets";
        int startIndex = filePath.IndexOf(keyword);

        if (startIndex != -1)
        {
            return filePath.Substring(startIndex);
        }
        else
        {
            Debug.LogError("Could not find the 'Assets' keyword in the file path.");
            return string.Empty;
        }
    }

}