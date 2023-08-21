using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatDisplaySelectionTooltip : SelectionTooltip
{
    private string tooptip = "";
    public void SetTooltip(string _text)
    {
        tooptip = _text;
    }
    public override string GetDescription()
    {
        return tooptip;
    }
}
