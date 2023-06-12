using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

[CreateAssetMenu(fileName = "MidRunPlayerStats", menuName = "Custom/MidRunPlayerStats")]
public class MidRunPlayerStats : SerializedScriptableObject, PlayerStatsCategory
{
    [SerializeField] private BasePlayerStats basePlayerStats = null;
    [SerializeField] private List<PlayerStatInstance> midRunStats = new List<PlayerStatInstance>();
    public List<PlayerStatInstance> GetStatInstances() { return midRunStats; }


    private void NotifyUpdatePlayerStats()
    {
        basePlayerStats.PlayerStatsChanged_RequestCompileStats();
    }

    public void ResetMidRunStats()
    {
        midRunStats.Clear();
    }

    //utils
    [Button]
    public void CreateMidRunStat(string _statName, int _val)
    {

        midRunStats.Add(new PlayerStatInstance(_statName, _val));
        NotifyUpdatePlayerStats();
    }
    [Button]
    public void DeleteAllStats()
    {
        midRunStats.Clear();

    }
}




