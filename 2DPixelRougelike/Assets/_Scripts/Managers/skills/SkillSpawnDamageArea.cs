using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSpawnDamageArea : Skill
{
    [Header("DamageArea")]
    [SerializeField] private DamageArea damageArea = null;
    [SerializeField] private float size = 1.0f;
    [SerializeField] private bool dieOnHit = false;
    [SerializeField] private float areaLifetime = 1.0f;
    [Header("spawning")]
    [SerializeField] private Vector2 spawnArea = Vector2.zero;
    //[SerializeField] private bool randomSpawn = true;
    [SerializeField] private bool randomVel = false;
    [SerializeField] private Vector2 spawnVel = Vector2.zero;
    public override void PerformeSkill()
    {
        base.PerformeSkill();
        SpawnDamageArea();
    }

    private void SpawnDamageArea()
    {
        //if (randomSpawn)
        //{
            //spawn point
            Vector2 _spawnPoint = Vector2.zero;
            _spawnPoint.x = spawnArea.x * Random.Range(-1f, 1f);
            _spawnPoint.y = spawnArea.y * Random.Range(-1f, 1f);
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
                        baseDamage + PlayerStatsHolder.Instance.TryGetStat("skill damage"),
                        _spawnVel,
                        collisionLayer,
                        areaLifetime,
                        size,
                        dieOnHit);

        //}
    }
}
