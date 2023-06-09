using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsHolder : StaticInstance<PlayerStatsHolder>
{
    [SerializeField] private PlayerStats playerStats;

    public int TryGetStat(string _statName)
    {
        return playerStats.GetStat(_statName);
    }
}
