using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEmitParticle : Skill
{
    [Header("particle emmiter")]
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private float emmisionDuration = .5f;

    public override void PerformeSkill()
    {
        base.PerformeSkill();
        StartCoroutine(SpawnParticles());
        //SpawnParticles();
    }

    private IEnumerator SpawnParticles()
    {
       //Debug.Log("should have shop");
        _particleSystem.Play();
        yield return new WaitForSeconds(emmisionDuration);
        _particleSystem.Stop();
    }

    private void OnParticleCollision(GameObject other)
    {
        //already in the right layer
        //if didnt collider with enemy layer, return
        //if (((1 << collision.gameObject.layer) & collisionLayer) == 0) { return; }

        //if (collision.gameObject.layer.value != collisionLayer.value) { return; }
        if (other.TryGetComponent(out IDamageable _d))
        {
            _d.TakeDamage(baseDamage + PlayerStatsHolder.Instance.TryGetStat("skill damage"));
        }
    }
}
