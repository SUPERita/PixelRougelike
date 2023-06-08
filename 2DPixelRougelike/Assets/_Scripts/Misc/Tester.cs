using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Tester : MonoBehaviour
{
    [SerializeField] private PlayerStats stats;
    [SerializeField] private bool compileEveryFrame = false;
    [SerializeField] private int times = 1;
    [Button]
    public void Compilestats()
    {
        for (int i = 0; i < times; i++)
        {
            stats.TEST_GetCompiledStats();
        }
    }

    private void Update()
    {


        //if (compileEveryFrame)
        //{
        //    for (int i = 0; i < times; i++)
        //    {
        //        ResourceSystem.Instance.AddResourceAmount(ResourceType.EnergyNugget, 1);
        //    }
        //    return;
        //}
        
        

        if (compileEveryFrame)
        {
                for (int i = 0; i < times; i++)
                {
                    //stats.TEST_GetCompiledStats();
                    stats.GetStat("speed");
                }
        }
        return;
    }
}
