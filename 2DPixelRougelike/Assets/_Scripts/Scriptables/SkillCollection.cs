using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}

[Serializable]
public struct SkillNamePair
{
    [field:SerializeField] public string _skillName { get; private set; }
    [field: SerializeField] public Skill _skill { get; private set; }
    [field: SerializeField] public Sprite _skillIcon { get; private set; }
}
