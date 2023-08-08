using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [Header("base")]
    [SerializeField] protected int baseDamage = 10;
    [SerializeField] private float coolDown = 1f;
    private float nextAvailableSkillTime = 0f;
    [SerializeField] protected LayerMask collisionLayer;
    //protected GameObject nextSkill = null;
    protected int skillIndexInNext = 0;
    private bool isRepeating = false;
    private SkillManager manager = null;

    
    private void Update()
    {
        if (isRepeating) {
            SkillTiming();
        }
        
    }

    protected virtual void SkillTiming()
    {
        nextAvailableSkillTime -= Time.deltaTime;

        if (nextAvailableSkillTime <= 0f)
        {
            PerformeSkill();
            nextAvailableSkillTime = coolDown / (PlayerStatsHolder.Instance.TryGetStat(StatType.SkillCooldown)/100f)/*skill attack speed*/;
        }
    }

    public virtual void PerformeSkill()
    {
        if (!isRepeating) { Destroy(gameObject, 10f); }
    }

    public Skill InitializeSkill(int _skillIndexInNext, SkillManager _manager)
    {
        manager = _manager;
        skillIndexInNext = _skillIndexInNext;
        return this;
    }
    //public Skill SetNextSkill(GameObject _g)
    //{
    //    nextSkill = _g;
    //    if(nextSkill != null)
    //    {
    //    Debug.Log("set for:" + gameObject.name +": the next skill - "+ _g.name);
    //    }

    //    return this;
    //}
    public Skill SetIsRepeating(bool _s)
    {
        isRepeating = _s;
        Debug.Log("set for:" + gameObject.name + ": repeating:" + _s);
        return this;
        //might be too late start already called
    }

    protected virtual int GetProjectileAmount()
    {
        //if is the base skill then add the skillProj stat if not, dont
        return isRepeating ? PlayerStatsHolder.Instance.TryGetStat(StatType.SkillProj) : 0;
    }

    protected void TrySpawnNextParticle(Vector3 _pos)
    {
        if (manager.GetNextSkillFromIndex(skillIndexInNext) != null)
        {
            Instantiate(manager.GetNextSkillFromIndex(skillIndexInNext), _pos/* + Vector3.up * 3*/, Quaternion.identity)
                .GetComponent<Skill>()
                .InitializeSkill(skillIndexInNext + 1, manager)
                //started manually beause its not repeating
                .PerformeSkill();
        }
    }
}
