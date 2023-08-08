using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DamageArea : MonoBehaviour
{
    [field: SerializeField] public Rigidbody2D rb { get; private set; }
    [SerializeField] private Collider2D collider;
    [SerializeField] private bool dieOnHit = false;
    [SerializeField] private float areaLifetime = 5.0f;
    [SerializeField] private float areaDestroySpeed = .25f;
    //private bool dieOnHit = false;
    private SkillSpawnDamageArea _emmiter;

    private int damage = 0;
    private LayerMask collisionLayer;

    [Header("zetsy variables")]
    [SerializeField] private bool spawnNextSkillOnDeath = false;
    [SerializeField] private bool spawnNextSkillOnCollide = true;

    public DamageArea InitializeArea(int _damage, Vector3 directionAndMagnitude, LayerMask _collisionLayer, float size, SkillSpawnDamageArea _parentEmmiter)
    {
        //Debug.Log("alive");
        damage = _damage;
        rb.velocity = directionAndMagnitude;
        //RotateSpriteToNormalizedDirection(direction);
        transform.localScale = Vector3.one * size;
        _emmiter = _parentEmmiter;
        //dieOnHit = _dieOnHit;
        //transform.rotation = Quaternion.LookRotation(direction);

        //float angle = Mathf.Atan2(directionAndMagnitude.y, directionAndMagnitude.x) * Mathf.Rad2Deg;
        //Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        //transform.rotation = q;

        collisionLayer = _collisionLayer;
        Invoke(nameof(DestroyArea), areaLifetime);

        return this;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if didnt collider with enemy layer, return
        if (((1 << collision.gameObject.layer) & collisionLayer) == 0) { return; }
        //if (collision.gameObject.layer.value != collisionLayer.value) { return; }
        if (collision.TryGetComponent(out IDamageable _d))
        {
            _d.TakeDamage(damage);
            if (_emmiter && spawnNextSkillOnCollide) { _emmiter.AreaHitSomething(collision.transform.position); }
            else if(spawnNextSkillOnCollide) { Debug.LogError("ayo wft why doesnt he has his parent emmiter he cant spawn projectiles, most likely he spawned directry on an enemy, maybe just call it with a delay of a frame or something"); }
        }


        if(dieOnHit) DestroyArea();
    }

    private void DestroyArea()
    {
        // probably something with pooling
        if (gameObject == null) { return; }

        if (spawnNextSkillOnDeath) { _emmiter.AreaHitSomething(transform.position); }
        collider.enabled = false;
        rb.velocity = Vector2.zero;
        transform.DOScale(0, areaDestroySpeed).OnComplete(()=> Destroy(gameObject));
        
    }

    private void RotateSpriteToNormalizedDirection(Vector3 direction)
    {
        Vector3 desiredDirection = direction.z * Vector3.forward;
        if (desiredDirection != Vector3.zero)
        {
            Quaternion desiredRotation = Quaternion.LookRotation(desiredDirection);
            transform.rotation = desiredRotation;
        }

    }
}
