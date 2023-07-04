using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;

[CreateAssetMenu(fileName = "PassivePlayerStats", menuName = "Custom/PassivePlayerStats")]
public class PassivePlayerStats : SerializedScriptableObject, PlayerStatsCategory
{
    [SerializeField] private BasePlayerStats basePlayerStats = null;
    [SerializeField] private List<PlayerStatInstance> samples = new List<PlayerStatInstance>();
    public List<PlayerStatInstance> GetStatInstances() { return samples; }


    private void NotifyUpdatePlayerStats()
    {
        basePlayerStats.PlayerStatsChanged_RequestCompileStats();
    }

    public void SetPassiveStatsList(List<PlayerStatInstance> list)
    {
        samples.Clear();
        samples.AddRange(list);
        NotifyUpdatePlayerStats();
    }

    //utils
    [Button]
    public void CreateSampleStat(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            //samples.Add(new PlayerStatInstance(UnityEngine.Random.Range(0, 2) == 1? "speed":"strength", UnityEngine.Random.Range(0, 99)));
        }
    }
    [Button]
    public void DeleteAllStats()
    {
        samples.Clear();

    }
}




