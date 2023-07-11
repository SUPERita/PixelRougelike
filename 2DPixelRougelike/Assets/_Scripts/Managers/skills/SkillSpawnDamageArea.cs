using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSpawnDamageArea : Skill
{
    [Header("DamageArea")]
    [SerializeField] private DamageArea damageArea = null;
    [SerializeField] private float size = 1.0f;
    [SerializeField] private int numberOfAreasToSpawn = 1;

    [Header("spawning")]
    [SerializeField] private bool randomSpawn = false;
    [SerializeField] private Vector2 spawnArea = Vector2.zero;
    //[SerializeField] private bool randomSpawn = true;
    [SerializeField] private bool randomVel = false;
    [SerializeField] private Vector2 spawnVel = Vector2.zero;
    public override void PerformeSkill()
    {
        base.PerformeSkill();
        for (int i = 0; i < numberOfAreasToSpawn; i++) SpawnDamageArea();
    }

    private void SpawnDamageArea()
    {
        //if (randomSpawn)
        //{
            //spawn point
            Vector2 _spawnPoint = spawnArea;
            if(randomSpawn)
            {
                _spawnPoint.x *= Random.Range(-1f, 1f);
                _spawnPoint.y *= Random.Range(-1f, 1f);
            }
            //spawn vel
            Vector2 _spawnVel = spawnVel;
            if(randomVel)
            {
                _spawnVel.x *= Random.Range(-1f, 1f);
                //_spawnVel.y *= Random.Range(-1f, 1f); // never really desired
            }


            Instantiate(damageArea, (Vector2)transform.position+ _spawnPoint, Quaternion.identity)
                .GetComponent<DamageArea>()
                    .InitializeArea(
                        baseDamage + PlayerStatsHolder.Instance.TryGetStat(StatType.SkillDamage),
                        _spawnVel,
                        collisionLayer,
                        size);

        //}
    }
}
