using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDisplaySelectionTooltip : SelectionTooltip
{
    [SerializeField] private ItemDisplay d;
    public override string GetDescription()
    {
        return d.item.GetItemStatsReadable()/*itemDescription*/;
    }
}
