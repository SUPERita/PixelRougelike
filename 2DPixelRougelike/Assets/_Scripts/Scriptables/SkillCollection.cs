using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
[CreateAssetMenu(fileName = "SkillCollection", menuName = "DataSet/SkillCollection")]
public class SkillCollection : ScriptableObject
{
    [field:SerializeField] public SkillNamePair[] skillNamePairs { get; private set; }

    public GameObject GetSkillFromName(string _name)
    {
        foreach (var pair in skillNamePairs)
        {
            if(pair._skillName == _name)
            {
                return pair._skill.gameObject;
            }
        }

        Debug.LogError("asked for nonexsistane skill");
        return null;
    }
    public SkillNamePair GetSkillDataFromName(string _name)
    {
        foreach (var pair in skillNamePairs)
        {
            if (pair._skillName == _name)
            {
                return pair;
            }
        }

        Debug.LogError("asked for nonexsistane skill");
        return new SkillNamePair();
    }
}

[Serializable]
public struct SkillNamePair
{
    [field:SerializeField, HorizontalGroup("b"), LabelWidth(75)] public string _skillName { get; private set; }
    [field: SerializeField, HorizontalGroup("b"), LabelWidth(75)] public int _skillCost { get; private set; }
    [field: SerializeField, HorizontalGroup("b"), LabelWidth(40)] public Skill _skill { get; private set; }
    [field: SerializeField, HorizontalGroup("a"), LabelWidth(75), TextArea(3, 5)] private string _skillDesctiption ;
    [field: SerializeField, PreviewField, HorizontalGroup("a", 50), LabelWidth(75)] public Sprite _skillIcon { get; private set; }

    public string GetColoredDescription()
    {
        string _out = _skillDesctiption;
        _out = _out.Replace("{stpAbl}", "<color=yellow>stops ability chain</color>");
        return _out;
    }
}
