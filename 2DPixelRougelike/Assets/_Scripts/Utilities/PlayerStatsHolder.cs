using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsHolder : StaticInstance<PlayerStatsHolder>
{
    [SerializeField] private PlayerStats playerStats;

    public int TryGetStat(StatType _statName)
    {
        return playerStats.GetStat(_statName);
    }
    public Sprite TryGetStatIcon(StatType _statName)
    {
        return playerStats.GetPlayerStatRaw(_statName).icon;
    }
    public bool IsStatExisting(StatType value)
    {
        return playerStats.StatExists(value);
    }

    public PlayerStats GetPlayerStats() { return playerStats; }
}
