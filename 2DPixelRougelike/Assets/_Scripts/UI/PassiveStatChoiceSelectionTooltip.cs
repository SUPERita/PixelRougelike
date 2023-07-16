using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveStatChoiceSelectionTooltip : SelectionTooltip
{
    [SerializeField] private PassiveUpgradeChoice choice;
    public override string GetDescription()
    {
        return PlayerStatsHolder.Instance.TryGetStatDesc(choice.statname);
    }
}
