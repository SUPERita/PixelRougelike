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
        int _numberOfSkills = PlayerStatsHolder.Instance.TryGetStat(StatType.SkillCap);//second layer of defence
        if (selectedSkills.Length == 0) { return; }
        for (int i = 0; i < _numberOfSkills; i++)
        {
            //if has more skill capacity than selected skills
            if (selectedSkills.Length <= i ) { break; }
            
            Instantiate(selectedSkills[i], transform);
            
        }
    }

    private void ClearSkills()
    {
         Helpers.DestroyChildren(transform);
    }
}
