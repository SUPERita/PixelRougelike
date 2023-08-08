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
    [SerializeField] private bool burstRotation = false;
    [SerializeField] private Vector2[] spawnVelOptions = null;
    public override void PerformeSkill()
    {
        base.PerformeSkill();
        for (int i = 0; i < GetProjectileAmount(); i++) SpawnDamageArea(i);
    }

    private void SpawnDamageArea(int _spawnIndex)
    {
        //if (randomSpawn)
        //{
        //spawn point
        Vector2 _spawnPoint = spawnArea;
        if (randomSpawn)
        {
            _spawnPoint.x *= Random.Range(-1f, 1f);
            _spawnPoint.y *= Random.Range(-1f, 1f);
        }
        //spawn vel
        Vector2 _spawnVel = spawnVel;
        if (randomVel)
        {
            _spawnVel.x *= Random.Range(-1f, 1f);
            //_spawnVel.y *= Random.Range(-1f, 1f); // never really desired
        }
        if (spawnVelOptions.Length > 0)
        {
            _spawnVel.x = spawnVelOptions[_spawnIndex % spawnVelOptions.Length].x;
            _spawnVel.y = spawnVelOptions[_spawnIndex % spawnVelOptions.Length].y;
            if (_spawnIndex >= spawnVelOptions.Length)
            {
                _spawnVel.x *= Random.Range(-1f, 1f);
                _spawnVel.y *= Random.Range(-1f, 1f);
            }
        }


        Vector3 _spawnRot = new Vector3(0f, 0f, 0f);
        if (burstRotation)
        {
            _spawnRot.z = _spawnIndex * 360f / GetProjectileAmount();
        }

        Instantiate(damageArea, (Vector2)transform.position + _spawnPoint, Quaternion.Euler(_spawnRot))
            .GetComponent<DamageArea>()
                .InitializeArea(
                    baseDamage + PlayerStatsHolder.Instance.TryGetStat(StatType.SkillDamage),
                    _spawnVel,
                    collisionLayer,
                    size,
                    this);

        //}
    }

    protected override int GetProjectileAmount()
    {
        return numberOfAreasToSpawn + base.GetProjectileAmount();
    }

    public void AreaHitSomething(Vector3 _pos)
    {
        TrySpawnNextParticle(_pos);
    }
}
