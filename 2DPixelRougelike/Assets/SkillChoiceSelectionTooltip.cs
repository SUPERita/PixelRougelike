using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillChoiceSelectionTooltip : SelectionTooltip
{
    [SerializeField] private SubButton _source;
    [SerializeField] private SkillCollection skillCollection;
    public override string GetDescription()
    {
        return skillCollection.GetSkillDataFromName(_source._string1).GetColoredDescription();
     
    }
}
