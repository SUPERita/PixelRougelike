using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyCollection", menuName = "DataSet/EnemyCollection")]
public class EnemyCollection : ScriptableObject
{
    [field: SerializeField] public EnemyWeightPair[] enemys { get; private set; }
    private List<EnemyWeightPair> enemysRateList;

    public string[] GetWaveOfWeight(int _weight)
    {
        //initializing
        List<string> list = new List<string>();
        int _leftWeight = _weight;
        //creating enemy rate list
        enemysRateList = new List<EnemyWeightPair>();
        foreach (EnemyWeightPair _pair in enemys)
        {
            for (int i = 0; i < _pair.enemyRelativeSpawnRate; i++)
            {
                enemysRateList.Add(_pair);
            }
        }

        //starting the wave list Creation
        int _counter = 0;
        while(_leftWeight > 0 && _counter < 5000)
        {
            _counter++;
            EnemyWeightPair _pair = enemysRateList[Random.Range(0, enemysRateList.Count)];
            //if "can affford" add
            if(_leftWeight - _pair.enemy.GetEnemyWeight() >= 0)
            {
                list.Add(_pair.enemy.gameObject.name);
                _leftWeight -= _pair.enemy.GetEnemyWeight();
            }

        }

        if(_counter > 5000) Debug.Log("couldent fill all of the weight");

        return list.ToArray();
    }
}

[System.Serializable]
public struct EnemyWeightPair
{
    [field: SerializeField] public Enemy enemy { get; private set; }
    
    [field: SerializeField] public int enemyRelativeSpawnRate { get; private set; }
}
