using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PoolCollection", menuName = "DataSet/PoolCollection")]
public class PoolCollection : ScriptableObject
{
    [field: SerializeField] public EnemyPoolInstance[] enemyPoolsInstance { get; private set; }
    [field: SerializeField] public ParticlePoolInstance[] particlePoolsInstance { get; private set; }
    [field: SerializeField] public DamageText DmgTextPrefab { get; private set; }
}
