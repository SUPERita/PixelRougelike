using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using System.Linq;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Custom/PlayerStats")]
public class PlayerStats : SerializedScriptableObject
{
    public BasePlayerStats basePlayerStats = null;

    [Button]
    private void GetCompiledStats()
    {
        //copies the dictionary?
        stats = basePlayerStats.PlayerStats_GetCompiledStats().ToDictionary(entry => entry.Key, entry => entry.Value); 
    }
    public void TEST_GetCompiledStats()
    {
        GetCompiledStats();
    }

    public void BasePlayerStats_RequestStatsCompile()
    {
        Debug.Log("called compile");
        GetCompiledStats();
    }

    //*
    /*
    private PlayerStat _p;
    [Button]
    public void SetPlayerSpeedStat(int _a)
    {
        // well damm
        //refernce
        //myDictionary[key] = newValue;, not myDictionary[key].number = newValue; // you need to change the whole value itself

        Debug.Log(" before "+stats["speed"].num);
        stats.TryGetValue("speed", out _p);
        _p.BaseStats_SetStatValue_USEONLYATBaseStats(_a);
        stats["speed"] = _p;

        Debug.Log(" after " + stats["speed"].num);
    }
    */

    [SerializeField] private Dictionary<string, PlayerStat> stats = new Dictionary<string, PlayerStat>();
    public int GetStat(string _statName)
    {
        return stats[_statName].num; 
    }

}

[Serializable]
public struct PlayerStat
{ 
    [HorizontalGroup("b")]
    [field:SerializeField] public string statName { get; private set; }
    [HorizontalGroup("b")]
    [field: SerializeField] public int num { get; private set; }
    [HorizontalGroup("a")]
    [field: SerializeField] public string description { get; private set; }
    [HorizontalGroup("a")]
    [field: SerializeField] public Sprite icon { get; private set; }

    public void BaseStats_SetStatValue_USEONLYATBaseStats(int _to)
    {
        //Debug.Log("hello i set my stats to = " + _to);
        num = _to;
        //Debug.Log("num = " + num);
    }
}
