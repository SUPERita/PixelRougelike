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
    public event Action OnPlayerStatsChanged;
    [Button]
    private void GetCompiledStats()
    {
        //copies the dictionary?
        stats2 = basePlayerStats.PlayerStats_GetCompiledStats().ToDictionary(entry => entry.Key, entry => entry.Value);
        
        //tells everyone something changed
        OnPlayerStatsChanged?.Invoke();
    }
    public void TEST_GetCompiledStats()
    {
        GetCompiledStats();
    }

    public void BasePlayerStats_RequestStatsCompile()
    {
        //Debug.Log("called compile");
        GetCompiledStats();
    }
    public void Player_RequestStatsCompile()
    {
        //Debug.Log("called compile");
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

        Debug.Log(" before "+stats["speed"].value);
        stats.TryGetValue("speed", out _p);
        _p.BaseStats_SetStatValue_USEONLYATBaseStats(_a);
        stats["speed"] = _p;

        Debug.Log(" after " + stats["speed"].value);
    }
    */
    /*
    [SerializeField] private Dictionary<string, PlayerStat> stats = new Dictionary<string, PlayerStat>();
    public Dictionary<string, PlayerStat> GetRawStats() { return stats; }
    public int GetStat(string _statName)
    {
        return stats[_statName].value; 
    }
    public bool StatExists(string _statName)
    {
        return basePlayerStats.GetBaseStatsForValidation().ContainsKey(_statName);
    }
    */
    //[SerializeField] private Dictionary<string, PlayerStat> stats = new Dictionary<string, PlayerStat>();
    [SerializeField] private Dictionary<StatType, PlayerStat> stats2 = new Dictionary<StatType, PlayerStat>();
    public Dictionary<StatType, PlayerStat> GetRawStats() { return stats2; }
    public int GetStat(StatType _statName)
    {
        return stats2[_statName].value;
    }
    public PlayerStat GetPlayerStatRaw(StatType _statName)
    {
        return stats2[_statName];
    }
    public bool StatExists(StatType _statName)
    {
        return basePlayerStats.GetBaseStatsForValidation().ContainsKey(_statName);
    }


}

[Serializable]
public struct PlayerStat
{ 
    [HorizontalGroup("b")]
    [field:SerializeField] public string statName { get; private set; }
    [HorizontalGroup("b")]
    [field: SerializeField] public int value { get; private set; }
    [HorizontalGroup("a")]
    [field: SerializeField] public string description { get; private set; }
    [HorizontalGroup("a")]
    [field: SerializeField] public Sprite icon { get; private set; }
    [HorizontalGroup("a")]
    [field: SerializeField] public bool showInDisplay { get; private set; }
    public void BaseStats_SetStatValue_USEONLYATBaseStats(int _to)
    {
        //Debug.Log("hello i set my stats to = " + _to);
        value = _to;
        //Debug.Log("value = " + value);
    }
}

public enum StatType
{
    //DONOT CHANGE THEIR ORDER
    
    MaxHealth = 0,
    AttackSpeed = 1,
    MoveSpeed = 2,
    Strength = 3,
    SkillCap = 4,
    SkillDamage = 5,
    WeaponDamage = 6,

    
    Dodge = 7,//cap at 40
    Armor = 8,//cap at 40
    XPGain = 9,
    MoneyGain = 10,//half implemented
    WeaponAttackSpeed = 11,
    SkillCooldown = 12,
    MeleeDamage = 13,
    PickUpRange = 14,
    SkillProj = 15


}
