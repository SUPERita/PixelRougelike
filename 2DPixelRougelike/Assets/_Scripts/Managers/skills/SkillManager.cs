using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [SerializeField] private Skill[] skills;

    void Start()
    {
        SpawnSkills();
    }

    private void SpawnSkills()
    {
        
        ClearSkills();
        for (int i = 0; i < skills.Length; i++)
        {

            Instantiate(skills[i].gameObject, transform);
        }
    }

    private void ClearSkills()
    {
         Helpers.DestroyChildren(transform);
    }
}
