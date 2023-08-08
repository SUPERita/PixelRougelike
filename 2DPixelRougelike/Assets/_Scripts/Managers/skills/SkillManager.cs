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


    GameObject[] selectedSkills = null;
    private void SpawnSkills()
    {
        //clear previous
        ClearSkills();

        //load selected
        selectedSkills = SkillSelection.GetSelectedSavedSkills(_skillCollection);
        int _numberOfSkills = PlayerStatsHolder.Instance.TryGetStat(StatType.SkillCap);//second layer of defence
        if (selectedSkills.Length == 0) { return; }


        //spawn the first skill
        Instantiate(selectedSkills[0], transform)
            .GetComponent<Skill>()
            .SetIsRepeating(true)
            .InitializeSkill(0, this);

    }

    private void ClearSkills()
    {
         Helpers.DestroyChildren(transform);
    }

    public GameObject GetNextSkillFromIndex(int _index)
    {
        if(_index+1 >= selectedSkills.Length) { return null; }
        return selectedSkills[_index+1];
    }
}
