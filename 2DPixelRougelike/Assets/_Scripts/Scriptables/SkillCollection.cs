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
    [field:SerializeField] public string _skillName { get; private set; }
    [field: SerializeField] public Skill _skill { get; private set; }
    [field: SerializeField] public int _skillCost { get; private set; }
    [field: SerializeField] public string _skillDesctiption { get; private set; }
    [field: SerializeField, PreviewField] public Sprite _skillIcon { get; private set; }
}
