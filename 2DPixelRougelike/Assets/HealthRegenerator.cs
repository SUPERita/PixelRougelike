using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRegenerator : MonoBehaviour
{
    [SerializeField] private Health target;
    //[SerializeField] private float delay;

    private void Start()
    {
        InvokeRepeating(nameof(Heal), 1, 5);
    }

    [Button]
    private void Heal()
    {
        target.HealHealth(PlayerStatsHolder.Instance.TryGetStat(StatType.HealthRegen));
    }
}
