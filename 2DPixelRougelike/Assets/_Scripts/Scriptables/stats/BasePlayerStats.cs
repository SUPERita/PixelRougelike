using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using System;
using System.Linq;

[CreateAssetMenu(fileName = "BasePlayerStats", menuName = "Custom/BasePlayerStats")]
public class BasePlayerStats : SerializedScriptableObject
{
    [SerializeField] private PlayerStats playerStats = null;
    [SerializeField] private PassivePlayerStats passivePlayerStats = null;
    [SerializeField] private MidRunPlayerStats midRunPlayerStats = null;
    [SerializeField] private ItemShopPlayerStats itemShopPlayerStats = null;

    [SerializeField] private Dictionary<string, PlayerStat> baseStats = new Dictionary<string, PlayerStat>();

    //stats passing
    private Dictionary<string, PlayerStat> compiledStats = new Dictionary<string, PlayerStat>();
    public Dictionary<string, PlayerStat> PlayerStats_GetCompiledStats()
    {
        //Debug.Log("reseted");
        //set base stats as the base
        compiledStats = baseStats.ToDictionary(entry => entry.Key, entry => entry.Value); ;

        //more passes and calls
        UpgradePass(passivePlayerStats.GetStatInstances());
        UpgradePass(midRunPlayerStats.GetStatInstances());
        UpgradePass(itemShopPlayerStats.GetStatInstances());

        //1 x
        //compiledStats.TryGetValue("speed", out tmpStat);
        //tmpStat.BaseStats_SetStatValue_USEONLYATBaseStats(-1);
        //2 x 
        //compiledStats["speed"].BaseStats_SetStatValue_USEONLYATBaseStats(-1);
        //3 v
        //compiledStats["speed"] = _samplePlayerStat

        return compiledStats;
    }

    private void UpgradePass(List<PlayerStatInstance> _statsForPass)
    {

        foreach (PlayerStatInstance _statToAdd in _statsForPass)
        {
            
            //check if has value
            if (!compiledStats.TryGetValue(_statToAdd.statName, out tmpStat)) { continue; }

            tmpStat.BaseStats_SetStatValue_USEONLYATBaseStats(
                //CurrentValue + valueToAdd
                /*compiledStats[_statToAdd.statName]*/tmpStat.num +
                _statToAdd.number);

            //set the compiled value To
            compiledStats[_statToAdd.statName] = tmpStat;
            

            
            //old
            /*
            //set the compiled value To
            compiledStats[_statToAdd.statName].BaseStats_SetStatValue_USEONLYATBaseStats(
                //CurrentValue + valueToAdd
                compiledStats[_statToAdd.statName].num +
                _statToAdd.number);
            */
            /*
            Debug.Log(_statToAdd.statName + "   " + _statToAdd.number);//it does find it

            Debug.Log(_statToAdd.statName + " compiled should be =   " + (compiledStats[_statToAdd.statName].num +
                _statToAdd.number));//it does find it
            */

            //Debug.Log(_statToAdd.statName + "is actually = " + compiledStats[_statToAdd.statName].num);//it does find it
        }
    }
    private PlayerStat tmpStat;

    public void PlayerStatsChanged_RequestCompileStats()
    {
        playerStats.BasePlayerStats_RequestStatsCompile();
    }



    //utils
    [Button]
    public void CreateSampleStat(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            baseStats.Add("tmp" + UnityEngine.Random.Range(0, 999999), new PlayerStat());
        }
    }
    [Button]
    public void DeleteAllStats()
    {
        baseStats = new Dictionary<string, PlayerStat>();
        
    }
}

[Serializable]
public struct PlayerStatInstance
{
    [field: SerializeField] public int number { get; private set; }
    [field: SerializeField] public string statName { get; private set; }

    public PlayerStatInstance(string _s, int _n)
    {
        number = _n;
        statName = _s;
    }
}

public interface PlayerStatsCategory
{
    List<PlayerStatInstance> GetStatInstances();
}


