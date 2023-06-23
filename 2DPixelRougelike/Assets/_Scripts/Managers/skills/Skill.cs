using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [Header("base")]
    [SerializeField] protected int damage = 10;
    [SerializeField] private float coolDown = 1f;
    private float nextAvailableSkillTime = 0f;
    [SerializeField] protected LayerMask collisionLayer;

    private void Update()
    {
        SkillTiming();
    }

    protected virtual void SkillTiming()
    {
        nextAvailableSkillTime -= Time.deltaTime;

        if (nextAvailableSkillTime <= 0f)
        {
            PerformeSkill();
            nextAvailableSkillTime = coolDown;
        }
    }

    public virtual void PerformeSkill()
    {
        //Debug.Log("done skill: " + name);
    }
}
