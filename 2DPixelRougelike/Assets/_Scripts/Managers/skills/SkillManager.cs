using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [SerializeField] private SkillCollection _skillCollection;
    //[SerializeField] private Skill[] skills;

    void Start()
    {
        SpawnSkills();
    }

    private void SpawnSkills()
    {
        //clear previous
        ClearSkills();

        //load selected
        GameObject[] selectedSkills = SkillSelection.GetSelectedSavedSkills(_skillCollection);
        if(selectedSkills.Length == 0) { return; }
        for (int i = 0; i < selectedSkills.Length; i++)
        {

            Instantiate(selectedSkills[i], transform);
        }
    }

    private void ClearSkills()
    {
         Helpers.DestroyChildren(transform);
    }
}
