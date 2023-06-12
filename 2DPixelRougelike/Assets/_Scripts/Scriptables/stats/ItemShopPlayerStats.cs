using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemShopPlayerStats", menuName = "Custom/ItemShopPlayerStats")]
public class ItemShopPlayerStats : SerializedScriptableObject, PlayerStatsCategory
{

    [SerializeField] private BasePlayerStats basePlayerStats = null;
    [SerializeField] private List<PlayerStatInstance> itemStats = new List<PlayerStatInstance>();
    public List<PlayerStatInstance> GetStatInstances() { return itemStats; }


    private void NotifyUpdatePlayerStats()
    {
        basePlayerStats.PlayerStatsChanged_RequestCompileStats();
    }

    public void SetItemStatsList(List<PlayerStatInstance> list)
    {
        itemStats.Clear();
        itemStats.AddRange(list);
        NotifyUpdatePlayerStats();
    }

    //utils
    [Button]
    public void CreateSampleStat(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            itemStats.Add(new PlayerStatInstance(UnityEngine.Random.Range(0, 2) == 1 ? "speed" : "strength", UnityEngine.Random.Range(0, 99)));
        }
    }
    [Button]
    public void DeleteAllStats()
    {
        itemStats.Clear();

    }
}
